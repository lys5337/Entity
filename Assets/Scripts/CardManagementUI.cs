using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button 타입을 위한 using 지시문 추가

namespace TJ
{
    public class CardManagementUI : MonoBehaviour
    {
        public GameObject cardListPanel; // 카드 리스트를 담을 패널 (활성화/비활성화 용도)
        public Transform cardListContainer; // 카드 리스트를 담을 부모 객체
        public GameObject cardItemPrefab; // 일반 카드 리스트 아이템 프리팹
        public GameObject removeCardItemPrefab; // 카드 제거용 아이템 프리팹
        public Button closeButton; // 카드 리스트 패널을 끄는 버튼

        private Shop shop;

        private void Start()
        {
            shop = FindObjectOfType<Shop>();
            cardListPanel.SetActive(false); // 초기에는 카드 리스트 패널 비활성화

            // 닫기 버튼에 HideCardList 함수 연결
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideCardList);
            }
            else
            {
                Debug.LogError("닫기 버튼이 설정되지 않았습니다.");
            }
        }

        // 카드 리스트를 보여주는 메서드
        public void ShowCardList(bool isRemoving)
        {
            cardListPanel.SetActive(true);
            PopulateCardList(isRemoving);
        }

        // 카드 리스트를 채우는 메서드
        private void PopulateCardList(bool isRemoving)
        {
            // 기존 카드 리스트 제거
            foreach (Transform child in cardListContainer)
            {
                Destroy(child.gameObject);
            }

            // playerDeck의 카드 리스트 생성
            foreach (Card card in shop.gameManager.playerDeck)
            {
                CreateCardItem(card, isRemoving, true); // playerDeck의 카드를 생성
            }

            // playerBattleDeck의 카드 리스트 생성
            foreach (Card card in shop.gameManager.playerBattleDeck)
            {
                CreateCardItem(card, isRemoving, false); // playerBattleDeck의 카드를 생성
            }
        }

        // 카드 아이템을 생성하는 메서드
        private void CreateCardItem(Card card, bool isRemoving, bool isInPlayerDeck)
        {
            // 제거용 프리팹과 일반 프리팹 선택
            GameObject prefabToUse = isRemoving ? removeCardItemPrefab : cardItemPrefab;

            GameObject cardItemGO = Instantiate(prefabToUse, cardListContainer);
            CardUI cardUI = cardItemGO.GetComponent<CardUI>();
            cardUI.LoadCard(card); // 기존의 카드 데이터를 설정

            // CardUpgradeUI에 카드 데이터를 설정
            CardUpgradeUI cardUpgradeUI = cardItemGO.GetComponent<CardUpgradeUI>();
            if (cardUpgradeUI != null)
            {
                cardUpgradeUI.SetCardEnhancePrice(card); // 강화 비용을 설정
            }

            // 카드 클릭 시 처리
            cardItemGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (isRemoving)
                {
                    shop.RemoveCard(card, isInPlayerDeck);
                }
                else
                {
                    shop.UpgradeCard(card);
                }
            });
        }

        public void HideCardList()
        {
            cardListPanel.SetActive(false); // 카드 리스트 패널 비활성화
        }
    }
}
