using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class PlayerStatsUI : MonoBehaviour
    {
        public TMP_Text healthDisplayText;
        public TMP_Text moneyAmountText;
        public TMP_Text floorText;
        public Transform relicParent;
        public GameObject relicPrefab;
        public GameObject playerStatsUIObject;

        private void Awake()
        {
            // 게임 매니저의 인스턴스가 존재하는지 확인하고, 플레이어 스탯 UI 객체를 설정합니다.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerStatsUI = this;
            }
        }

        public void DisplayRelics()
        {
            // 모든 기존 자식 객체를 제거합니다.
            foreach (Transform c in relicParent)
                Destroy(c.gameObject);

            // 게임 매니저의 인스턴스에서 유물을 가져와서 UI에 표시합니다.
            foreach (Relic r in GameManager.Instance.relics)
            {
                GameObject relicObject = Instantiate(relicPrefab, relicParent);
                relicObject.GetComponent<Image>().sprite = r.relicIcon;
            }
        }
    }
}