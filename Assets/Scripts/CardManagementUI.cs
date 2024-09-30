using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Button 타입을 위한 using 지시문 추가

namespace TJ
{
    public class CardManagementUI : MonoBehaviour
    {
        public GameObject cardListPanel; // 카드 리스트를 담을 패널 (활성화/비활성화 용도)
        public Transform cardListContainer; // 카드 리스트를 담을 부모 객체
        public GameObject cardItemPrefab; // 카드 리스트 아이템 프리팹
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

            // 플레이어 덱에서 카드 리스트 생성
            foreach (Card card in shop.gameManager.playerDeck)
            {
                GameObject cardItemGO = Instantiate(cardItemPrefab, cardListContainer);
                CardUI cardUI = cardItemGO.GetComponent<CardUI>();
                cardUI.LoadCard(card);

                // 카드 클릭 시 처리
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
            cardListPanel.SetActive(false); // 카드 리스트 패널 비활성화
        }
    }
}
