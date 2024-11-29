using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class Shop : MonoBehaviour
    {
        [Header("UI Elements")]
        public Transform itemContainer;
        public Transform relicContainer;
        public GameObject itemPrefab;
        public GameObject relicPrefab;
        public Button refreshButton;
        public Text refreshButtonText;

        [Header("Card Management")]
        public Button removeCardButton;
        public Button upgradeCardButton;
        public Text removeCardButtonText;
        public Text upgradeCardButtonText;
        public int removeCardCost = 75;

        [Header("Items")]
        public List<Item> itemsForSale = new List<Item>();
        public List<Item> relicsForSale = new List<Item>();
        public int itemsToDisplay = 5;
        public int relicsToDisplay = 3;

        public GameManager gameManager;
        public CardManagementUI cardManagementUI;
        PlayerStatsUI playerStatsUI;

        private int baseRefreshPrice = 50;
        private int refreshPrice;
        private int freeRefreshesRemaining = 3;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardManagementUI = FindObjectOfType<CardManagementUI>();
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager를 찾을 수 없습니다.");
                return;
            }

            if (cardManagementUI == null)
            {
                Debug.LogError("CardManagementUI를 찾을 수 없습니다.");
                return;
            }

            refreshPrice = baseRefreshPrice;
            PopulateShop();
            PopulateRelics();
            UpdateRefreshButtonText();
            UpdateRemoveCardButtonText();
            UpdateUpgradeCardButtonText();

            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(RefreshShop);
            }

            if (removeCardButton != null)
            {
                removeCardButton.onClick.AddListener(() => cardManagementUI.ShowCardList(true));
            }

            if (upgradeCardButton != null)
            {
                upgradeCardButton.onClick.AddListener(() => cardManagementUI.ShowCardList(false));
            }
        }

        private void Update()
        {
            UpdateRefreshButtonText();
            UpdateRemoveCardButtonText();
            UpdateUpgradeCardButtonText();
        }

        // 상점에 아이템 목록을 표시
        public void PopulateShop()
        {
            if (itemPrefab == null || itemContainer == null)
            {
                return;
            }

            foreach (Transform child in itemContainer)
            {
                Destroy(child.gameObject);
            }

            List<Item> shuffledItems = new List<Item>(itemsForSale);
            for (int i = 0; i < shuffledItems.Count; i++)
            {
                Item temp = shuffledItems[i];
                int randomIndex = Random.Range(i, shuffledItems.Count);
                shuffledItems[i] = shuffledItems[randomIndex];
                shuffledItems[randomIndex] = temp;
            }

            int itemsDisplayed = Mathf.Min(itemsToDisplay, shuffledItems.Count);
            for (int i = 0; i < itemsDisplayed; i++)
            {
                Item item = shuffledItems[i];
                GameObject itemGO = Instantiate(itemPrefab, itemContainer);
                CardItemUI itemUI = itemGO.GetComponent<CardItemUI>();

                if (itemUI != null && !item.isRelic)
                {
                    itemUI.SetItem(item, this); // 아이템이 카드일 경우 설정
                    itemUI.buyButton.interactable = true; // 구매 버튼 활성화
                }
                else
                {
                    Debug.LogWarning("ItemUI가 null이거나 아이템이 카드가 아닙니다.");
                }
            }
        }

        // 상점에 렐릭 목록을 표시
        public void PopulateRelics()
        {
            if (relicPrefab == null || relicContainer == null)
            {
                return;
            }

            foreach (Transform child in relicContainer)
            {
                Destroy(child.gameObject);
            }

            List<Item> shuffledRelics = new List<Item>(relicsForSale);
            for (int i = 0; i < shuffledRelics.Count; i++)
            {
                Item temp = shuffledRelics[i];
                int randomIndex = Random.Range(i, shuffledRelics.Count);
                shuffledRelics[i] = shuffledRelics[randomIndex];
                shuffledRelics[randomIndex] = temp;
            }

            int relicsDisplayed = Mathf.Min(relicsToDisplay, shuffledRelics.Count);
            for (int i = 0; i < relicsDisplayed; i++)
            {
                Item relic = shuffledRelics[i];
                GameObject relicGO = Instantiate(relicPrefab, relicContainer);
                ItemUI relicUI = relicGO.GetComponent<ItemUI>();

                if (relicUI != null)
                {
                    relicUI.SetItem(relic, this);
                    relicUI.buyButton.interactable = true; // 구매 버튼 활성화
                }
            }
        }

        // 상점 새로고침 기능
        private void RefreshShop()
        {
            int currentRefreshPrice = refreshPrice;

            // "블랙 카드" 렐릭 효과: 3회 무료 새로고침
            if (gameManager.PlayerHasRelic("블랙 카드") && freeRefreshesRemaining > 0)
            {
                currentRefreshPrice = 0;
                freeRefreshesRemaining--;
            }

            if (gameManager.goldAmount >= currentRefreshPrice)
            {
                gameManager.goldAmount -= currentRefreshPrice;
                if (currentRefreshPrice > 0) // 비용이 발생할 경우에만 증가
                {
                    refreshPrice *= 2;
                }
                PopulateShop();
                PopulateRelics();
                UpdateRefreshButtonText();
                gameManager.UpdateGoldNumber(0);
            }
        }

        private void UpdateRefreshButtonText()
        {
            if (gameManager.PlayerHasRelic("블랙 카드") && freeRefreshesRemaining > 0)
            {
                refreshButtonText.text = $"새로고침 : 무료 ({freeRefreshesRemaining}회)";
            }
            else
            {
                refreshButtonText.text = $"새로고침 : {refreshPrice} 골드";
            }
        }

        private void UpdateRemoveCardButtonText()
        {
            int currentRemoveCardCost = gameManager.PlayerHasRelic("마스터카드") ? 10 : removeCardCost;
            if (removeCardButtonText != null)
            {
                removeCardButtonText.text = $"카드 제거 : {currentRemoveCardCost} 골드";
            }
        }

        public void RemoveCard(Card card, bool isInPlayerDeck)
        {
            int cardRemoveCost = removeCardCost;
            UpdateRemoveCardButtonText();

            if (gameManager.PlayerHasRelic("마스터카드"))
            {
                cardRemoveCost = 10;
            }

            if (gameManager != null && gameManager.goldAmount >= cardRemoveCost)
            {
                gameManager.goldAmount -= cardRemoveCost;

                if (isInPlayerDeck && gameManager.playerDeck.Contains(card))
                {
                    gameManager.playerDeck.Remove(card);
                    Debug.Log($"{card.cardTitle} has been removed from the playerDeck.");
                }
                else if (!isInPlayerDeck && gameManager.playerBattleDeck.Contains(card))
                {
                    gameManager.playerBattleDeck.Remove(card);
                    Debug.Log($"{card.cardTitle} has been removed from the playerBattleDeck.");
                }

                gameManager.UpdateGoldNumber(0);
                cardManagementUI.HideCardList();
            }
            else
            {
                Debug.Log("골드가 부족하거나 GameManager가 없습니다.");
            }
        }

        private void UpdateUpgradeCardButtonText()
        {
            int upgradeCost = 100;

            if (gameManager.PlayerHasRelic("넷플릭스 구독권"))
            {
                upgradeCost = Mathf.CeilToInt(upgradeCost * 0.75f);
            }

            if (upgradeCardButtonText != null)
            {
                upgradeCardButtonText.text = $"카드 강화 : {upgradeCost} 골드";
            }
        }

        

        public void BuyItem(Item item, Button buyButton)
        {
            if (!item.isRelic && gameManager.playerDeck.Count >= 30)
            {
                Debug.LogWarning("Cannot buy item. The playerDeck already has 30 cards.");
                return;
            }

            int itemPrice = item.isRelic ? item.price : item.card.cardPrice.cardShopPrice;

            if (!item.isRelic && gameManager.PlayerHasRelic("Chat-GPT 구독권"))
            {
                itemPrice = Mathf.CeilToInt(itemPrice * 0.75f);
            }

            if (gameManager.goldAmount >= itemPrice)
            {
                gameManager.goldAmount -= itemPrice;

                if (item.isRelic)
                {
                    gameManager.AddRelic(item.relic);
                    playerStatsUI.DisplayRelics();
                    relicsForSale.Remove(item);
                    if(gameManager.PlayerHasRelic("딸기")&&gameManager.strawberryUse==true)
                    {
                        gameManager.playerMaxHealth+=10;
                        gameManager.strawberryUse=false;
                    }

                    if(gameManager.PlayerHasRelic("사과")&&gameManager.appleUse==true)
                    {
                        gameManager.playerMaxHealth+=7;
                        gameManager.appleUse=false;
                    }

                    if(gameManager.PlayerHasRelic("샤인머스캣")&&gameManager.shinemuscatUse==true)
                    {
                        gameManager.playerMaxHealth+=15;
                        gameManager.shinemuscatUse=false;
                    }

                    if(gameManager.PlayerHasRelic("생명의 비약")&&gameManager.elixirUse==true)
                    {
                        gameManager.playerMaxHealth+=25;
                        gameManager.elixirUse=false;
                    }
                }
                else
                {
                    gameManager.playerDeck.Add(item.card);
                }

                gameManager.UpdateGoldNumber(0);
                gameManager.TransformRelics();

                buyButton.interactable = false;
                buyButton.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }

        

        public void UpgradeCard(Card card)
        {
            int cardUpgradeCost = card.cardPrice.cardEnhancePrice;

            // "넷플릭스 구독권" 렐릭이 있는지 확인하여 카드 강화 비용 25% 할인
            if (gameManager.PlayerHasRelic("넷플릭스 구독권"))
            {
                cardUpgradeCost = Mathf.CeilToInt(cardUpgradeCost * 0.75f); // 비용을 25% 감소
            }

            if (gameManager != null && gameManager.goldAmount >= cardUpgradeCost)
            {
                // 카드가 이미 강화된 상태인지 확인
                if (!card.isUpgraded)
                {
                    gameManager.goldAmount -= cardUpgradeCost;

                    // 카드 복제 후 업그레이드
                    Card upgradedCard = Instantiate(card);
                    upgradedCard.isUpgraded = true;
                    Debug.Log($"{upgradedCard.cardTitle} has been upgraded.");

                    // playerDeck에서 해당 카드만 교체
                    for (int i = 0; i < gameManager.playerDeck.Count; i++)
                    {
                        if (gameManager.playerDeck[i] == card)
                        {
                            gameManager.playerDeck[i] = upgradedCard;
                            Debug.Log($"{card.cardTitle} has been upgraded in the playerDeck.");
                            break; // 첫 번째로 일치하는 카드만 교체
                        }
                    }

                    // playerBattleDeck에서 해당 카드만 교체
                    for (int i = 0; i < gameManager.playerBattleDeck.Count; i++)
                    {
                        if (gameManager.playerBattleDeck[i] == card)
                        {
                            gameManager.playerBattleDeck[i] = upgradedCard;
                            Debug.Log($"{card.cardTitle} has been upgraded in the playerBattleDeck.");
                            break; // 첫 번째로 일치하는 카드만 교체
                        }
                    }

                    gameManager.UpdateGoldNumber(0);
                    cardManagementUI.HideCardList();
                }
                else
                {
                    Debug.Log($"{card.cardTitle} 카드는 이미 강화된 상태입니다.");
                }
            }
            else
            {
                Debug.Log("골드가 부족합니다.");
            }
        }


        public void ResetRefreshPrice()
        {
            refreshPrice = baseRefreshPrice; // 초기 새로고침 가격으로 되돌림
            UpdateRefreshButtonText(); // 새로고침 버튼의 텍스트 업데이트
        }
    }
}