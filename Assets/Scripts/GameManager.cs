using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                playerStatsUI = FindObjectOfType<PlayerStatsUI>();
                InitializeDeck();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeDeck()
        {
            // ���� �ʱ�ȭ�� ��, �� ī�带 �����ؼ� ���������� ����
            List<Card> clonedDeck = new List<Card>();
            foreach (Card card in playerDeck)
            {
                Card clonedCard = Instantiate(card); // Card�� �����Ͽ� ���ο� �ν��Ͻ� ����
                clonedCard.isUpgraded = false; // �ʱ�ȭ ���·� ����
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
