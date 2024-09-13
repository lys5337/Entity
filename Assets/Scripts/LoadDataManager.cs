using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TJ
{
    public class LoadDataManager : MonoBehaviour
    {
        private string GetSaveFilePath(int slot)
        {
            return Application.persistentDataPath + $"/savefile_slot{slot}.json";
        }

        public void LoadGame(GameManager gameManager, Fighter fighter, int slot)
        {
            string saveFilePath = GetSaveFilePath(slot);

            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                gameManager.floorNumber = saveData.floorNumber;
                gameManager.goldAmount = saveData.goldAmount;

                // Fighter의 체력 정보 로드
                fighter.currentHealth = saveData.fighterData.currentHealth;
                fighter.maxHealth = saveData.fighterData.maxHealth;
                fighter.UpdateHealthUI(fighter.currentHealth);

                // 캐릭터 정보 로드
                gameManager.character.name = saveData.character.name;

                // 카드 덱 로드
                gameManager.playerDeck.Clear();
                foreach (CardData cardData in saveData.playerDeck)
                {
                    Card card = new Card { cardTitle = cardData.cardName, isUpgraded = cardData.isUpgraded };
                    gameManager.playerDeck.Add(card);
                }

                // 유물 로드
                gameManager.relics.Clear();
                foreach (RelicData relicData in saveData.relics)
                {
                    Relic relic = new Relic { relicName = relicData.relicName, relicDescription = relicData.description };
                    gameManager.relics.Add(relic);
                }

                if (!string.IsNullOrEmpty(saveData.currentSceneName))
                {
                    SceneManager.LoadScene(saveData.currentSceneName);
                }

                Debug.Log($"게임이 로드되었습니다: 슬롯 {slot} - 경로: " + GetSaveFilePath(slot));
            }
            else
            {
                Debug.LogWarning("세이브 파일이 존재하지 않습니다.");
            }
        }
    }
}
