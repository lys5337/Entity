using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // EventSystem을 사용하기 위해 추가

namespace TJ
{
    public class CardPileUI : MonoBehaviour
    {
        public Button discardPileButton; // discardPile을 보여주는 버튼
        public Button drawPileButton; // drawPile을 보여주는 버튼
        public GameObject discardPilePanel; // discardPile 카드들을 표시할 패널
        public GameObject drawPilePanel; // drawPile 카드들을 표시할 패널
        public GameObject cardPrefab; // 카드 UI를 보여줄 프리팹

        // 추가: 스크롤 뷰와 콘텐츠 패널
        public GameObject discardPileScrollView; // discardPile 스크롤 뷰
        public GameObject drawPileScrollView; // drawPile 스크롤 뷰
        public RectTransform discardPileContent; // discardPile 콘텐츠 영역
        public RectTransform drawPileContent; // drawPile 콘텐츠 영역

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

            // drawPile 버튼 클릭 이벤트 등록
            if (drawPileButton != null)
            {
                drawPileButton.onClick.AddListener(ToggleDrawPile);
            }

            // 패널에 클릭 이벤트 차단을 위한 EventTrigger 추가
            AddEventTrigger(discardPilePanel);
            AddEventTrigger(drawPilePanel);
            AddEventTrigger(discardPileScrollView);
            AddEventTrigger(drawPileScrollView);

            // 처음에는 패널 및 스크롤 뷰를 비활성화
            discardPilePanel.SetActive(false);
            drawPilePanel.SetActive(false);
            discardPileScrollView.SetActive(false);
            drawPileScrollView.SetActive(false);
        }

        // discardPile 창 표시/숨기기 (버튼을 클릭했을 때만 동작)
        public void ToggleDiscardPile()
        {
            if (isDrawPileVisible)
            {
                ToggleDrawPile(); // DrawPile이 열려 있으면 닫기
            }

            isDiscardPileVisible = !isDiscardPileVisible; // 상태를 반전
            discardPilePanel.SetActive(isDiscardPileVisible);
            discardPileScrollView.SetActive(isDiscardPileVisible); // 스크롤 뷰도 함께 표시/숨기기

            if (isDiscardPileVisible)
            {
                ShowDiscardPile();
            }
            else
            {
                ClearPanel(discardPileContent.gameObject); // 패널에서 카드 프리팹 삭제
            }
        }

        // drawPile 창 표시/숨기기 (버튼을 클릭했을 때만 동작)
        public void ToggleDrawPile()
        {
            if (isDiscardPileVisible)
            {
                ToggleDiscardPile(); // DiscardPile이 열려 있으면 닫기
            }

            isDrawPileVisible = !isDrawPileVisible; // 상태를 반전
            drawPilePanel.SetActive(isDrawPileVisible);
            drawPileScrollView.SetActive(isDrawPileVisible); // 스크롤 뷰도 함께 표시/숨기기

            if (isDrawPileVisible)
            {
                ShowDrawPile();
            }
            else
            {
                ClearPanel(drawPileContent.gameObject); // 패널에서 카드 프리팹 삭제
            }
        }

        // 패널이나 스크롤 뷰에 클릭 이벤트를 추가하여 이벤트 전달을 차단
        private void AddEventTrigger(GameObject target)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = target.AddComponent<EventTrigger>();
            }

            // 이벤트 트리거 엔트리 생성
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick; // 클릭 이벤트
            entry.callback.AddListener((data) => { BlockPanelClick(); }); // 클릭 시 BlockPanelClick 실행

            // 이벤트 트리거에 엔트리 추가
            trigger.triggers.Add(entry);
        }

        // 패널 클릭 이벤트를 차단하는 메서드
        private void BlockPanelClick()
        {
            Debug.Log("패널 클릭 이벤트가 차단되었습니다.");
            // 아무 동작도 하지 않도록 하여 클릭 이벤트가 패널에 전달되지 않도록 함
        }

        // discardPile 내용을 표시
        public void ShowDiscardPile()
        {
            if (battleSceneManager != null)
            {
                DisplayCards(battleSceneManager.discardPile, discardPileContent.gameObject);
            }
        }

        // drawPile 내용을 표시
        public void ShowDrawPile()
        {
            if (battleSceneManager != null)
            {
                DisplayCards(battleSceneManager.drawPile, drawPileContent.gameObject);
            }
        }

        // 카드 리스트를 받아 패널에 카드 프리팹을 생성하여 표시
        private void DisplayCards(List<Card> cardList, GameObject targetContent)
        {
            // 패널 초기화 (기존에 남아 있는 카드 프리팹이 있을 수 있으므로 모두 제거)
            ClearPanel(targetContent);

            if (cardList != null && cardList.Count > 0)
            {
                foreach (Card card in cardList)
                {
                    // 카드 프리팹 생성
                    GameObject cardObject = Instantiate(cardPrefab, targetContent.transform);

                    // 카드 프리팹의 CardUI 컴포넌트를 가져와 카드 데이터를 로드
                    CardUI cardUI = cardObject.GetComponent<CardUI>();
                    if (cardUI != null)
                    {
                        cardUI.LoadCard(card); // 카드 데이터 로드
                    }
                    else
                    {
                        Debug.LogError("CardUI 컴포넌트가 카드 프리팹에 존재하지 않습니다.");
                    }
                }
            }
        }

        // 패널에 있는 모든 자식 오브젝트 (카드 프리팹) 삭제
        private void ClearPanel(GameObject panel)
        {
            foreach (Transform child in panel.transform)
            {
                Destroy(child.gameObject); // 패널의 모든 자식 오브젝트 삭제
            }
        }
    }
}
