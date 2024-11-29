using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace TJ
{
    public class SaveDataManager : MonoBehaviour
    {
        public GameManager gameManager; // GameManager 인스턴스

        // 슬롯 번호에 맞는 파일 경로 반환
        private string GetSceneNameFilePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"savedSceneName_slot{slot}.txt");
        }

        private string GetGameDataFilePath(int slot)
        {
            return Path.Combine(Application.persistentDataPath, $"gameData_slot{slot}.json");
        }

        // 슬롯 번호에 맞게 현재 실행 중인 씬의 이름과 GameManager 데이터를 저장하는 메서드
        public void SaveGame(int slot)
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
            }

            // 현재 씬의 이름과 플레이어 이름을 조합하여 저장
            string currentSceneName = SceneManager.GetActiveScene().name;
            string playerSceneName = $"{gameManager.playerName} - {currentSceneName}";
            File.WriteAllText(GetSceneNameFilePath(slot), playerSceneName);
            Debug.Log($"'{playerSceneName}'이 슬롯 {slot}에 저장되었습니다.");

            // GameManager 데이터 저장
            GameSaveData data = new GameSaveData
            {
                playerName = gameManager.playerName,
                partner = gameManager.partner,
                playerMaxHealth = gameManager.playerMaxHealth,
                playerCurrentHealth = gameManager.playerCurrentHealth,
                playerPower = gameManager.playerPower,
                playerAmor = gameManager.playerAmor,
                playerLuck = gameManager.playerLuck,
                goldAmount = gameManager.goldAmount,
                currentTendencyValue = gameManager.currentTendencyValue,
                deck1 = gameManager.playerDeck.ConvertAll(card => $"{card.cardTitle}:{card.isUpgraded}"),
                deck2 = gameManager.playerBattleDeck.ConvertAll(card => $"{card.cardTitle}:{card.isUpgraded}"),
                relics = gameManager.relics.ConvertAll(relic => relic.relicName),
                nowStageNumber = gameManager.stageNumber,

                bronzeCoin = gameManager.bronzeCoin,
                silverCoin = gameManager.silverCoin,
                goldCoin = gameManager.goldCoin,
                appleUse = gameManager.appleUse,
                strawberryUse = gameManager.strawberryUse,
                shinemuscatUse = gameManager.shinemuscatUse,
                elixirUse = gameManager.elixirUse,
                bearBile = gameManager.bearBile
            };

            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(GetGameDataFilePath(slot), jsonData);
            Debug.Log($"GameManager 데이터가 슬롯 {slot}에 저장되었습니다.");
        }

        // 저장된 씬 이름을 가져오는 메서드
        public string GetSavedSceneName(int slot)
        {
            string filePath = GetSceneNameFilePath(slot);

            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                return "Empty Slot";
            }
        }
    }

    [System.Serializable]
    public class GameSaveData
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

        [HideInInspector] public bool bronzeCoin;
        [HideInInspector] public bool silverCoin;
        [HideInInspector] public bool goldCoin;
        [HideInInspector] public bool appleUse;
        [HideInInspector] public bool strawberryUse;
        [HideInInspector] public bool shinemuscatUse;
        [HideInInspector] public bool elixirUse;
        [HideInInspector] public bool bearBile;
    }
}
