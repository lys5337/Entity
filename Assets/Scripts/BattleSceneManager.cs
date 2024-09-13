using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
namespace TJ
{
    public class BattleSceneManager : MonoBehaviour
    {
        [Header("Cards")]
        public List<Card> deck;
        public List<Card> drawPile = new List<Card>();
        public List<Card> cardsInHand = new List<Card>();
        public List<Card> discardPile = new List<Card>();
        public CardUI selectedCard;
        public List<CardUI> cardsInHandGameObjects = new List<CardUI>();

        [Header("Stats")]
        public Fighter cardTarget;
        public Fighter player;
        public int maxEnergy;
        public int energy;
        public int drawAmount = 5;
        public Turn turn;
        public enum Turn { Player, Enemy };

        [Header("UI")]
        public Button endTurnButton;
        public TMP_Text drawPileCountText;
        public TMP_Text discardPileCountText;
        public TMP_Text energyText;
        public Transform topParent;
        public Transform enemyParent;
        public EndScreen endScreen;

        [Header("Enemies")]
        public List<Enemy> enemies = new List<Enemy>();
        List<Fighter> enemyFighters = new List<Fighter>();
        public GameObject[] possibleEnemies;
        public GameObject[] possibleElites;
        public GameObject[] possibleBosses; // 보스 전투를 위한 새로운 배열 추가
        bool eliteFight;
        bool bossFight; // 보스 전투 여부를 추적하는 변수 추가
        public GameObject birdIcon;
        CardActions cardActions;
        GameManager gameManager;
        PlayerStatsUI playerStatsUI;
        public Animator banner;
        public TMP_Text turnText;
        public GameObject gameover;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardActions = GetComponent<CardActions>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
            //endScreen = FindObjectOfType<EndScreen>();
        }
        public void StartHallwayFight()
        {
            BeginBattle(possibleEnemies);
        }
        public void StartEliteFight()
        {
            eliteFight = true;
            BeginBattle(possibleElites);
        }

        public void StartBossFight() // 새로운 메서드 추가
        {
            bossFight = true;
            BeginBattle(possibleBosses);
        }

        public void BeginBattle(GameObject[] prefabsArray)
        {
            turnText.text = "Player's Turn";
            banner.Play("bannerOut");

            //playerIcon.SetActive(true);

            GameObject newEnemy = Instantiate(prefabsArray[Random.Range(0, prefabsArray.Length)], enemyParent);
            //endScreen = FindObjectOfType<EndScreen>();
            if (endScreen != null)
                endScreen.gameObject.SetActive(false);

            Enemy[] eArr = FindObjectsOfType<Enemy>();
            enemies = new List<Enemy>();

            #region discard hand
            foreach (Card card in cardsInHand)
            {
                DiscardCard(card);
            }
            foreach (CardUI cardUI in cardsInHandGameObjects)
            {
                cardUI.gameObject.SetActive(false);
                //cardsInHand.Remove(cardUI.card);
            }
            #endregion

            discardPile = new List<Card>();
            drawPile = new List<Card>();
            cardsInHand = new List<Card>();

            foreach (Enemy e in eArr) { enemies.Add(e); }
            foreach (Enemy e in eArr) { enemyFighters.Add(e.GetComponent<Fighter>()); }
            foreach (Enemy e in enemies) e.DisplayIntent();

            discardPile.AddRange(gameManager.playerDeck);
            ShuffleCards();
            DrawCards(drawAmount);
            energy = maxEnergy;
            energyText.text = energy.ToString();

            #region relic checks

            if (gameManager.PlayerHasRelic("PreservedInsect") && eliteFight)
                enemyFighters[0].currentHealth = (int)(enemyFighters[0].currentHealth * 0.25);

            if (gameManager.PlayerHasRelic("Anchor"))
                player.AddBlock(10);

            if (gameManager.PlayerHasRelic("Lantern"))
                energy += 1;

            if (gameManager.PlayerHasRelic("Marbles"))
                enemyFighters[0].AddBuff(Buff.Type.vulnerable, 1);

            if (gameManager.PlayerHasRelic("Bag of Preparation"))
                DrawCards(2);

            if (gameManager.PlayerHasRelic("Varja"))
                player.AddBuff(Buff.Type.strength, 1);

            if (bossFight && gameManager.PlayerHasRelic("BurningBlood")) // 보스 전투에서만 발동되는 유물 처리
                player.currentHealth += 10;

            #endregion

            //if (enemies[0].bird)
            //    birdIcon.SetActive(true);
        }
        public void ShuffleCards()
        {
            discardPile.Shuffle();
            drawPile = discardPile;
            discardPile = new List<Card>();
            discardPileCountText.text = discardPile.Count.ToString();
        }

