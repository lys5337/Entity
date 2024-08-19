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

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            animator = GetComponent<Animator>();

            // 만약 인스펙터에서 할당되지 않았다면 자동으로 찾는 코드 추가
            if (cardManagementUI == null)
            {
                cardManagementUI = FindObjectOfType<CardManagementUI>();
                if (cardManagementUI == null)
                {
                    Debug.LogError("CardManagementUI를 찾을 수 없습니다. 인스펙터에서 할당해주세요.");
                }
            }
        }

        private void OnEnable()
        {
            animator.Play("HoverOffCard");
        }

        public void LoadCard(Card _card)
        {
            card = _card;
            gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
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
            else
            {
                Debug.LogError("CardManagementUI를 찾을 수 없습니다.");
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
            if (battleSceneManager.energy < card.GetCardCostAmount())
                return;

            if (card.cardType == Card.CardType.Attack)
            {
                battleSceneManager.PlayCard(this);
                animator.Play("HoverOffCard");
            }
            else if (card.cardType != Card.CardType.Attack)
            {
                animator.Play("HoverOffCard");
                battleSceneManager.PlayCard(this);
            }
        }
    }
}
