using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // 이 줄을 추가하세요

namespace TJ
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Character character;
        public SceneChanger sceneChanger;
        public Fighter fighter;

        // 레어도에 따른 카드 라이브러리 분리
        public List<Card> commonCardLibrary = new List<Card>();
        public List<Card> uncommonCardLibrary = new List<Card>();
        public List<Card> rareCardLibrary = new List<Card>();
        public List<Card> epicCardLibrary = new List<Card>();
        public List<Card> legendCardLibrary = new List<Card>();
        public List<Card> hiddenCardLibrary = new List<Card>();

        [Header("Relic Libraries")]
        public List<Relic> allRelics = new List<Relic>();
        public RelicStage nowStageRelics = new RelicStage();
        [Header("Stage Relic Libraries")]
        public RelicStage stage0 = new RelicStage();
        public RelicStage stage1 = new RelicStage();
        public RelicStage stage2 = new RelicStage();
        public RelicStage stage2B = new RelicStage();
        public RelicStage stage3 = new RelicStage();

        [System.Serializable]
        public class RelicStage
        {
            public List<Relic> normalRelics = new List<Relic>();
            public List<Relic> chestRelics = new List<Relic>();
            public List<Relic> eliteRelics = new List<Relic>();
            public List<Relic> bossRelics = new List<Relic>();
            public List<Relic> eventRelics = new List<Relic>();
            public List<Relic> specifiedEventRelics = new List<Relic>();
        }

        [HideInInspector]public int floorNumber = 1;
        public int stageNumber;

        public PlayerStatsUI playerStatsUI;
        public PlayerStatusBox playerStatusBox;
        public List<Card> expiredCards = new List<Card>();

        [HideInInspector] public int maxTendencyValue = 100;
        [HideInInspector] public TMP_InputField nameInputField;

        [Header("Player Status & Data")]
        public string playerName;
        public string partner= "";
        public int playerMaxHealth;
        public int playerCurrentHealth;
        public int goldAmount;
        public int playerPower;
        public int playerAmor;
        public int playerLuck;
        public int currentTendencyValue;
        TendencyBar tendencyBar;
        public List<Card> playerDeck = new List<Card>();
        public List<Card> playerBattleDeck = new List<Card>();
        public List<Relic> relics = new List<Relic>();

        [HideInInspector] public bool bronzeCoin = true;
        [HideInInspector] public bool silverCoin = true;
        [HideInInspector] public bool goldCoin = true;
        [HideInInspector] public bool appleUse = true;
        [HideInInspector] public bool strawberryUse = true;
        [HideInInspector] public bool shinemuscatUse = true;
        [HideInInspector] public bool elixirUse = true;
        [HideInInspector] public bool bearBile = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePlayerStats();

                if (relics == null)
                {
                    relics = new List<Relic>();
                }

                playerStatsUI = FindObjectOfType<PlayerStatsUI>();
                playerStatusBox = FindObjectOfType<PlayerStatusBox>();
                fighter = FindObjectOfType<Fighter>();

                SceneManager.sceneLoaded += OnSceneLoaded;

                InitializeDeck();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            NowStageRelicSet();
        }
        
        private void Update()
        {
            UpdatePlayerStatsUI();

            if (sceneChanger != null)
            {
                sceneChanger.CheckGameOver();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 로드 후 필요한 오브젝트를 다시 참조
            sceneChanger = FindObjectOfType<SceneChanger>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
            playerStatusBox = FindObjectOfType<PlayerStatusBox>();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void NowStageRelicSet()
        {
            nowStageRelics = new RelicStage();

            switch (stageNumber)
            {
                case 0:
                    AddRelicsToNowStage(stage0);
                    break;
                case 1:
                    AddRelicsToNowStage(stage1);
                    break;
                case 2:
                    AddRelicsToNowStage(stage2);
                    break;
                case 3:
                    AddRelicsToNowStage(stage2B);
                    break;
                case 4:
                    AddRelicsToNowStage(stage3);
                    break;
                default:
                    Debug.LogWarning("유효하지 않은 스테이지 번호입니다.");
                    break;
            }
        }

        private void AddRelicsToNowStage(RelicStage stage)
        {
            nowStageRelics.normalRelics.AddRange(stage.normalRelics);
            nowStageRelics.bossRelics.AddRange(stage.bossRelics);
            nowStageRelics.chestRelics.AddRange(stage.chestRelics);
            nowStageRelics.eliteRelics.AddRange(stage.eliteRelics);
            nowStageRelics.eventRelics.AddRange(stage.eventRelics);
            nowStageRelics.specifiedEventRelics.AddRange(stage.specifiedEventRelics);
        }

        public void DrainEnemyStats(int a, int b, int c, int d)
        {
            playerPower += a;
            playerAmor += b;
            playerLuck += c;
            playerMaxHealth += d;
        }

        public void IncreaseTendencyValue(int amount)
        {
            currentTendencyValue += amount;
            if (currentTendencyValue > maxTendencyValue)
                currentTendencyValue = maxTendencyValue;

            if (tendencyBar != null)
            {
                tendencyBar.UpdateTendencyBar();
            }

            Debug.Log($"GameManager: 플레이어 성향이 {amount}만큼 증가했습니다. 현재 성향: {currentTendencyValue}/{maxTendencyValue}");
        }

        public void IncreasePlayerHealth(int amount)
        {
            playerCurrentHealth += amount;
            if (playerCurrentHealth > playerMaxHealth)
                playerCurrentHealth = playerMaxHealth;

            DisplayHealth(playerCurrentHealth, playerMaxHealth);
            Debug.Log($"GameManager: 플레이어 체력이 {amount}만큼 증가했습니다. 현재 체력: {playerCurrentHealth}/{playerMaxHealth}");
        }

        public void DecreasePlayerHealth(int amount)
        {
            playerCurrentHealth -= amount;
            if (playerCurrentHealth < 0)
                playerCurrentHealth = 0;

            DisplayHealth(playerCurrentHealth, playerMaxHealth);
            Debug.Log($"GameManager: 플레이어 체력이 {amount}만큼 감소했습니다. 현재 체력: {playerCurrentHealth}/{playerMaxHealth}");
        }

        public void IncreasePlayerMaxHealth(int amount)
        {
            playerMaxHealth += amount;
            playerCurrentHealth = Mathf.Min(playerCurrentHealth, playerMaxHealth);
            DisplayHealth(playerCurrentHealth, playerMaxHealth);
            Debug.Log($"GameManager: 플레이어 최대 체력이 {amount}만큼 증가했습니다. 현재 최대 체력: {playerMaxHealth}");
        }

        public void DecreasePlayerMaxHealth(int amount)
        {
            playerMaxHealth -= amount;
            if (playerMaxHealth < 1)
                playerMaxHealth = 1;

            playerCurrentHealth = Mathf.Min(playerCurrentHealth, playerMaxHealth);
            DisplayHealth(playerCurrentHealth, playerMaxHealth);
            Debug.Log($"GameManager: 플레이어 최대 체력이 {amount}만큼 감소했습니다. 현재 최대 체력: {playerMaxHealth}");
        }

        public void SubmitName()
        {
            if (nameInputField != null)
            {
                playerName = nameInputField.text;
                Debug.Log($"플레이어 이름이 {playerName}로 설정되었습니다.");
            }
            else
            {
                Debug.LogError("이름 입력 필드가 연결되지 않았습니다.");
            }
        }

        private void InitializePlayerStats()
        {
            playerMaxHealth = 50;
            playerCurrentHealth = playerMaxHealth;
            goldAmount = 0;

            playerPower = 2;
            playerAmor = 2;
            playerLuck = 2;
            currentTendencyValue = 10;

            DisplayHealth(playerCurrentHealth, playerMaxHealth);
        }

        public void DisplayPlayerStats()
        {
            if (playerStatusBox != null)
            {
                playerStatusBox.UpdatePlayerStats(playerPower, playerAmor, playerLuck);
            }
        }

        public void IncreasePlayerPower(int amount)
        {
            playerPower += amount;
            Debug.Log($"플레이어 힘이 {amount}만큼 증가했습니다. 현재 힘: {playerPower}");
        }

        public void IncreasePlayerAmor(int amount)
        {
            playerAmor += amount;
            Debug.Log($"플레이어 방어가 {amount}만큼 증가했습니다. 현재 방어: {playerAmor}");
        }

        public void IncreasePlayerLuck(int amount)
        {
            playerLuck += amount;
            Debug.Log($"플레이어 운이 {amount}만큼 증가했습니다. 현재 운: {playerLuck}");
        }

        public void InitializeDeck()
        {
            List<Card> clonedDeck = new List<Card>();
            foreach (Card card in playerDeck)
            {
                Card clonedCard = Instantiate(card);
                clonedCard.isUpgraded = false;
                clonedDeck.Add(clonedCard);
            }
            playerDeck = clonedDeck;
        }

        public void LoadCharacterStats()
        {
            AddRelic(character.startingRelic);
            playerStatsUI.playerStatsUIObject.SetActive(true);
            playerStatsUI.DisplayRelics();
        }

        public void AddCardToLibrary(Card card, CardRarity rarity)
        {
            switch (rarity)
            {
                case CardRarity.Common:
                    commonCardLibrary.Add(card);
                    break;
                case CardRarity.Rare:
                    rareCardLibrary.Add(card);
                    break;
                case CardRarity.Epic:
                    epicCardLibrary.Add(card);
                    break;
                default:
                    Debug.LogError("Invalid card rarity specified.");
                    break;
            }
        }

        public void AddRelic(Relic newRelic, string relicType = "")
        {
            if (newRelic == null || PlayerHasRelic(newRelic.relicName))
            {
                Debug.Log($"Relic {newRelic?.relicName} is already owned or null.");
                return;
            }

            relics.Add(newRelic);

            switch (relicType)
            {
                case "normal":
                    nowStageRelics.normalRelics.Remove(newRelic);
                    break;
                case "boss":
                    nowStageRelics.bossRelics.Remove(newRelic);
                    break;
                case "chest":
                    nowStageRelics.chestRelics.Remove(newRelic);
                    break;
                case "elite":
                    nowStageRelics.eliteRelics.Remove(newRelic);
                    break;
                case "event":
                    nowStageRelics.eventRelics.Remove(newRelic);
                    break;
                case "specifiedEvent":
                    nowStageRelics.specifiedEventRelics.Remove(newRelic);
                    break;
                default:
                    break;
            }
        }

        public void TransformRelics()
        {
            string relic20 = "달인의 반지";
            string relic21 = "십자 조준선";
            string transformedRelic10 = "용 조각상";

            if (PlayerHasRelic(relic20) && PlayerHasRelic(relic21))
            {
                relics.RemoveAll(r => r.relicName == relic20 || r.relicName == relic21);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic10);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic10}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic10} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }


            string relic1 = "평범한 동전";
            string relic2 = "화려한 동전";
            string relic3 = "고대의 동전";
            string transformedRelic = "희귀하고 화려한 고대의 주화";

            if (PlayerHasRelic(relic1) && PlayerHasRelic(relic2) && PlayerHasRelic(relic3))
            {
                relics.RemoveAll(r => r.relicName == relic1 || r.relicName == relic2 || r.relicName == relic3);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }

            string relic4 = "금화";
            string relic5 = "은화";
            string relic6 = "동화";
            string transformedRelic2 = "보물상자";

            if (PlayerHasRelic(relic4) && PlayerHasRelic(relic5) && PlayerHasRelic(relic6))
            {
                relics.RemoveAll(r => r.relicName == relic4 || r.relicName == relic5 || r.relicName == relic6);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic2);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic2}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic2} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();

                UpdateGoldNumber(5000);
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }

            string relic7 = "하얀 보석이 박힌 단검";
            string relic8 = "푸른 보석이 박힌 반지";
            string relic9 = "붉은 보석이 박힌 귀걸이";
            string transformedRelic3 = "3가지 보석이 박힌 왕관";

            if (PlayerHasRelic(relic7) && PlayerHasRelic(relic8) && PlayerHasRelic(relic9))
            {
                relics.RemoveAll(r => r.relicName == relic7 || r.relicName == relic8 || r.relicName == relic9);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic3);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic3}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic3} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }

            string relic10 = "특이한 광석";
            string relic11 = "세공용 망치";
            string relic12 = "세공용 연마제";
            string transformedRelic4 = "빛이 새어나오는 보석";

            if (PlayerHasRelic(relic10) && PlayerHasRelic(relic11) && PlayerHasRelic(relic12))
            {
                relics.RemoveAll(r => r.relicName == relic10 || r.relicName == relic11 || r.relicName == relic12);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic4);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic4}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic4} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }

            string relic13 = "빛이 새어나오는 보석";
            string relic14 = "3가지 보석이 박힌 왕관";
            string relic15 = "준비된 검";
            string transformedRelic5 = "빛나는 보석이 박힌 검";

            if (PlayerHasRelic(relic13) && PlayerHasRelic(relic14) && PlayerHasRelic(relic15))
            {
                relics.RemoveAll(r => r.relicName == relic13 || r.relicName == relic14 || r.relicName == relic15);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic5);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic5}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic5} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }

            string relic16 = "독화살";
            string relic17 = "별의 조각";
            string transformedRelic6 = "별의 화살";

            if (PlayerHasRelic(relic16) && PlayerHasRelic(relic17))
            {
                relics.RemoveAll(r => r.relicName == relic16 || r.relicName == relic17);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic6);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic6}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic6} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }


            string relic18 = "별의 조각";
            string relic19 = "수호의 부적";
            string transformedRelic7 = "별의 부적";

            if (PlayerHasRelic(relic18) && PlayerHasRelic(relic19))
            {
                relics.RemoveAll(r => r.relicName == relic18 || r.relicName == relic19);

                Relic newRelic = allRelics.Find(r => r.relicName == transformedRelic7);
                if (newRelic != null)
                {
                    relics.Add(newRelic);
                    Debug.Log($"Relics transformed into {transformedRelic7}");
                }
                else
                {
                    Debug.LogWarning($"{transformedRelic7} relic not found in allRelics list.");
                }
                Debug.Log("Relic transformation successful.");

                playerStatsUI.DisplayRelics();
            }
            else
            {
                Debug.Log("Relic transformation failed.");
            }
        }

        public void AddNormalRelic()
        {
            if (nowStageRelics.normalRelics.Count > 0)
            {
                AddRelic(nowStageRelics.normalRelics[0], "normal"); // 첫 번째 normalRelic을 추가
            }
        }

        // Helper 메서드: 특정 종류의 relic을 추가
        public void AddBossRelic()
        {
            if (nowStageRelics.bossRelics.Count > 0)
            {
                AddRelic(nowStageRelics.bossRelics[0], "boss"); // 첫 번째 bossRelic을 추가
            }
        }

        public void AddChestRelic()
        {
            if (nowStageRelics.chestRelics.Count > 0)
            {
                AddRelic(nowStageRelics.chestRelics[0], "chest"); // 첫 번째 chestRelic을 추가
            }
        }

        public void AddEliteRelic()
        {
            if (nowStageRelics.eliteRelics.Count > 0)
            {
                AddRelic(nowStageRelics.eliteRelics[0], "elite"); // 첫 번째 eliteRelic을 추가
            }
        }

        public void AddEventRelic()
        {
            if (nowStageRelics.eventRelics.Count > 0)
            {
                AddRelic(nowStageRelics.eventRelics[0], "event"); // 첫 번째 eventRelic을 추가
            }
        }

        public void AddSpecifiedEventRelic()
        {
            if (nowStageRelics.specifiedEventRelics.Count > 0)
            {
                AddRelic(nowStageRelics.specifiedEventRelics[0], "specifiedEvent"); // 첫 번째 specifiedEventRelic을 추가
            }
        }


        public bool PlayerHasRelic(string rName)
        {
            foreach (Relic r in relics)
            {
                if (r.relicName == rName)
                    return true;
            }
            return false;
        }

        public void UpdateFloorNumber()
        {
            floorNumber += 1;

            switch (floorNumber)
            {
                case 1:
                    playerStatsUI.floorText.text = floorNumber + "st Floor";
                    break;
                case 2:
                    playerStatsUI.floorText.text = floorNumber + "nd Floor";
                    break;
                case 3:
                    playerStatsUI.floorText.text = floorNumber + "rd Floor";
                    break;
                default:
                    playerStatsUI.floorText.text = floorNumber + "th Floor";
                    break;
            }
        }

        public void RestoreExpiredCards()
        {
            expiredCards.Clear();
        }

        public void AddGold(int StealGold)
        {
            goldAmount += StealGold;
        }

        public void UpdateGoldNumber(int newGold)
        {
            goldAmount += newGold;
            playerStatsUI.moneyAmountText.text = goldAmount.ToString();
            playerStatusBox.moneyAmountText.text = goldAmount.ToString();
        }

        public void UpdatePlayerStatsUI()
        {
            if (playerStatsUI == null)
            {
                Debug.LogWarning("PlayerStatsUI가 null입니다. 씬에 해당 오브젝트가 있는지 확인하세요.");
                return;
            }
            if (playerStatusBox == null)
            {
                Debug.LogWarning("PlayerStatusBox가 null입니다. 씬에 해당 오브젝트가 있는지 확인하세요.");
                return;
            }

            DisplayHealth(playerCurrentHealth, playerMaxHealth);
            playerStatsUI.DisplayPlayerName();
            UpdateGoldNumber(0);
            if (tendencyBar != null)
            {
                tendencyBar.UpdateTendencyBar();
            }
            playerStatsUI.DisplayRelics();
        }

        public void DisplayHealth(int healthAmount, int maxHealth)
        {
            if (playerStatsUI != null)
            {
                playerStatsUI.healthDisplayText.text = $"{healthAmount} / {maxHealth}";
            }
            if (playerStatusBox != null)
            {
                playerStatusBox.healthDisplayText.text = $"{healthAmount} / {maxHealth}";
            }

            playerCurrentHealth = healthAmount;
            playerMaxHealth = maxHealth;
        }
    }

    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legend,
        Hidden
    }
}