        public void DrawCards(int amountToDraw)
        {
            int cardsDrawn = 0;

            while (cardsDrawn < amountToDraw && cardsInHand.Count <= 10)
            {
                if (drawPile.Count < 1)
                {
                    ShuffleCards(); // 카드가 남지 않았으면 셔플
                }

                if (drawPile.Count > 0)
                {
                    Card drawnCard = drawPile[0];
                    cardsInHand.Add(drawnCard);          // 손에 카드 추가
                    DisplayCardInHand(drawnCard);        // 카드를 UI에 표시
                    drawPile.RemoveAt(0);                // 덱에서 카드 제거
                    drawPileCountText.text = drawPile.Count.ToString();
                    cardsDrawn++;
                }
                else
                {
                    Debug.LogError("덱에 카드가 없습니다.");
                    break;
                }
            }
        }

        public void DisplayCardInHand(Card card)
        {
            if (cardsInHandGameObjects == null || cardsInHandGameObjects.Count == 0)
            {
                Debug.LogError("cardsInHandGameObjects 배열이 비어 있습니다.");
                return;
            }

            // 카드를 추가하려고 하는 인덱스가 유효한지 확인
            if (cardsInHand.Count - 1 < 0 || cardsInHand.Count > cardsInHandGameObjects.Count)
            {
                Debug.LogError("카드를 추가할 수 있는 인덱스가 유효하지 않습니다.");
                return;
            }

            CardUI cardUI = cardsInHandGameObjects[cardsInHand.Count - 1];

            if (cardUI == null)
            {
                Debug.LogError("CardUI가 null입니다.");
                return;
            }

            cardUI.LoadCard(card);  // LoadCard에서 null 체크 추가
            cardUI.gameObject.SetActive(true);
        }


        public void PlayCard(CardUI cardUI)
        {
            // 공격 카드의 경우 타겟이 필요함
            if (cardUI.card.cardType == Card.CardType.Attack && cardTarget == null)
            {
                Debug.LogError("타겟이 설정되지 않았습니다. 공격 카드를 사용할 수 없습니다.");
                cardUI.ResetCardPosition();  // 카드 사용을 취소하고 원래 위치로 복귀
                return;
            }

            // GoblinNob이 분노 상태일 때 처리
            if (cardUI.card.cardType != Card.CardType.Attack && enemies[0].GetComponent<Fighter>().enrage.buffValue > 0)
            {
                enemies[0].GetComponent<Fighter>().AddBuff(Buff.Type.strength, enemies[0].GetComponent<Fighter>().enrage.buffValue);
            }

            // 카드 액션 수행
            cardActions.PerformAction(cardUI.card, cardTarget);

            // 에너지 차감 및 UI 업데이트
            energy -= cardUI.card.GetCardCostAmount();
            energyText.text = energy.ToString();

            // 카드 효과 및 처리
            Instantiate(cardUI.discardEffect, cardUI.transform.position, Quaternion.identity, topParent);
            selectedCard = null;
            cardUI.gameObject.SetActive(false); // 카드 비활성화
            cardsInHand.Remove(cardUI.card);    // 카드 손에서 제거
            DiscardCard(cardUI.card);           // 카드 버리기
        }


