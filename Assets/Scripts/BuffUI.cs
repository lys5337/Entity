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
            // Animator가 존재하는지 확인하고, 파괴되지 않았다면 애니메이션 실행
            if (animator != null && animator.gameObject != null)
            {
                animator.Play("IntentSpawn");
            }
            else
            {
                Debug.LogWarning("Animator가 파괴되었거나 존재하지 않습니다.");
            }

            if (b.buffIcon != null)
            {
                buffImage.sprite = b.buffIcon;
            }
            else
            {
                Debug.LogError("버프 아이콘이 설정되지 않았습니다.");
            }

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
            if (gameObject != null)
            {
                Destroy(gameObject); // 버프 UI 제거
            }
        }
    }
}
