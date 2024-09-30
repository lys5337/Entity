using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TJ
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Character character;
        public List<Card> playerDeck = new List<Card>();
        public List<Card> cardLibrary = new List<Card>();
        public List<Relic> relics = new List<Relic>();
        public List<Relic> relicLibrary = new List<Relic>();
        public int floorNumber = 1;
        public int goldAmount;
        public PlayerStatsUI playerStatsUI;
        public List<Card> expiredCards = new List<Card>();

        public int maxTendencyValue = 300; // 성향치 최대 값
        public int currentTendencyValue = 150; // 현재 성향치 값
        
        public TMP_InputField nameInputField; // TextMeshPro 입력 필드 (플레이어 이름 입력)
        public string playerName; // 저장할 플레이어 이름

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // 유물 리스트가 null이면 초기화
                if (relics == null)
                {
                    relics = new List<Relic>();
                }

                playerStatsUI = FindObjectOfType<PlayerStatsUI>();
                InitializeDeck();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SubmitName()
        {
            if (nameInputField != null)
            {
                playerName = nameInputField.text; // 입력된 이름을 playerName에 저장
                Debug.Log($"플레이어 이름이 {playerName}로 설정되었습니다.");
            }
            else
            {
                Debug.LogError("이름 입력 필드가 연결되지 않았습니다.");
            }
        }


        public void InitializeDeck()
        {
            // 덱을 초기화할 때, 각 카드를 복제해서 개별적으로 관리
            List<Card> clonedDeck = new List<Card>();
            foreach (Card card in playerDeck)
            {
                Card clonedCard = Instantiate(card); // Card를 복제하여 새로운 인스턴스 생성
                clonedCard.isUpgraded = false; // 초기화 상태로 설정
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

        public void AddRelic(Relic newRelic)
        {
            if (newRelic != null && !PlayerHasRelic(newRelic.relicName))
            {
                relics.Add(newRelic);
                relicLibrary.Remove(newRelic);
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
            expiredCards.Clear(); // 소멸 리스트 초기화
        }

        public void UpdateGoldNumber(int newGold)
        {
            goldAmount += newGold;
            playerStatsUI.moneyAmountText.text = goldAmount.ToString();
        }

        public void DisplayHealth(int healthAmount, int maxHealth)
        {
            playerStatsUI.healthDisplayText.text = $"{healthAmount} / {maxHealth}";
        }
    }
}