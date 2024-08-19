using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class Shop : MonoBehaviour
    {
        [Header("UI Elements")]
        public Transform itemContainer; // ������ UI�� ���� �θ� ��ü
        public Transform relicContainer; // ���� UI�� ���� �θ� ��ü
        public GameObject itemPrefab; // ������ UI ������
        public GameObject relicPrefab; // ���� UI ������
        public Button refreshButton; // ���ΰ�ħ ��ư
        public Text refreshButtonText; // ���ΰ�ħ ��ư ���� �ؽ�Ʈ

        [Header("Card Management")]
        public Button removeCardButton; // ī�� ���� ��ư
        public Button upgradeCardButton; // ī�� ��ȭ ��ư
        public Text removeCardButtonText; // ī�� ���� ��ư �ؽ�Ʈ
        public Text upgradeCardButtonText; // ī�� ��ȭ ��ư �ؽ�Ʈ
        public int removeCardCost = 75; // ī�� ���� ���
        public int upgradeCardCost = 100; // ī�� ��ȭ ���

        [Header("Items")]
        public List<Item> itemsForSale = new List<Item>(); // �������� �Ǹ��� ������ ���
        public List<Item> relicsForSale = new List<Item>(); // �������� �Ǹ��� ���� ���
        public int itemsToDisplay = 5; // ������ ������ ��
        public int relicsToDisplay = 3; // ������ ���� ��

        public GameManager gameManager;
        public CardManagementUI cardManagementUI; // ī�� ���� UI�� ����

        private int refreshPrice = 50; // �ʱ� ���ΰ�ħ ����

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            cardManagementUI = FindObjectOfType<CardManagementUI>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager�� ã�� �� �����ϴ�.");
                return;
            }

            if (cardManagementUI == null)
            {
                Debug.LogError("CardManagementUI�� ã�� �� �����ϴ�.");
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
                removeCardButtonText.text = $"ī�� ���� : {removeCardCost} ���";
            }

            if (upgradeCardButton != null)
            {
                upgradeCardButton.onClick.AddListener(() => cardManagementUI.ShowCardList(false));
                upgradeCardButtonText.text = $"ī�� ��ȭ : {upgradeCardCost} ���";
            }
        }

        // ������ ������ ����� ǥ��
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
                    itemUI.buyButton.interactable = true; // ���� ��ư Ȱ��ȭ
                }
            }
        }

        // ������ ���� ����� ǥ��
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
                    relicUI.buyButton.interactable = true; // ���� ��ư Ȱ��ȭ
                }
            }
        }

        // ���� ���ΰ�ħ ���
        private void RefreshShop()
        {
            if (gameManager.goldAmount >= refreshPrice)
            {
                gameManager.goldAmount -= refreshPrice;
                refreshPrice *= 2; // ���ΰ�ħ ���� �� ��� ����
                PopulateShop();
                PopulateRelics();
                UpdateRefreshButtonText(); // ���ΰ�ħ ��ư �ؽ�Ʈ ������Ʈ
                gameManager.UpdateGoldNumber(0); // UI ������Ʈ
            }
        }

        private void UpdateRefreshButtonText()
        {
            if (refreshButtonText != null)
            {
                refreshButtonText.text = $"���ΰ�ħ : {refreshPrice} ���";
            }
        }

        public void BuyItem(Item item, Button buyButton)
        {
            if (gameManager != null && gameManager.goldAmount >= item.price)
            {
                gameManager.goldAmount -= item.price;
                if (item.isRelic)
                {
                    gameManager.AddRelic(item.relic); // ���� �߰�
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
                // ī�尡 �̹� ��ȭ�� �������� Ȯ��
                if (!card.isUpgraded)
                {
                    gameManager.goldAmount -= upgradeCardCost;
                    card.isUpgraded = true;
                    gameManager.UpdateGoldNumber(0);
                    cardManagementUI.HideCardList();

                    // ���⼭ �ϳ��� ī�常 ��ȭ�ǵ��� ó��
                    Debug.Log($"{card.cardTitle} ī�尡 ��ȭ�Ǿ����ϴ�.");
                }
                else
                {
                    Debug.Log($"{card.cardTitle} ī��� �̹� ��ȭ�� �����Դϴ�.");
                }
            }
            else
            {
                Debug.Log("��尡 �����մϴ�.");
            }
        }

    }
}
