using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP_Text를 위한 using 지시문 추가

namespace TJ
{
    public class CardGenerateUI_5 : MonoBehaviour
    {
        public List<Card> cards = new List<Card>(); // 전체 카드 리스트
        public GameObject card1Prefab; // 첫 번째 카드 프리팹
        public GameObject card2Prefab; // 두 번째 카드 프리팹
        public GameObject card3Prefab; // 세 번째 카드 프리팹
        public GameObject card4Prefab; // 네 번째 카드 프리팹 추가
        public GameObject card5Prefab; // 다섯 번째 카드 프리팹 추가
        public Button card1Button; // 첫 번째 카드 버튼
        public Button card2Button; // 두 번째 카드 버튼
        public Button card3Button; // 세 번째 카드 버튼
        public Button card4Button; // 네 번째 카드 버튼 추가
        public Button card5Button; // 다섯 번째 카드 버튼 추가
        public GameObject cardDisplayParent; // 카드를 표시하는 UI 창

        private List<Card> selectedCards = new List<Card>(); // UI에 표시된 카드 정보를 저장할 리스트

        private BattleSceneManager battleSceneManager; // BattleSceneManager 참조

        private void Start()
        {
            // BattleSceneManager 찾기
            battleSceneManager = FindObjectOfType<BattleSceneManager>();

            if (battleSceneManager == null)
            {
                Debug.LogError("BattleSceneManager를 찾을 수 없습니다.");
                return;
            }

            AssignRandomCardsToPrefabs();
            AssignButtonListeners();
        }

        // 랜덤으로 카드 5장을 선택하여 프리팹에 할당하는 함수
        public void AssignRandomCardsToPrefabs()
        {
            if (cards.Count < 5)
            {
                Debug.LogWarning("카드 리스트에 충분한 카드가 없습니다.");
                return;
            }

            for (int i = 0; i < 5; i++)
            {
                int randomIndex = Random.Range(0, cards.Count);
                selectedCards.Add(cards[randomIndex]);

                switch (i)
                {
                    case 0:
                        AssignCardToPrefab(card1Prefab, selectedCards[0]);
                        break;
                    case 1:
                        AssignCardToPrefab(card2Prefab, selectedCards[1]);
                        break;
                    case 2:
                        AssignCardToPrefab(card3Prefab, selectedCards[2]);
                        break;
                    case 3:
                        AssignCardToPrefab(card4Prefab, selectedCards[3]);
                        break;
                    case 4:
                        AssignCardToPrefab(card5Prefab, selectedCards[4]);
                        break;
                }
            }
        }

        // 버튼 리스너 할당 함수
        private void AssignButtonListeners()
        {
            if (card1Button != null)
            {
                card1Button.onClick.RemoveAllListeners();
                card1Button.onClick.AddListener(() => OnCardButtonClick(selectedCards[0]));
            }
            else
            {
                Debug.LogWarning("card1Button이 할당되지 않았습니다.");
            }

            if (card2Button != null)
            {
                card2Button.onClick.RemoveAllListeners();
                card2Button.onClick.AddListener(() => OnCardButtonClick(selectedCards[1]));
            }
            else
            {
                Debug.LogWarning("card2Button이 할당되지 않았습니다.");
            }

            if (card3Button != null)
            {
                card3Button.onClick.RemoveAllListeners();
                card3Button.onClick.AddListener(() => OnCardButtonClick(selectedCards[2]));
            }
            else
            {
                Debug.LogWarning("card3Button이 할당되지 않았습니다.");
            }

            if (card4Button != null)
            {
                card4Button.onClick.RemoveAllListeners();
                card4Button.onClick.AddListener(() => OnCardButtonClick(selectedCards[3]));
            }
            else
            {
                Debug.LogWarning("card4Button이 할당되지 않았습니다.");
            }

            if (card5Button != null)
            {
                card5Button.onClick.RemoveAllListeners();
                card5Button.onClick.AddListener(() => OnCardButtonClick(selectedCards[4]));
            }
            else
            {
                Debug.LogWarning("card5Button이 할당되지 않았습니다.");
            }
        }

        // 카드 버튼 클릭 시 호출되는 함수
        private void OnCardButtonClick(Card selectedCard)
        {
            if (battleSceneManager != null && selectedCard != null)
            {
                // drawPile에 카드 추가
                battleSceneManager.drawPile.Add(selectedCard);

                // drawPileCountText 갱신
                if (battleSceneManager.drawPileCountText != null)
                {
                    battleSceneManager.drawPileCountText.text = battleSceneManager.drawPile.Count.ToString();
                }
            }

            cardDisplayParent.SetActive(false); // 창 비활성화
        }

        // 카드 데이터를 프리팹에 할당하는 함수
        private void AssignCardToPrefab(GameObject cardPrefab, Card cardData)
        {
            if (cardPrefab == null || cardData == null)
            {
                Debug.LogError("카드 프리팹 또는 카드 데이터가 null입니다.");
                return;
            }

            CardUI cardUI = cardPrefab.GetComponent<CardUI>();
            if (cardUI != null)
            {
                cardUI.LoadCard(cardData); // 카드 데이터를 UI에 설정
            }
            else
            {
                Debug.LogError("CardUI 스크립트가 카드 프리팹에 없습니다.");
            }
        }
    }
}
