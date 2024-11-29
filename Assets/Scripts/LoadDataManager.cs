using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace TJ
{
    public class LoadDataManager : MonoBehaviour
    {
        public GameManager gameManager; // GameManager 인스턴스

        public void Start()
        {
            gameManager = GameManager.Instance;
        }

        // 슬롯 번호에 맞는 파일 경로 반환
        private string GetSceneNameFilePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"savedSceneName_slot{slot}.txt");
        }

        private string GetGameDataFilePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"gameData_slot{slot}.json");
        }

        // 슬롯 번호에 맞는 씬을 로드하는 메서드
        public void LoadGame(int slot)
        {
            string sceneFilePath = GetSceneNameFilePath(slot);

            if (File.Exists(sceneFilePath))
            {
                string savedPlayerSceneName = File.ReadAllText(sceneFilePath);
                string[] nameParts = savedPlayerSceneName.Split(new string[] { " - " }, System.StringSplitOptions.None);
                if (nameParts.Length > 1)
                {
                    string savedSceneName = nameParts[1];
                    SceneManager.LoadScene(savedSceneName);
                    Debug.Log($"슬롯 {slot}에 저장된 씬 '{savedSceneName}'을 로드합니다.");

                    // 씬 로드 후 데이터 복원 메서드를 호출
                    SceneManager.sceneLoaded += (scene, mode) => RestoreGameData(slot);
                }
                else
                {
                    Debug.LogWarning($"슬롯 {slot}에 저장된 데이터가 올바르지 않습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"슬롯 {slot}에 저장된 씬 이름 파일이 없습니다.");
            }
            gameManager.UpdatePlayerStatsUI();
        }

        public void RestoreGameData(int slot)
        {
            // GameManager가 null인 경우 자동으로 할당
            if (gameManager == null)
            {
                GameObject gameDataObject = GameObject.Find("GameData");
                if (gameDataObject != null)
                {
                    gameManager = gameDataObject.GetComponent<GameManager>();
                }

                if (gameManager == null)
                {
                    Debug.LogError("GameManager를 찾을 수 없습니다.");
                    return;
                }
                gameManager.UpdatePlayerStatsUI();
            }

            string dataFilePath = GetGameDataFilePath(slot);

            if (File.Exists(dataFilePath))
            {
                string jsonData = File.ReadAllText(dataFilePath);
                PlayerGameData data = JsonUtility.FromJson<PlayerGameData>(jsonData);

                if (gameManager != null)
                {
                    // 기본 데이터 복원
                    gameManager.playerName = data.playerName;
                    gameManager.partner = data.partner;
                    gameManager.playerMaxHealth = data.playerMaxHealth;
                    gameManager.playerCurrentHealth = data.playerCurrentHealth;
                    gameManager.playerPower = data.playerPower;
                    gameManager.playerAmor = data.playerAmor;
                    gameManager.playerLuck = data.playerLuck;
                    gameManager.goldAmount = data.goldAmount;
                    gameManager.currentTendencyValue = data.currentTendencyValue;
                    gameManager.stageNumber = data.nowStageNumber;

                    gameManager.bronzeCoin = data.bronzeCoin;
                    gameManager.silverCoin = data.silverCoin;
                    gameManager.goldCoin = data.goldCoin;
                    gameManager.appleUse = data.appleUse;
                    gameManager.strawberryUse = data.strawberryUse;
                    gameManager.shinemuscatUse = data.shinemuscatUse;
                    gameManager.elixirUse = data.elixirUse;
                    gameManager.bearBile = data.bearBile;

                    // 덱 복원
                    gameManager.playerDeck.Clear();
                    foreach (string cardData in data.deck1)
                    {
                        string[] cardParts = cardData.Split(':');
                        if (cardParts.Length == 2)
                        {
                            string cardName = cardParts[0];
                            bool isUpgraded = bool.Parse(cardParts[1]);

                            Card originalCard = FindCardByName(cardName);
                            if (originalCard != null)
                            {
                                // 카드를 복제한 후 업그레이드 상태 설정
                                Card clonedCard = Instantiate(originalCard);
                                clonedCard.isUpgraded = isUpgraded;
                                gameManager.playerDeck.Add(clonedCard);
                            }
                            else
                            {
                                Debug.LogWarning($"'{cardName}' 이름의 카드를 찾을 수 없습니다.");
                            }
                        }
                    }

                    gameManager.playerBattleDeck.Clear();
                    foreach (string cardData in data.deck2)
                    {
                        string[] cardParts = cardData.Split(':');
                        if (cardParts.Length == 2)
                        {
                            string cardName = cardParts[0];
                            bool isUpgraded = bool.Parse(cardParts[1]);

                            Card originalCard = FindCardByName(cardName);
                            if (originalCard != null)
                            {
                                Card clonedCard = Instantiate(originalCard);
                                clonedCard.isUpgraded = isUpgraded;
                                gameManager.playerBattleDeck.Add(clonedCard);
                            }
                            else
                            {
                                Debug.LogWarning($"'{cardName}' 이름의 카드를 찾을 수 없습니다.");
                            }
                        }
                    }

                    gameManager.InitializeDeck();
                    Debug.Log($"슬롯 {slot}의 GameManager 데이터가 복원되었습니다.");

                    gameManager.relics.Clear();
                    foreach (string relicName in data.relics)
                    {
                        Relic relic = FindRelicByName(relicName);
                        if (relic != null)
                        {
                            gameManager.relics.Add(relic);
                            Debug.Log($"렐릭 '{relicName}'이 복원되었습니다.");
                        }
                        else
                        {
                            Debug.LogWarning($"렐릭 '{relicName}'을 찾을 수 없습니다.");
                        }
                    }
                }
                else
                {
                    Debug.LogError("GameManager가 할당되지 않았습니다.");
                }
            }
            else
            {
                Debug.LogWarning($"슬롯 {slot}에 저장된 게임 데이터 파일이 없습니다.");
            }
            gameManager.UpdatePlayerStatsUI();
            gameManager.NowStageRelicSet();
        }

        private Relic FindRelicByName(string relicName)
        {
            foreach (Relic relic in gameManager.allRelics)
            {
                if (relic.relicName == relicName)
                {
                    return relic;
                }
            }
            return null;
        }

        // 카드 이름으로 GameManager의 카드 라이브러리에서 카드 객체를 찾는 메서드
        private Card FindCardByName(string cardName)
        {
            // 모든 카드 라이브러리에서 카드 검색
            foreach (Card card in gameManager.commonCardLibrary)
                if (card.cardTitle == cardName) return card;

            foreach (Card card in gameManager.uncommonCardLibrary)
                if (card.cardTitle == cardName) return card;

            foreach (Card card in gameManager.rareCardLibrary)
                if (card.cardTitle == cardName) return card;

            foreach (Card card in gameManager.epicCardLibrary)
                if (card.cardTitle == cardName) return card;

            foreach (Card card in gameManager.legendCardLibrary)
                if (card.cardTitle == cardName) return card;
            
            foreach (Card card in gameManager.hiddenCardLibrary)
                if (card.cardTitle == cardName) return card;

            return null; // 해당 이름의 카드가 없으면 null 반환
        }
    }

    // GameManager의 데이터 구조 - PlayerGameData로 클래스 이름 변경
    [System.Serializable]
    public class PlayerGameData
    {
        public string playerName;
        public string partner;
        public int playerMaxHealth;
        public int playerCurrentHealth;
        public int playerPower;
        public int playerAmor;
        public int playerLuck;
        public int goldAmount;
        public int currentTendencyValue;
        public List<string> deck1; // 카드 제목과 업그레이드 여부를 저장
        public List<string> deck2; // 카드 제목과 업그레이드 여부를 저장
        public List<string> relics;
        public int nowStageNumber;

        public bool bronzeCoin;
        public bool silverCoin;
        public bool goldCoin;
        public bool appleUse;
        public bool strawberryUse;
        public bool shinemuscatUse;
        public bool elixirUse;
        public bool bearBile;
        
    }
}
