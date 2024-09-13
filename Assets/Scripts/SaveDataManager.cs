using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;  // 씬 관리를 위해 추가

namespace TJ
{
    public class SaveDataManager : MonoBehaviour
    {
        private string saveFilePath;

        // 슬롯 번호에 따른 경로 설정
        private string GetSaveFilePath(int slot)
        {
            return Application.persistentDataPath + $"/savefile_slot{slot}.json";
        }

        // 슬롯에 맞는 저장 함수
        public void SaveGame(GameManager gameManager, Fighter fighter, int slot)
        {
            SaveData saveData = new SaveData
            {
                playerDeck = new List<CardData>(),
                relics = new List<RelicData>(),
                floorNumber = gameManager.floorNumber,
                goldAmount = gameManager.goldAmount,
                fighterData = new FighterData
                {
                    currentHealth = fighter.currentHealth,
                    maxHealth = fighter.maxHealth
                },
                character = new CharacterData
                {
                    name = gameManager.character.name
                },
                currentSceneName = SceneManager.GetActiveScene().name  // 현재 씬 이름 저장
            };

            // 카드 덱 저장
            foreach (Card card in gameManager.playerDeck)
            {
                saveData.playerDeck.Add(new CardData
                {
                    cardName = card.cardTitle,
                    isUpgraded = card.isUpgraded
                });
            }

            // 유물 저장
            foreach (Relic relic in gameManager.relics)
            {
                saveData.relics.Add(new RelicData
                {
                    relicName = relic.relicName,
                    description = relic.relicDescription
                });
            }

            // JSON으로 변환 후 파일로 저장
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(GetSaveFilePath(slot), json);
            Debug.Log($"게임이 저장되었습니다: 슬롯 {slot} - 경로: " + GetSaveFilePath(slot));
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public List<CardData> playerDeck;
        public List<RelicData> relics;
        public int floorNumber;
        public int goldAmount;
        public FighterData fighterData;
        public CharacterData character;
        public string currentSceneName;  // 씬 이름 저장 추가
    }

    [System.Serializable]
    public class CardData
    {
        public string cardName;
        public bool isUpgraded;
    }

    [System.Serializable]
    public class RelicData
    {
        public string relicName;
        public string description;
    }

    [System.Serializable]
    public class FighterData
    {
        public int currentHealth;
        public int maxHealth;
    }

    [System.Serializable]
    public class CharacterData
    {
        public string name;
    }
}
