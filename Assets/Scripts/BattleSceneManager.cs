using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using QTEPack;
using UnityEngine.Events;
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
        public GameObject[] possibleEvents;

        public bool normalFight; // 일반 전투 여부를 추적하는 변수 추가
        public bool eliteFight;
        public bool bossFight; // 보스 전투 여부를 추적하는 변수 추가
        public bool eventFight;
        
        CardActions cardActions;
        GameManager gameManager;
        PlayerStatsUI playerStatsUI;
        public Animator banner;
        public TMP_Text turnText;
        public GameObject gameover;
        public float nextCardDamageMultiplier = 1.0f; //역할을 모르겠음

        [Header("QTEAction")]
        public QTE_SmashButton qteSmashButtonPrefab;
        public QTE_Ring qteRingPrefab;

        private int currentEliteIndex = 0; // 현재 엘리트 인덱스

        private bool isBattleOver = false;
        [HideInInspector]public bool defenseStanceJudge; //광기 버프 온 오프 용

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardActions = GetComponent<CardActions>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
        }

        public void StartNormalFight()
        {
            BeginBattle(possibleEnemies);
            
            normalFight = true;

            BGMManager.Instance.StopBGM();
            BGMManager.Instance.PlaySound("BattleBGM");
        }
        public void StartEliteFight()
        {
            eliteFight = true;

            GameObject currentElite = possibleElites[currentEliteIndex];
            BeginBattle(new GameObject[] { currentElite });

            // 다음 인덱스로 이동 (순환하도록 설정)
            currentEliteIndex = (currentEliteIndex + 1) % possibleElites.Length;
        }



        public void StartBossFight() // 새로운 메서드 추가
        {
            bossFight = true;
            BeginBattle(possibleBosses);
        }

        public void ResetFight()
        {
            normalFight = false;
            eliteFight = false;
            bossFight = false;
        }

        private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
        }

        public void StartQTESmash(Vector2 position, float scale, int difficulty, UnityEngine.Events.UnityAction onSuccess, UnityEngine.Events.UnityAction onFail)
        {
            if (qteSmashButtonPrefab != null)
            {
                QTE_SmashButton qteInstance = Instantiate(qteSmashButtonPrefab, transform);
                
                qteInstance.OnSuccess.RemoveAllListeners();
                qteInstance.OnFail.RemoveAllListeners();

                qteInstance.OnSuccess.AddListener(() =>
                {
                    onSuccess.Invoke();
                    Destroy(qteInstance.gameObject, 0.7f);
                });

                qteInstance.OnFail.AddListener(() =>
                {
                    onFail.Invoke();
                    Destroy(qteInstance.gameObject, 0.7f);
                });

                // QTE 시작
                qteInstance.ShowQTE(position, scale, difficulty);
            }
            else
            {
                Debug.LogError("QTE_SmashButton 프리팹이 설정되지 않았습니다.");
            }
        }

        public void StartQTERing(Vector2 position, float scale, float difficulty, UnityAction onPerfect, UnityAction onSuccess, UnityAction onFail)
        {
            if (qteRingPrefab != null)
            {
                QTE_Ring qteInstance = Instantiate(qteRingPrefab, transform);

                qteInstance.OnPerfect.RemoveAllListeners();
                qteInstance.OnSuccess.RemoveAllListeners();
                qteInstance.OnFail.RemoveAllListeners();

                qteInstance.OnPerfect.AddListener(() =>
                {
                    onPerfect.Invoke(); 
                    Destroy(qteInstance.gameObject, 0.7f); 
                });

                qteInstance.OnSuccess.AddListener(() =>
                {
                    onSuccess.Invoke();
                    Destroy(qteInstance.gameObject, 0.7f);
                });

                qteInstance.OnFail.AddListener(() =>
                {
                    onFail.Invoke();
                    Destroy(qteInstance.gameObject, 0.7f);
                });

                int difficultyIndex = Mathf.RoundToInt(difficulty);

                qteInstance.ShowQTE(position, scale, difficultyIndex);
            }
            else
            {
                Debug.LogError("QTE_Ring 프리팹이 설정되지 않았습니다.");
            }
        }

        private IEnumerator DisableAfterDelay(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            obj.SetActive(false);
        }

        public bool MaxHealthCheck()
        {
            Debug.Log("현재 체력"+player.maxHealth);
            Debug.Log("현재 체력"+player.currentHealth);
            if (player.maxHealth == player.currentHealth)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int PlayerMaxHealthReturn()
        {
            return player.maxHealth;
        }

        public int PlayerCurrnetHealthReturn()
        {
            return player.currentHealth;
        }

        public void BeginBattle(GameObject[] prefabsArray)
        {
            turnText.text = "플레이어의 차례";
            banner.Play("bannerOut");

            GameObject newEnemy = Instantiate(prefabsArray[Random.Range(0, prefabsArray.Length)], enemyParent);
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
            

            discardPile.AddRange(gameManager.playerBattleDeck);
            ShuffleCards();
            DrawCards(drawAmount);
            energy = maxEnergy;
            if (gameManager.PlayerHasRelic("산삼")) energy +=1;
            energyText.text = energy.ToString();

            #region relic checks
            
            if(gameManager.PlayerHasRelic("침이 없는 시계"))
            {
                player.AddBuff(Buff.Type.strength, 3);
                player.AddBuff(Buff.Type.solidity, 3);
                player.AddBuff(Buff.Type.vulnerable, 2);
                player.AddBuff(Buff.Type.weak, 2);
            }

            if(gameManager.PlayerHasRelic("피가 담긴 병")) player.HealEntity(2);

            if(gameManager.PlayerHasRelic("뱀의 허물")) enemyFighters[0].AddBuff(Buff.Type.poison, 5);

            if (gameManager.PlayerHasRelic("닻"))player.AddBlock(10);

            if (gameManager.PlayerHasRelic("랜턴"))energy += 1;

            if (gameManager.PlayerHasRelic("작은 구슬")) enemyFighters[0].AddBuff(Buff.Type.vulnerable, 1);

            if (gameManager.PlayerHasRelic("준비된 가방"))DrawCards(2);

            if (gameManager.PlayerHasRelic("금강저"))player.AddBuff(Buff.Type.strength, 1);

            if (gameManager.PlayerHasRelic("별의 부적"))player.AddBuff(Buff.Type.solidity, 2);

            if (bossFight && gameManager.PlayerHasRelic("타오르는 피")) // 보스 전투에서만 발동되는 유물 처리
            {
                int currentHealth = player.currentHealth;
                int maxHealth = player.maxHealth;
                int healingAmount = 10;

                int expectedHealthAfterHeal = currentHealth + healingAmount;

                if (expectedHealthAfterHeal > maxHealth)
                {
                    healingAmount = maxHealth - currentHealth;
                }
                player.HealEntity(healingAmount);
            }

            #endregion
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
            // 타겟이 없는 공격 카드 처리
            if (cardUI.card.cardType == Card.CardType.Attack && cardTarget == null)
            {
                Debug.LogError("타겟이 설정되지 않았습니다. 공격 카드를 사용할 수 없습니다.");
                cardUI.ResetCardPosition();  // 카드 사용을 취소하고 원래 위치로 복귀
                return;
            }

            // 적의 분노 상태 처리
            if (cardUI.card.cardType != Card.CardType.Attack && enemies[0].GetComponent<Fighter>().enrage.buffValue > 0)
            {
                enemies[0].GetComponent<Fighter>().AddBuff(Buff.Type.strength, enemies[0].GetComponent<Fighter>().enrage.buffValue);
            }

            // 카드 액션 수행
            cardActions.PerformAction(cardUI.card, cardTarget);

            // 에너지 차감 및 UI 업데이트
            energy -= cardUI.card.GetCardCostAmount();
            energyText.text = energy.ToString();

            // 카드 소멸 처리
            if (cardUI.card.expiring)
            {
                gameManager.expiredCards.Add(cardUI.card);  // 소멸 리스트에 추가
            }
            else
            {
                DiscardCard(cardUI.card);  // 일반적인 카드 처리 (버리기)
            }

            // 카드 비활성화 및 효과 처리
            Instantiate(cardUI.discardEffect, cardUI.transform.position, Quaternion.identity, topParent);
            selectedCard = null;
            cardUI.gameObject.SetActive(false);  // 카드 비활성화
            cardsInHand.Remove(cardUI.card);     // 카드 손에서 제거
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

                if(gameManager.PlayerHasRelic("생명의 비약")) player.HealEntity(3);
                

                if(gameManager.PlayerHasRelic("별의 부적"))
                {
                    player.AddBlock(10);
                }
                else if(gameManager.PlayerHasRelic("수호의 부적"))
                {
                    player.AddBlock(5);
                }

                if(gameManager.PlayerHasRelic("별의 화살"))
                {
                    enemyFighters[0].AddBuff(Buff.Type.poison, 3);
                    player.AddBuff(Buff.Type.strength, 1);
                }
                else if(gameManager.PlayerHasRelic("독화살"))
                {
                    enemyFighters[0].AddBuff(Buff.Type.poison, 2);
                }

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

                if(!defenseStanceJudge)
                {
                    player.currentBlock = 0;
                    player.fighterHealthBar.DisplayBlock(0);
                }
                
                energy = maxEnergy;
                if (gameManager.PlayerHasRelic("산삼")) energy +=1;
                energyText.text = energy.ToString();

                endTurnButton.enabled = true;
                DrawCards(drawAmount);

                turnText.text = "플레이어의 차례";
                banner.Play("bannerOut");
            }
        }

        private IEnumerator HandleEnemyTurn()
        {
            turnText.text = "적의 차례";
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
            if (!win && gameover != null)
            {
                gameover.SetActive(true);
            }

            if (gameManager != null && player != null)
            {
                isBattleOver = true;

                if (gameManager.PlayerHasRelic("타오르는 피") && player.currentHealth <= player.maxHealth / 2)
                {
                    player.currentHealth += 6;
                    if (player.currentHealth > player.maxHealth)
                    {
                        player.currentHealth = player.maxHealth;
                    }

                    player.UpdateHealthUI(player.currentHealth);
                }

                if (gameManager.PlayerHasRelic("은 목걸이"))
                {
                    player.currentHealth += 20;
                    if (player.currentHealth > player.maxHealth)
                    {
                        player.currentHealth = player.maxHealth;
                    }

                    player.UpdateHealthUI(player.currentHealth);
                }

                player.ResetBuffs();

                HandleEndScreen();

                gameManager.UpdateFloorNumber();

                if (enemies != null && enemies.Count > 0 && enemies[0] != null)
                {
                    int goldReward = enemies[0].goldDrop;
                    gameManager.UpdateGoldNumber(goldReward);
                }
                else
                {
                    Debug.LogError("적 정보(enemies)가 null이거나 적 목록이 비어 있습니다.");
                }

                // 적 관련 데이터 초기화
                enemies.Clear(); // enemies 리스트 비우기
                enemyFighters.Clear(); // enemyFighters 리스트 비우기
            }
            else
            {
                Debug.LogError("gameManager 또는 player가 null입니다.");
            }

            DisableAllCardsAndButtons();
        }


        public void UpdateCardUI()
        {
            for (int i = 0; i < cardsInHandGameObjects.Count; i++)
            {
                CardUI cardUI = cardsInHandGameObjects[i].GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.LoadCard(cardUI.card);
                }
            }
        }


        private void DisableAllCardsAndButtons()
        {
            foreach (CardUI cardUI in cardsInHandGameObjects)
            {
                if (cardUI != null)
                {
                    cardUI.gameObject.SetActive(false);
                }
            }

            Debug.Log("모든 카드와 버튼이 비활성화되었습니다.");
        }


        public void HandleEndScreen()
        {
            if (endScreen == null || endScreen.goldReward == null || endScreen.relicReward == null)
            {
                Debug.LogError("EndScreen 또는 관련 UI 요소가 null입니다.");
                return;
            }

            endScreen.gameObject.SetActive(true);
            endScreen.goldReward.gameObject.SetActive(true);
            endScreen.cardRewardButton.gameObject.SetActive(true);
            endScreen.goldReward.relicName.text = enemies[0].goldDrop.ToString() + " Gold";

            // checkTravel 계산
            int travelChance = 25 + gameManager.playerLuck / 5;
            bool checkTravel = Random.Range(0, 100) < travelChance;

            // 확률 Debug.Log로 출력
            Debug.Log($"checkTravel 확률: {travelChance}%, 결과: {checkTravel}");

            List<Relic> currentRelicPool = null;

            if (enemies[0].normalCheck && gameManager != null && checkTravel)
            {
                currentRelicPool = gameManager.nowStageRelics.normalRelics;
            }
            else if (enemies[0].eliteCheck && gameManager != null)
            {
                currentRelicPool = gameManager.nowStageRelics.eliteRelics;
            }
            else if (enemies[0].bossCheck && gameManager != null)
            {
                currentRelicPool = gameManager.nowStageRelics.bossRelics;
            }

            if (currentRelicPool != null && currentRelicPool.Count > 0)
            {
                currentRelicPool.Shuffle();
                Relic selectedRelic = currentRelicPool[0];

                endScreen.relicReward.gameObject.SetActive(true);
                endScreen.relicReward.DisplayRelic(selectedRelic);

                gameManager.relics.Add(selectedRelic);
                currentRelicPool.RemoveAt(0);

                if (playerStatsUI != null)
                {
                    playerStatsUI.DisplayRelics();
                }
                else
                {
                    Debug.LogError("PlayerStatsUI가 null입니다.");
                }

                if(gameManager.PlayerHasRelic("웅담") && gameManager.bearBile == true)
                {
                    gameManager.playerMaxHealth += 15;
                    gameManager.bearBile = false;
                }

                gameManager.TransformRelics();
            }
            else
            {
                endScreen.relicReward.gameObject.SetActive(false);
            }
        }




        public void OnEndTurnButtonClick()
        {
            if (isBattleOver)
            {
                Debug.Log("전투가 종료되어 턴을 종료할 수 없습니다.");
                return;
            }

            ChangeTurn();
        }
    }
}