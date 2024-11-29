using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class CardDeckSelect : MonoBehaviour
    {
        public Button deck1Button; // 덱 1 선택 버튼
        public Button deck2Button; // 덱 2 선택 버튼
        public Button deck3Button; // 덱 3 선택 버튼

        public Button closeButton; // 닫기 버튼
        public Button confirmButton; // 결정 버튼

        public GameObject cardPanel; // 카드들을 보여줄 패널
        public GameObject cardPrefab; // 카드 프리팹 오브젝트 (UI에 보여줄 카드 형태)

        public List<Card> deck1Cards = new List<Card>(); // 덱 1의 카드 리스트
        public List<Card> deck2Cards = new List<Card>(); // 덱 2의 카드 리스트
        public List<Card> deck3Cards = new List<Card>(); // 덱 3의 카드 리스트

        private List<Card> currentSelectedDeck = new List<Card>(); // 현재 선택된 덱

        private void Start()
        {
            // 버튼에 OnClick 이벤트 연결
            if (deck1Button == null || deck2Button == null || deck3Button == null || cardPanel == null || cardPrefab == null)
            {
                Debug.LogError("UI 요소가 할당되지 않았습니다. 인스펙터에서 확인하세요.");
                return;
            }

            deck1Button.onClick.AddListener(() => SelectDeck(deck1Cards));
            deck2Button.onClick.AddListener(() => SelectDeck(deck2Cards));
            deck3Button.onClick.AddListener(() => SelectDeck(deck3Cards));

            // 닫기 및 결정 버튼을 숨김
            closeButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            cardPanel.SetActive(false); // 카드 패널도 기본적으로 비활성화

            // 결정 버튼 클릭 시, GameManager의 playerDeck에 덱을 추가
            confirmButton.onClick.AddListener(AddSelectedDeckToPlayerDeck);
        }

        // 덱 선택 함수
        public void SelectDeck(List<Card> selectedDeck)
        {
            if (selectedDeck == null || cardPanel == null || cardPrefab == null)
            {
                Debug.LogError("선택된 덱 또는 UI 요소가 null입니다.");
                return;
            }

            currentSelectedDeck = selectedDeck; // 선택된 덱을 현재 덱으로 저장

            // 각 카드의 업그레이드 상태를 초기화
            foreach (Card card in currentSelectedDeck)
            {
                if (card != null)
                {
                    card.isUpgraded = false; // 업그레이드 상태 초기화
                }
            }

            // 카드 패널을 보여줌
            cardPanel.SetActive(true);

            // 패널 안에 기존의 카드 오브젝트들을 모두 제거 (초기화)
            foreach (Transform child in cardPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // 선택한 덱의 카드 리스트를 패널에 표시
            foreach (Card card in selectedDeck)
            {
                if (card != null)
                {
                    GameObject cardObj = Instantiate(cardPrefab, cardPanel.transform); // 카드 프리팹 생성
                    CardUI cardUI = cardObj.GetComponent<CardUI>(); // CardUI 스크립트 가져오기

                    if (cardUI != null)
                    {
                        cardUI.LoadCard(card); // 카드 데이터를 CardUI에 설정
                    }
                    else
                    {
                        Debug.LogError("CardUI 스크립트가 카드 프리팹에 없습니다.");
                    }
                }
            }

            // 닫기 버튼과 결정 버튼을 활성화
            closeButton.gameObject.SetActive(true);
            confirmButton.gameObject.SetActive(true);
        }

        // 선택된 덱을 GameManager의 playerDeck에 추가하는 함수
        private void AddSelectedDeckToPlayerDeck()
        {
            if (currentSelectedDeck == null || currentSelectedDeck.Count == 0)
            {
                Debug.LogWarning("선택된 덱이 없습니다.");
                return;
            }

            GameManager.Instance.playerDeck.Clear();
            GameManager.Instance.playerBattleDeck.Clear();

            // 선택된 덱의 카드를 플레이어 덱에 추가
            foreach (Card card in currentSelectedDeck)
            {
                if (card != null)
                {
                    GameManager.Instance.playerBattleDeck.Add(card);
                }
            }

            Debug.Log($"선택한 덱이 플레이어 덱에 추가되었습니다. 현재 덱의 카드 수: {GameManager.Instance.playerDeck.Count}");
            GameManager.Instance.InitializeDeck();
        }

        // 닫기 버튼 클릭 시 호출될 함수
        public void CloseButtons()
        {
            cardPanel.SetActive(false); // 카드 패널 숨김
            closeButton.gameObject.SetActive(false);
            confirmButton.gameObject.SetActive(false);
        }
    }
}
