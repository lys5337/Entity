using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class PlayerNowDeck : MonoBehaviour
    {
        public GameObject deckDisplayPanel; // 덱을 표시할 패널
        public ScrollRect deckScrollView; // 스크롤 뷰
        public Button showDeckButton; // 덱을 표시하는 버튼
        public Button closeDeckButton; // 덱 패널을 닫는 버튼
        public GameObject cardPrefab; // 카드 정보를 나타낼 프리팹
        public Transform cardContainer; // 카드 프리팹을 배치할 부모 오브젝트

        private void Start()
        {
            // 덱 표시 버튼에 DisplayDeck 메서드 연결
            if (showDeckButton != null)
            {
                showDeckButton.onClick.AddListener(DisplayDeck);
            }

            // 닫기 버튼에 HideDeck 메서드 연결
            if (closeDeckButton != null)
            {
                closeDeckButton.onClick.AddListener(HideDeck);
            }

            // 패널과 스크롤 뷰를 처음에는 비활성화 상태로 시작
            deckDisplayPanel.SetActive(false);
            deckScrollView.gameObject.SetActive(false);
        }

        // 덱을 UI로 표시하는 메서드
        public void DisplayDeck()
        {
            // 이전에 생성된 카드 프리팹을 모두 삭제하여 초기화
            foreach (Transform child in cardContainer)
            {
                Destroy(child.gameObject);
            }

            // GameManager에서 플레이어 덱 가져오기
            List<Card> playerDeck = GameManager.Instance.playerDeck;

            // 각 카드에 대해 프리팹을 생성하여 표시
            foreach (Card card in playerDeck)
            {
                GameObject cardObject = Instantiate(cardPrefab, cardContainer);

                // 생성된 카드 오브젝트의 CardUI 컴포넌트를 사용하여 카드 정보 설정
                CardUI cardUI = cardObject.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.LoadCard(card); // CardUI의 LoadCard 메서드로 카드 정보 전달
                }
            }

            // 덱 패널과 스크롤 뷰를 활성화하여 덱을 보여줌
            deckDisplayPanel.SetActive(true);
            deckScrollView.gameObject.SetActive(true);
        }

        // 덱 패널과 스크롤 뷰를 숨기는 메서드
        public void HideDeck()
        {
            deckDisplayPanel.SetActive(false);
            deckScrollView.gameObject.SetActive(false);
        }
    }
}
