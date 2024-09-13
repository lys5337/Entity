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
            // GameManager.Instance가 null인지 확인
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerStatsUI = this;
            }
        }

        public void DisplayRelics()
        {
            // GameManager.Instance가 null인지 확인
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager 인스턴스가 null입니다. 유물 정보를 표시할 수 없습니다.");
                return;
            }

            // 유물 리스트가 null인지 확인
            if (GameManager.Instance.relics == null || GameManager.Instance.relics.Count == 0)
            {
                Debug.LogWarning("유물 리스트가 비어 있거나 null입니다.");
                return;
            }

            // 모든 기존 자식 객체를 제거합니다.
            foreach (Transform c in relicParent)
                Destroy(c.gameObject);

            // 유물 리스트를 순회하면서 UI에 표시합니다.
            foreach (Relic r in GameManager.Instance.relics)
            {
                if (r != null)
                {
                    GameObject relicObject = Instantiate(relicPrefab, relicParent);
                    relicObject.GetComponent<Image>().sprite = r.relicIcon;
                }
                else
                {
                    Debug.LogWarning("유물 리스트에 null 값이 포함되어 있습니다.");
                }
            }
        }
    }
}
