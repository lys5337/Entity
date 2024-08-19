using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class ItemUI : MonoBehaviour
    {
        public Text itemNameText;
        public Text priceText;
        public Button buyButton;
        public Image itemImage; // ������ �Ǵ� ���� �̹����� ǥ���� Image ������Ʈ �߰�

        private Item item;
        private Shop shopManager;

        public void SetItem(Item newItem, Shop manager)
        {
            item = newItem;
            shopManager = manager;

            if (itemNameText == null)
            {
                Debug.LogError("itemNameText�� �������� �ʾҽ��ϴ�.");
            }

            if (priceText == null)
            {
                Debug.LogError("priceText�� �������� �ʾҽ��ϴ�.");
            }

            if (buyButton == null)
            {
                Debug.LogError("buyButton�� �������� �ʾҽ��ϴ�.");
            }

            if (itemImage == null)
            {
                Debug.LogError("itemImage�� �������� �ʾҽ��ϴ�.");
            }

            itemNameText.text = item.itemName;
            priceText.text = item.price.ToString();
            itemImage.sprite = item.itemSprite; // ������ �̹��� ����

            buyButton.onClick.AddListener(() => shopManager.BuyItem(item, buyButton));
        }

        // ������ �����ϴ� �޼��� �߰�
        public void SetRelic(Item relicItem, Shop manager)
        {
            SetItem(relicItem, manager); // ������ SetItem���� ó��
        }
    }
}
