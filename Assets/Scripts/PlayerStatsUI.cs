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
            // ���� �Ŵ����� �ν��Ͻ��� �����ϴ��� Ȯ���ϰ�, �÷��̾� ���� UI ��ü�� �����մϴ�.
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerStatsUI = this;
            }
        }

        public void DisplayRelics()
        {
            // ��� ���� �ڽ� ��ü�� �����մϴ�.
            foreach (Transform c in relicParent)
                Destroy(c.gameObject);

            // ���� �Ŵ����� �ν��Ͻ����� ������ �����ͼ� UI�� ǥ���մϴ�.
            foreach (Relic r in GameManager.Instance.relics)
            {
                GameObject relicObject = Instantiate(relicPrefab, relicParent);
                relicObject.GetComponent<Image>().sprite = r.relicIcon;
            }
        }
    }
}