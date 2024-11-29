using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextShift : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI buttonText; // 버튼의 텍스트
    public Vector2 pressedOffset = new Vector2(0, -5); // 눌렀을 때 이동할 위치

    private Vector2 originalPosition;

    private void Start()
    {
        if (buttonText != null)
            originalPosition = buttonText.rectTransform.anchoredPosition; // 원래 위치 저장
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 버튼을 누를 때 텍스트를 아래로 이동
        if (buttonText != null)
            buttonText.rectTransform.anchoredPosition = originalPosition + pressedOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 버튼에서 손을 뗄 때 원래 위치로 복귀
        if (buttonText != null)
            buttonText.rectTransform.anchoredPosition = originalPosition;
    }
}