using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button Ÿ���� ���� using ���ù� �߰�

namespace TJ
{
    public class CardManagementUI : MonoBehaviour
    {
        public GameObject cardListPanel; // ī�� ����Ʈ�� ���� �г� (Ȱ��ȭ/��Ȱ��ȭ �뵵)
        public Transform cardListContainer; // ī�� ����Ʈ�� ���� �θ� ��ü
        public GameObject cardItemPrefab; // ī�� ����Ʈ ������ ������

        private Shop shop;

        private void Start()
        {
            shop = FindObjectOfType<Shop>();
            cardListPanel.SetActive(false); // �ʱ⿡�� ī�� ����Ʈ �г� ��Ȱ��ȭ
        }

        // ī�� ����Ʈ�� �����ִ� �޼���
        public void ShowCardList(bool isRemoving)
        {
            cardListPanel.SetActive(true);
            PopulateCardList(isRemoving);
        }

        // ī�� ����Ʈ�� ä��� �޼���
        private void PopulateCardList(bool isRemoving)
        {
            // ���� ī�� ����Ʈ ����
            foreach (Transform child in cardListContainer)
            {
                Destroy(child.gameObject);
            }

            // �÷��̾� ������ ī�� ����Ʈ ����
            foreach (Card card in shop.gameManager.playerDeck)
            {
                GameObject cardItemGO = Instantiate(cardItemPrefab, cardListContainer);
                CardUI cardUI = cardItemGO.GetComponent<CardUI>();
                cardUI.LoadCard(card);

                // ī�� Ŭ�� �� ó��
                cardItemGO.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (isRemoving)
                        shop.RemoveCard(card);
                    else
                        shop.UpgradeCard(card);
                });
            }
        }

        public void HideCardList()
        {
            cardListPanel.SetActive(false);
        }
    }
}
