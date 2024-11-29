using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위한 using 지시문

namespace TJ
{
    public class CardUpgradeUI : MonoBehaviour
    {
        public TMP_Text cardEnhancePriceText; // 강화 비용을 표시할 TMP_Text
        public GameManager gameManager;

        public void Awake()
        {
            gameManager = GameManager.Instance;
        }

        // 카드 데이터를 설정하는 메서드
        public void SetCardEnhancePrice(Card card)
        {
            // 강화 비용 텍스트 설정
            if (cardEnhancePriceText != null)
            {
                float tempnum = 1.0f;
                if (gameManager.PlayerHasRelic("넷플릭스 구독권"))
                {
                    tempnum = 0.75f;
                }
                int upgradeCost = (int)(card.cardPrice.cardEnhancePrice * tempnum);
                cardEnhancePriceText.text = $"비용: {upgradeCost} 골드";
            }
        }
    }
}