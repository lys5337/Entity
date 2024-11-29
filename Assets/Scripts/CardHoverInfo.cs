using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace TJ
{
    public class CardHoverInfo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public TMP_Text tooltipText; // 피해량 정보를 표시할 TextMeshPro 텍스트
        public GameObject tooltipObject; // 툴팁 오브젝트
        private Card cardData; // 카드 데이터
        private Fighter player; // 플레이어 객체
        private Camera mainCamera; // 메인 카메라
        private bool isDragging; // 드래그 중인지 여부

        public void SetCardData(Card card, Fighter player)
        {
            this.cardData = card;
            this.player = player;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            tooltipObject.SetActive(false); // 처음에는 툴팁을 비활성화
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (cardData != null && player != null)
            {
                isDragging = true;
                tooltipObject.SetActive(true);
                UpdateTooltip(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isDragging)
            {
                UpdateTooltip(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            tooltipObject.SetActive(false); // 드래그가 끝나면 툴팁 비활성화
        }

        private void UpdateTooltip(PointerEventData eventData)
        {
            // 카드의 총 피해량 계산 (기본 피해 + 플레이어 공격력)
            int totalDamage = cardData.cardEffect.baseAmount + player.GetPlayerPower();

            // 툴팁에 총 피해량 업데이트
            tooltipText.text = $"{totalDamage}의 피해";

            // 툴팁 위치를 마우스 위치에 맞게 업데이트
            Vector3 tooltipPosition = Input.mousePosition;
            tooltipObject.transform.position = tooltipPosition + new Vector3(20, -20, 0); // 약간의 오프셋 적용
        }
    }
}
