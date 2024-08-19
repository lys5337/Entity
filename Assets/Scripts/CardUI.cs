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

        // CardManagementUI�� �ν����Ϳ��� ���� �Ҵ��� �� �ֵ��� public���� ����
        public CardManagementUI cardManagementUI;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            animator = GetComponent<Animator>();

            // ���� �ν����Ϳ��� �Ҵ���� �ʾҴٸ� �ڵ����� ã�� �ڵ� �߰�
            if (cardManagementUI == null)
            {
                cardManagementUI = FindObjectOfType<CardManagementUI>();
                if (cardManagementUI == null)
                {
                    Debug.LogError("CardManagementUI�� ã�� �� �����ϴ�. �ν����Ϳ��� �Ҵ����ּ���.");
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
                cardManagementUI.gameObject.SetActive(true); // ī�� ���� UI �г� Ȱ��ȭ
            }
            else
            {
                Debug.LogError("CardManagementUI�� ã�� �� �����ϴ�.");
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
            // �巡�� ������ �ʿ��� ��� ���⿡ �߰�
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
