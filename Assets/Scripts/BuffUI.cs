using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class BuffUI : MonoBehaviour
    {
        public Image buffImage;
        public TMP_Text buffAmountText;
        public Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        // 버프 표시
        public void DisplayBuff(Buff b)
        {
            animator.Play("IntentSpawn");
            buffImage.sprite = b.buffIcon;
            buffAmountText.text = b.buffValue.ToString();
        }

        // 버프 수치 업데이트
        public void UpdateBuffAmount(int amount)
        {
            buffAmountText.text = amount.ToString();
        }

        // 버프 제거
        public void RemoveBuff()
        {
            Destroy(gameObject); // 버프 UI 제거
        }
    }
}