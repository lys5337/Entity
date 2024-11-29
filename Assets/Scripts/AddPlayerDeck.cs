using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class AddPlayerDeck : MonoBehaviour
    {
        public Button addButton; // Unity 에디터에서 할당할 버튼
        public Card card; // 이동할 카드
        private CardUI cardUI; // CardUI 컴포넌트 참조
        private PlayerStatsUI playerStatsUI; // PlayerStatsUI 참조

        private void Start()
        {
            // 버튼이 할당되지 않은 경우, 자동으로 현재 게임 오브젝트에서 가져옴
            if (addButton == null)
            {
                addButton = GetComponent<Button>();
            }

            // 현재 게임 오브젝트에서 CardUI 컴포넌트를 가져옴
            cardUI = GetComponent<CardUI>();
            if (cardUI != null)
            {
                card = cardUI.card; // CardUI의 카드 정보를 가져옴
            }
            else
            {
                Debug.LogError("CardUI 컴포넌트를 찾을 수 없습니다.");
            }

            // 버튼이 유효할 경우 클릭 이벤트에 메서드 연결
            if (addButton != null)
            {
                addButton.onClick.AddListener(AddCardToBattleDeck);
            }
            else
            {
                Debug.LogError("AddButton이 할당되지 않았습니다.");
            }

            // PlayerStatsUI 컴포넌트 가져오기
            playerStatsUI = FindObjectOfType<PlayerStatsUI>();
            if (playerStatsUI == null)
            {
                Debug.LogError("PlayerStatsUI를 찾을 수 없습니다.");
            }
        }

        // 버튼을 클릭했을 때 호출될 메서드
        private void AddCardToBattleDeck()
        {
            if (GameManager.Instance != null && card != null)
            {
                // playerBattleDeck이 15장 미만인지 확인
                if (GameManager.Instance.playerBattleDeck.Count < 15)
                {
                    playerStatsUI?.ShowMessage("더이상 카드를 제거할 수 없습니다.\n카드덱에는 최소 15장의 카드가 있어야 합니다.");
                    return; // 카드 추가를 중단
                }

                // playerDeck의 카드 수가 30장을 초과하는지 확인
                if (GameManager.Instance.playerDeck.Count >= 30)
                {
                    playerStatsUI?.ShowMessage("더이상 카드를 추가할 수 없습니다.\n플레이어덱은 30장을 초과할 수 없습니다.");
                    return; // 카드 추가를 중단
                }

                // 카드 추가 로직
                GameManager.Instance.playerDeck.Add(card);
                Debug.Log($"{card.cardTitle} has been added to the playerDeck.");

                // playerBattleDeck에서 해당 카드 제거
                if (GameManager.Instance.playerBattleDeck.Contains(card))
                {
                    GameManager.Instance.playerBattleDeck.Remove(card);
                    Debug.Log($"{card.cardTitle} has been removed from the playerBattleDeck.");
                }

                // 추가 후 카드 오브젝트를 비활성화
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("GameManager 또는 카드 정보가 없습니다.");
            }
        }
    }
}
