using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace TJ
{
    public class RelicScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public TMP_Text tooltipText; // 툴팁 설명을 표시할 TextMeshPro 텍스트
        public GameObject tooltipObject; // 툴팁 오브젝트
        private RectTransform tooltipRectTransform; // 툴팁의 RectTransform
        private Canvas parentCanvas; // UI 캔버스
        private Relic relicData; // 유물 데이터 (ScriptableObject)

        public delegate void RelicClickedHandler(Relic relic);
        public event RelicClickedHandler OnRelicClicked; // Relic 클릭 이벤트

        private bool isPointerOver = false; // 마우스가 오브젝트 위에 있는지 여부

        // 유물 데이터를 설정하는 함수
        public void SetRelicData(Relic relic)
        {
            relicData = relic;
        }

        private void Start()
        {
            if (tooltipObject != null)
            {
                tooltipRectTransform = tooltipObject.GetComponent<RectTransform>();
                parentCanvas = tooltipObject.GetComponentInParent<Canvas>();
            }
        }

        // 마우스를 유물 위에 올렸을 때 툴팁 표시
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tooltipObject != null && tooltipText != null && relicData != null)
            {
                tooltipText.text = relicData.relicDescription;
                tooltipObject.SetActive(true); // 툴팁 오브젝트 활성화
                isPointerOver = true; // 마우스가 오브젝트 위에 있음
                UpdateTooltipPosition();
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

        // 유물을 클릭했을 때 이벤트 호출
        public void OnPointerClick(PointerEventData eventData)
        {
            OnRelicClicked?.Invoke(relicData); // 클릭 이벤트 발생
        }

        private void Update()
        {
            if (isPointerOver && tooltipObject != null)
            {
                UpdateTooltipPosition();
            }
        }

        // 툴팁의 위치를 업데이트
        private void UpdateTooltipPosition()
        {
            if (parentCanvas == null || tooltipRectTransform == null)
                return;

            Vector2 mousePosition = Input.mousePosition;

            // 월드 좌표에서 로컬 캔버스 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                mousePosition,
                parentCanvas.worldCamera,
                out Vector2 localPoint
            );

            // 툴팁의 위치를 캔버스 로컬 좌표에 맞춰 설정
            tooltipRectTransform.localPosition = localPoint + new Vector2(200, -150); // 오프셋 추가
        }
    }
}
