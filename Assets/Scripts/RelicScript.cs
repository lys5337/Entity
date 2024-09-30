using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace TJ
{
    public class RelicScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public TMP_Text tooltipText; // 툴팁 설명을 표시할 TextMeshPro 텍스트
        public GameObject tooltipObject; // 툴팁 오브젝트
        private Relic relicData; // 유물 데이터 (ScriptableObject)

        private bool isPointerOver = false; // 마우스가 오브젝트 위에 있는지 여부

        // 유물 데이터를 설정하는 함수
        public void SetRelicData(Relic relic)
        {
            relicData = relic;
        }

        // 마우스를 유물 위에 올렸을 때 툴팁 표시
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipObject != null && tooltipText != null && relicData != null)
            {
                tooltipText.text = $"{relicData.relicDescription}";
                tooltipObject.SetActive(true); // 툴팁 오브젝트 활성화
                isPointerOver = true; // 마우스가 오브젝트 위에 있음
            }
        }

        // 마우스를 유물에서 벗어났을 때 툴팁 숨기기
        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltipObject != null)
            {
                tooltipObject.SetActive(false); // 툴팁 오브젝트 비활성화
                isPointerOver = false; // 마우스가 오브젝트에서 벗어남
            }
        }

        private void Update()
        {
            // 마우스가 오브젝트 위에 있을 때만 툴팁 위치를 업데이트
            if (isPointerOver && tooltipObject != null)
            {
                // 마우스 포인터 위치를 기준으로 툴팁 오브젝트 위치 설정
                Vector3 mousePosition = Input.mousePosition;
                tooltipObject.transform.position = mousePosition + new Vector3(220, -140, 0); // 마우스 위치 기준으로 오프셋 적용
            }
        }
    }
}
