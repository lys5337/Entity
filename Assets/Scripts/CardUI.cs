using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TJ
{
    public class CardUI : MonoBehaviour
    {
        public Card card;
        public TMP_Text cardTitleText;
        public TMP_Text cardDescriptionText;
        public TMP_Text cardCostText;
        public Image cardImage;
        public GameObject discardEffect;
        BattleSceneManager battleSceneManager;
        Animator animator;

        // CardManagementUI를 인스펙터에서 직접 할당할 수 있도록 public으로 선언
        public CardManagementUI cardManagementUI;

        // 카드를 원래 위치로 복구하는데 필요한 변수
        private Vector3 originalPosition;
        private RectTransform rectTransform;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            animator = GetComponent<Animator>();

            if (cardManagementUI == null)
            {
                cardManagementUI = FindObjectOfType<CardManagementUI>();
            }

            rectTransform = GetComponent<RectTransform>();
            originalPosition = rectTransform.position; // 카드의 초기 위치 저장
        }

        private void OnEnable()
        {
            animator.Play("HoverOffCard");
        }

        public void LoadCard(Card _card)
        {
            if (_card == null)
            {
                Debug.LogError("카드가 null입니다. LoadCard를 실행할 수 없습니다.");
                return;
            }

            // 카드 데이터 설정
            card = _card;

            // UI 컴포넌트 null 확인
            if (cardTitleText == null || cardDescriptionText == null || cardCostText == null || cardImage == null)
            {
                Debug.LogError("카드 UI 컴포넌트가 설정되지 않았습니다. LoadCard를 실행할 수 없습니다.");
                return;
            }

            // 카드 정보를 UI에 로드
            cardTitleText.text = card.cardTitle;
            cardDescriptionText.text = card.GetCardDescriptionAmount();
            cardCostText.text = card.GetCardCostAmount().ToString();
            cardImage.sprite = card.cardIcon;
        }


        public void SelectCard()
        {
            battleSceneManager.selectedCard = this;

            if (cardManagementUI != null)
            {
                cardManagementUI.gameObject.SetActive(true); // 카드 관리 UI 패널 활성화
            }
        }

        public void DeselectCard()
        {
            battleSceneManager.selectedCard = null;
            animator.Play("HoverOffCard");
        }

        public void HoverCard()
        {
            if (battleSceneManager.selectedCard == null)
                animator.Play("HoverOnCard");
        }

        public void DropCard()
        {
            if (battleSceneManager.selectedCard == null)
                animator.Play("HoverOffCard");
        }

        public void HandleDrag()
        {
            // 드래그 로직이 필요할 경우 여기에 추가
        }

        public void HandleEndDrag()
        {
            // 에너지가 부족하면 카드 사용 불가
            if (battleSceneManager.energy < card.GetCardCostAmount())
            {
                Debug.Log("에너지가 부족합니다. 카드를 사용할 수 없습니다.");
                ResetCardPosition();  // 카드 원래 위치로 복귀
                return;
            }

            // 타겟이 없으면 공격 카드를 사용할 수 없게 설정
            if (card.cardType == Card.CardType.Attack && battleSceneManager.cardTarget == null)
            {
                Debug.LogError("타겟이 설정되지 않았습니다. 공격 카드를 사용할 수 없습니다.");
                ResetCardPosition();  // 카드 원래 위치로 복귀
                return;
            }

            // 카드 타입에 따른 처리
            if (card.cardType == Card.CardType.Attack)
            {
                // 공격 카드 처리
                Debug.Log("공격 카드 사용: " + card.cardTitle);
                battleSceneManager.PlayCard(this);
            }
            else if (card.cardType == Card.CardType.Skill)
            {
                // 스킬 카드 처리
                Debug.Log("스킬 카드 사용: " + card.cardTitle);
                battleSceneManager.PlayCard(this);
            }
            else if (card.cardType == Card.CardType.Power)
            {
                // 파워 카드 처리
                Debug.Log("파워 카드 사용: " + card.cardTitle);
                battleSceneManager.PlayCard(this);
            }

            animator.Play("HoverOffCard");
        }


        // 카드를 원래 위치로 되돌리는 메서드
        public void ResetCardPosition()
        {
            rectTransform.position = originalPosition;
        }
    }
}
