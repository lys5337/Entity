using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class CardItemUI : MonoBehaviour
    {
        public Transform cardDisplayParent; // 카드 프리팹을 표시할 부모 객체
        public GameObject cardUIPrefab; // Inspector에서 설정할 CardUI 프리팹
        public Text priceText; // 카드 가격을 표시할 텍스트
        public Button buyButton; // 구매 버튼

        private Item item; // 현재 UI에 표시되는 아이템 데이터
        private Shop shopManager; // 상점 관리 클래스 참조

        // 아이템이 카드일 경우 UI에 표시하는 메서드
        public void SetItem(Item newItem, Shop manager)
        {
            if (newItem == null || manager == null)
            {
                Debug.LogError("Item 또는 Shop이 null입니다.");
                return;
            }

            item = newItem;
            shopManager = manager;

            // 기존 카드 프리팹 제거
            foreach (Transform child in cardDisplayParent)
            {
                Destroy(child.gameObject);
            }

            // 아이템이 카드일 경우 카드 프리팹 표시
            if (!item.isRelic && item.card != null)
            {
                GameObject cardInstance = Instantiate(cardUIPrefab, cardDisplayParent);
                CardUI cardUI = cardInstance.GetComponent<CardUI>();

                if (cardUI != null)
                {
                    // CardUI의 LoadCard 메서드를 사용하여 모든 데이터를 로드
                    cardUI.LoadCard(item.card); // 카드 데이터 로드
                    cardUI.LoadCardRarity(); // 레어리티 로드 추가
                }
                else
                {
                    Debug.LogWarning("CardUIPrefab에 CardUI 컴포넌트가 없습니다.");
                }
            }

            // 카드 가격 계산 및 렐릭 효과 적용
            if (item.card != null)
            {
                int cardPrice = item.card.cardPrice.cardShopPrice;

                // "Chat-GPT 구독권" 렐릭이 있는 경우 가격을 25% 감소
                if (GameManager.Instance.PlayerHasRelic("Chat-GPT 구독권"))
                {
                    cardPrice = Mathf.CeilToInt(cardPrice * 0.75f); // 가격을 25% 감소시키고 올림 처리
                }

                priceText.text = $"{cardPrice} 골드"; // 감소된 가격을 텍스트에 표시
            }

            // 구매 버튼 리스너 설정
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => shopManager.BuyItem(item, buyButton));
        }
    }
}
