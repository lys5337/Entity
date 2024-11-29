using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TJ
{
    public class RelicRewardUI : MonoBehaviour
    {
        Card card;
        public GameObject common;
        public GameObject uncommon;
        public GameObject rare;
        public GameObject epic;
        public GameObject legendary;
        public GameObject hidden;

        public Image relicImage;
        public TMP_Text relicName;
        public TMP_Text relicDescription;
        public TMP_Text cardCostText; // 카드 비용을 표시할 UI 요소 추가

        public void DisplayRelic(Relic r)
        {
            relicImage.sprite = r.relicIcon;
            relicName.text = r.relicName;
            relicDescription.text = r.relicDescription;
        }

        public void DisplayCard(Card r)
        {
            relicImage.sprite = r.cardIcon;
            relicName.text = r.cardTitle;
            relicDescription.text = r.GetCardDescriptionAmount();
            cardCostText.text = r.GetCardCostAmount().ToString();
        }
    }
}
