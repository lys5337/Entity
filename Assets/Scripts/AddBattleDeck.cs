using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class AddBattleDeck : MonoBehaviour
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
            // GameManager의 playerBattleDeck에 카드 추가
            if (GameManager.Instance != null && card != null)
            {
                // BattleDeck의 카드 수가 20장 이하일 때만 추가
                if (GameManager.Instance.playerBattleDeck.Count < 20)
                {
                    GameManager.Instance.playerBattleDeck.Add(card);
                    Debug.Log($"{card.cardTitle}이(가) 전투 덱에 추가되었습니다.");

                    // playerDeck에서 해당 카드 제거
                    if (GameManager.Instance.playerDeck.Contains(card))
                    {
                        GameManager.Instance.playerDeck.Remove(card);
                        Debug.Log($"{card.cardTitle}이(가) 플레이어 덱에서 제거되었습니다.");
                    }

                    // 추가 후 카드 오브젝트를 비활성화하거나 제거할 수도 있음
                    gameObject.SetActive(false);
                }
                else
                {
                    // BattleDeck에 카드가 20장 이상일 때 메시지 표시
                    playerStatsUI?.ShowMessage("더 이상 카드를 추가할 수 없습니다.\n전투 덱은 20장까지 추가할 수 있습니다.");
                }
            }
            else
            {
                Debug.LogError("GameManager 또는 카드 정보가 없습니다.");
            }
        }
    }
}
