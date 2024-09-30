using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class TendencyBar : MonoBehaviour
    {
        public Image tendencyBackground; // 성향치 배경 이미지
        public Image tendencyIcon; // 성향치 아이콘 이미지
        public TMP_Text tendencyValueText; // 성향치 값을 표시할 텍스트
        public Slider tendencySlider; // 성향치 바 슬라이더

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                // GameManager에서 성향치 값 가져오기
                tendencySlider.maxValue = GameManager.Instance.maxTendencyValue;

                // 성향치 UI를 업데이트
                UpdateTendencyBar();
            }
            else
            {
                Debug.LogError("GameManager Instance가 설정되지 않았습니다.");
            }
        }

        private void OnValidate()
        {
            if (GameManager.Instance != null)
            {
                UpdateTendencyBar();
            }
        }

        // 성향치 바를 업데이트하는 함수
        private void UpdateTendencyBar()
        {
            if (GameManager.Instance != null)
            {
                // 성향치 값을 GameManager에서 가져와 텍스트로 표시
                tendencyValueText.text = GameManager.Instance.currentTendencyValue.ToString();

                // 성향치 바를 업데이트
                tendencySlider.value = GameManager.Instance.currentTendencyValue;
            }
        }

        // 성향치를 수동으로 설정하는 함수
        public void SetTendencyValue(int value)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentTendencyValue = Mathf.Clamp(value, 0, GameManager.Instance.maxTendencyValue); // 0과 최대 값 사이로 제한
                UpdateTendencyBar();
            }
        }
    }
}
