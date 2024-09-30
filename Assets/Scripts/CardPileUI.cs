using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용을 위한 네임스페이스

namespace TJ
{
    public class CardPileUI : MonoBehaviour
    {
        public Button discardPileButton; // discardPile을 보여주는 버튼
        public Button drawPileButton; // drawPile을 보여주는 버튼
        public TMP_Text discardPileText; // discardPile 내용을 보여줄 TextMeshPro 텍스트
        public TMP_Text drawPileText; // drawPile 내용을 보여줄 TextMeshPro 텍스트

        private BattleSceneManager battleSceneManager;
        private bool isDiscardPileVisible = false; // discardPile 표시 상태 확인용
        private bool isDrawPileVisible = false; // drawPile 표시 상태 확인용

        private void Start()
        {
            // BattleSceneManager 인스턴스를 찾아서 할당
            battleSceneManager = FindObjectOfType<BattleSceneManager>();

            if (battleSceneManager == null)
            {
                Debug.LogError("BattleSceneManager가 설정되지 않았습니다.");
                return;
            }

            // discardPile 버튼 클릭 이벤트 등록
            if (discardPileButton != null)
            {
                discardPileButton.onClick.AddListener(ToggleDiscardPile);
            }
            else
            {
                Debug.LogError("discardPileButton이 설정되지 않았습니다.");
            }

            // drawPile 버튼 클릭 이벤트 등록
            if (drawPileButton != null)
            {
                drawPileButton.onClick.AddListener(ToggleDrawPile);
            }
            else
            {
                Debug.LogError("drawPileButton이 설정되지 않았습니다.");
            }

            // 처음에는 텍스트를 비활성화
            discardPileText.gameObject.SetActive(false);
            drawPileText.gameObject.SetActive(false);
        }

        // discardPile 창 표시/숨기기
        public void ToggleDiscardPile()
        {
            isDiscardPileVisible = !isDiscardPileVisible; // 상태를 반전
            discardPileText.gameObject.SetActive(isDiscardPileVisible);

            if (isDiscardPileVisible)
            {
                ShowDiscardPile();
            }
        }

        // drawPile 창 표시/숨기기
        public void ToggleDrawPile()
        {
            isDrawPileVisible = !isDrawPileVisible; // 상태를 반전
            drawPileText.gameObject.SetActive(isDrawPileVisible);

            if (isDrawPileVisible)
            {
                ShowDrawPile();
            }
        }

        // discardPile 내용을 표시
        public void ShowDiscardPile()
        {
            if (battleSceneManager != null)
            {
                DisplayCardList(battleSceneManager.discardPile, discardPileText);
            }
        }

        // drawPile 내용을 표시
        public void ShowDrawPile()
        {
            if (battleSceneManager != null)
            {
                DisplayCardList(battleSceneManager.drawPile, drawPileText);
            }
        }

        // 카드 리스트 내용을 TextMeshPro 텍스트로 출력
        private void DisplayCardList(List<Card> cardList, TMP_Text targetText)
        {
            if (targetText == null)
            {
                Debug.LogError("targetText가 설정되지 않았습니다.");
                return;
            }

            targetText.text = ""; // 텍스트 초기화

            if (cardList != null && cardList.Count > 0)
            {
                foreach (Card card in cardList)
                {
                    targetText.text += card.cardTitle + "\n"; // 카드 이름을 줄바꿈하여 출력
                }
            }
            else
            {
                targetText.text = "카드가 없습니다.";
            }
        }
    }
}
