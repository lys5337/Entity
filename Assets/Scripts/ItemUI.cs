using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class ItemUI : MonoBehaviour
    {
        public Text itemNameText;
        public Text priceText;
        public Button buyButton;
        public Image itemImage; // 아이템 또는 렐릭 이미지를 표시할 Image 컴포넌트 추가

        private Item item;
        private Shop shopManager;

        public void SetItem(Item newItem, Shop manager)
        {
            item = newItem;
            shopManager = manager;

            if (itemNameText == null)
            {
                Debug.LogError("itemNameText가 설정되지 않았습니다.");
            }

            if (priceText == null)
            {
                Debug.LogError("priceText가 설정되지 않았습니다.");
            }

            if (buyButton == null)
            {
                Debug.LogError("buyButton이 설정되지 않았습니다.");
            }

            if (itemImage == null)
            {
                Debug.LogError("itemImage가 설정되지 않았습니다.");
            }

            itemNameText.text = item.itemName;
            priceText.text = item.price.ToString();
            itemImage.sprite = item.itemSprite; // 아이템 이미지 설정

            buyButton.onClick.AddListener(() => shopManager.BuyItem(item, buyButton));
        }

        // 렐릭을 설정하는 메서드 추가
        public void SetRelic(Item relicItem, Shop manager)
        {
            SetItem(relicItem, manager); // 렐릭도 SetItem으로 처리
        }
    }
}
