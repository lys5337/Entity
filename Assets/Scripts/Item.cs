using UnityEngine;

namespace TJ
{
    [System.Serializable]
    public class Item
    {
        public string itemName; // 아이템 이름 (카드 또는 렐릭의 이름)
        public int price; // 아이템 가격
        public Sprite itemSprite; // 아이템 이미지를 위한 필드 (카드 또는 렐릭의 이미지)
        public Card card; // 카드 아이템
        public Relic relic; // 렐릭 아이템

        public bool isRelic; // true일 경우 렐릭, false일 경우 카드

        public void SetItemDetails(string name, int itemPrice, Sprite sprite, Card cardItem)
        {
            itemName = name;
            price = itemPrice;
            itemSprite = sprite;
            card = cardItem;
            isRelic = false; // 카드 아이템일 경우 false
        }

        public void SetRelicDetails(string name, int relicPrice, Sprite sprite, Relic relicItem)
        {
            itemName = name;
            price = relicPrice;
            itemSprite = sprite;
            relic = relicItem;
            isRelic = true; // 렐릭 아이템일 경우 true
        }
    }
}