        public void DiscardCard(Card card)
        {
            discardPile.Add(card);
            discardPileCountText.text = discardPile.Count.ToString();
        }
        public void ChangeTurn()
        {
            if (turn == Turn.Player)
            {
                turn = Turn.Enemy;
                endTurnButton.enabled = false;

                #region discard hand
                foreach (Card card in cardsInHand)
                {
                    DiscardCard(card);
                }
                foreach (CardUI cardUI in cardsInHandGameObjects)
                {
                    if (cardUI.gameObject.activeSelf)
                        Instantiate(cardUI.discardEffect, cardUI.transform.position, Quaternion.identity, topParent);

                    cardUI.gameObject.SetActive(false);
                    cardsInHand.Remove(cardUI.card);
                }
                #endregion

                foreach (Enemy e in enemies)
                {
                    if (e.thisEnemy == null)
                        e.thisEnemy = e.GetComponent<Fighter>();

                    //reset block
                    e.thisEnemy.currentBlock = 0;
                    e.thisEnemy.fighterHealthBar.DisplayBlock(0);
                }

                player.EvaluateBuffsAtTurnEnd();
                StartCoroutine(HandleEnemyTurn());
            }
            else
            {
                foreach (Enemy e in enemies)
                {
                    e.DisplayIntent();
                }
                turn = Turn.Player;

                //reset block
                player.currentBlock = 0;
                player.fighterHealthBar.DisplayBlock(0);
                energy = maxEnergy;
                energyText.text = energy.ToString();

                endTurnButton.enabled = true;
                DrawCards(drawAmount);

                turnText.text = "Player's Turn";
                banner.Play("bannerOut");
            }
        }
        private IEnumerator HandleEnemyTurn()
        {
            turnText.text = "Enemy's Turn";
            banner.Play("bannerIn");

            yield return new WaitForSeconds(1.5f);

            foreach (Enemy enemy in enemies)
            {
                enemy.midTurn = true;
                enemy.TakeTurn();
                while (enemy.midTurn)
                    yield return new WaitForEndOfFrame();
            }
            Debug.Log("Turn Over");
            ChangeTurn();
        }
        public void EndFight(bool win)
        {
            // 승리 여부에 따른 게임오버 UI 설정
            if (!win && gameover != null)
            {
                gameover.SetActive(true);
            }

            // gameManager와 player가 null이 아닌지 확인
            if (gameManager != null && player != null)
            {
                // BurningBlood 유물 체크 후 체력 회복 처리
                if (gameManager.PlayerHasRelic("BurningBlood"))
                {
                    player.currentHealth += 6;
                    if (player.currentHealth > player.maxHealth)
                    {
                        player.currentHealth = player.maxHealth;
                    }

                    player.UpdateHealthUI(player.currentHealth);
                }

                // 버프 초기화
                player.ResetBuffs();
 
                // 엔드 스크린 처리
                HandleEndScreen();

                // 층수 업데이트
                gameManager.UpdateFloorNumber();

                // enemies 배열이 null인지, 첫 번째 적이 있는지 확인
                if (enemies != null && enemies.Count > 0 && enemies[0] != null)
                {
                    // 골드 보상 처리
                    int goldReward = enemies[0].goldDrop;
                    gameManager.UpdateGoldNumber(goldReward);

                    // birdIcon이 null이 아닌지 확인 후 처리
                    if (enemies[0].bird && birdIcon != null)
                    {
                        birdIcon.SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError("적 정보(enemies)가 null이거나 적 목록이 비어 있습니다.");
                }
            }
            else
            {
                Debug.LogError("gameManager 또는 player가 null입니다.");
            }
        }


        public void HandleEndScreen()
        {
            // endScreen과 관련된 UI 요소가 null인지 확인
            if (endScreen == null || endScreen.goldReward == null || endScreen.relicReward == null)
            {
                Debug.LogError("EndScreen 또는 관련 UI 요소가 null입니다.");
                return;
            }

            // gold 관련 UI 활성화
            endScreen.gameObject.SetActive(true);
            endScreen.goldReward.gameObject.SetActive(true);
            endScreen.cardRewardButton.gameObject.SetActive(true);

            // enemies 배열이 null이 아니고, 첫 번째 적이 있는지 확인
            if (enemies != null && enemies.Count > 0 && enemies[0] != null)
            {
                // 골드 정보 업데이트
                endScreen.goldReward.relicName.text = enemies[0].goldDrop.ToString() + " Gold";

                // 유물 처리 (nob 확인)
                if (enemies[0].nob && gameManager != null && gameManager.relicLibrary != null && gameManager.relicLibrary.Count > 0)
                {
                    gameManager.relicLibrary.Shuffle();
                    endScreen.relicReward.gameObject.SetActive(true);
                    endScreen.relicReward.DisplayRelic(gameManager.relicLibrary[0]);

                    // 유물을 playerStatsUI에 추가하고 표시
                    gameManager.relics.Add(gameManager.relicLibrary[0]);
                    gameManager.relicLibrary.Remove(gameManager.relicLibrary[0]);

                    // playerStatsUI가 null이 아닌지 확인 후 호출
                    if (playerStatsUI != null)
                    {
                        playerStatsUI.DisplayRelics();
                    }
                    else
                    {
                        Debug.LogError("PlayerStatsUI가 null입니다.");
                    }
                }
                else
                {
                    // nob이 없을 경우 relicReward 비활성화
                    endScreen.relicReward.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogError("적 정보(enemies)가 null이거나 적 목록이 비어 있습니다.");
            }
        }
    }
}