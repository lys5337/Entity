using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class Shop : MonoBehaviour
    {
        [Header("UI Elements")]
        public Transform itemContainer; // 아이템 UI를 담을 부모 객체
        public Transform relicContainer; // 렐릭 UI를 담을 부모 객체
        public GameObject itemPrefab; // 아이템 UI 프리팹
        public GameObject relicPrefab; // 렐릭 UI 프리팹
        public Button refreshButton; // 새로고침 버튼
        public Text refreshButtonText; // 새로고침 버튼 내의 텍스트

        [Header("Card Management")]
        public Button removeCardButton; // 카드 제거 버튼
        public Button upgradeCardButton; // 카드 강화 버튼
        public Text removeCardButtonText; // 카드 제거 버튼 텍스트
        public Text upgradeCardButtonText; // 카드 강화 버튼 텍스트
        public int removeCardCost = 75; // 카드 제거 비용
        public int upgradeCardCost = 100; // 카드 강화 비용

        [Header("Items")]
        public List<Item> itemsForSale = new List<Item>(); // 상점에서 판매할 아이템 목록
        public List<Item> relicsForSale = new List<Item>(); // 상점에서 판매할 렐릭 목록
        public int itemsToDisplay = 5; // 노출할 아이템 수
        public int relicsToDisplay = 3; // 노출할 렐릭 수

        public GameManager gameManager;
        public CardManagementUI cardManagementUI; // 카드 관리 UI와 연동

        private int refreshPrice = 50; // 초기 새로고침 가격

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardManagementUI = FindObjectOfType<CardManagementUI>();

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

            PopulateShop();
            PopulateRelics();
            UpdateRefreshButtonText();

            if (refreshButton != null)
            {
                refreshButton.onClick.AddListener(RefreshShop);
            }

            if (removeCardButton != null)
            {
                removeCardButton.onClick.AddListener(() => cardManagementUI.ShowCardList(true));
                removeCardButtonText.text = $"카드 제거 : {removeCardCost} 골드";
            }

            if (upgradeCardButton != null)
            {
                upgradeCardButton.onClick.AddListener(() => cardManagementUI.ShowCardList(false));
                upgradeCardButtonText.text = $"카드 강화 : {upgradeCardCost} 골드";
            }
        }

        // 상점에 아이템 목록을 표시
        private void PopulateShop()
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
                ItemUI itemUI = itemGO.GetComponent<ItemUI>();

                if (itemUI != null)
                {
                    itemUI.SetItem(item, this);
                    itemUI.buyButton.interactable = true; // 구매 버튼 활성화
                }
            }
        }

        // 상점에 렐릭 목록을 표시
        private void PopulateRelics()
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
            if (gameManager.goldAmount >= refreshPrice)
            {
                gameManager.goldAmount -= refreshPrice;
                refreshPrice *= 2; // 새로고침 가격 두 배로 증가
                PopulateShop();
                PopulateRelics();
                UpdateRefreshButtonText(); // 새로고침 버튼 텍스트 업데이트
                gameManager.UpdateGoldNumber(0); // UI 업데이트
            }
        }

        private void UpdateRefreshButtonText()
        {
            if (refreshButtonText != null)
            {
                refreshButtonText.text = $"새로고침 : {refreshPrice} 골드";
            }
        }

        public void BuyItem(Item item, Button buyButton)
        {
            if (gameManager != null && gameManager.goldAmount >= item.price)
            {
                gameManager.goldAmount -= item.price;
                if (item.isRelic)
                {
                    gameManager.AddRelic(item.relic); // 렐릭 추가
                    relicsForSale.Remove(item);
                    Destroy(buyButton.gameObject);
                }
                else
                {
                    gameManager.playerDeck.Add(item.card);
                }
                gameManager.UpdateGoldNumber(0);
                buyButton.interactable = false;
            }
        }

        public void RemoveCard(Card card)
        {
            if (gameManager != null && gameManager.goldAmount >= removeCardCost)
            {
                gameManager.goldAmount -= removeCardCost;
                gameManager.playerDeck.Remove(card);
                gameManager.UpdateGoldNumber(0);
                cardManagementUI.HideCardList();
            }
        }

        public void UpgradeCard(Card card)
        {
            if (gameManager != null && gameManager.goldAmount >= upgradeCardCost)
            {
                // 카드가 이미 강화된 상태인지 확인
                if (!card.isUpgraded)
                {
                    gameManager.goldAmount -= upgradeCardCost;
                    card.isUpgraded = true;
                    gameManager.UpdateGoldNumber(0);
                    cardManagementUI.HideCardList();

                    // 여기서 하나의 카드만 강화되도록 처리
                    Debug.Log($"{card.cardTitle} 카드가 강화되었습니다.");
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

    }
}
