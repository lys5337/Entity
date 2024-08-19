using UnityEngine;

namespace TJ
{
    [System.Serializable]
    public class Item
    {
        public string itemName; // ������ �̸� (ī�� �Ǵ� ������ �̸�)
        public int price; // ������ ����
        public Sprite itemSprite; // ������ �̹����� ���� �ʵ� (ī�� �Ǵ� ������ �̹���)
        public Card card; // ī�� ������
        public Relic relic; // ���� ������

        public bool isRelic; // true�� ��� ����, false�� ��� ī��

        public void SetItemDetails(string name, int itemPrice, Sprite sprite, Card cardItem)
        {
            itemName = name;
            price = itemPrice;
            itemSprite = sprite;
            card = cardItem;
            isRelic = false; // ī�� �������� ��� false
        }

        public void SetRelicDetails(string name, int relicPrice, Sprite sprite, Relic relicItem)
        {
            itemName = name;
            price = relicPrice;
            itemSprite = sprite;
            relic = relicItem;
            isRelic = true; // ���� �������� ��� true
        }
    }
}
