using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TJ
{
    public class CardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Card card; // 카드 데이터
        public TMP_Text tooltipText; // 툴팁에 표시할 TextMeshPro 텍스트
        public GameObject tooltipObject; // 툴팁 오브젝트
        public Fighter player;
        public Fighter fighter;

        public GameObject common;
        public GameObject uncommon;
        public GameObject rare;
        public GameObject epic;
        public GameObject legendary;
        public GameObject hidden;

        public TMP_Text cardTitleText;
        public TMP_Text cardDescriptionText;
        public TMP_Text cardCostText;
        public Image cardImage;
        public GameObject discardEffect;
        BattleSceneManager battleSceneManager;
        GameManager gameManager;
        Animator animator;

        // CardManagementUI를 인스펙터에서 직접 할당할 수 있도록 public으로 선언
        public CardManagementUI cardManagementUI;

        // 카드를 원래 위치로 복구하는데 필요한 변수
        private Vector3 originalPosition;
        private RectTransform rectTransform;

        private DragPathRenderer dragPathRenderer;
        private CursorManager cursorManager;

        

        

        private Vector3 dragOffset;  // 드래그 시 카드의 중심을 조정하기 위한 오프셋

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            animator = GetComponent<Animator>();
            fighter = FindObjectOfType<Fighter>();

            if (cardManagementUI == null)
            {
                cardManagementUI = FindObjectOfType<CardManagementUI>();
            }

            rectTransform = GetComponent<RectTransform>();
            originalPosition = rectTransform.position; // 카드의 초기 위치 저장

            player = battleSceneManager.player;
        }

        private void Start()
        {
            dragPathRenderer = FindObjectOfType<DragPathRenderer>();
            cursorManager = FindObjectOfType<CursorManager>();
            dragOffset = new Vector3(0, rectTransform.rect.height / 2, 0);
            LoadCardRarity();

            if (tooltipObject != null)
            {
                tooltipObject.SetActive(false);
            }
        }

        private void Update()
        {
            LoadCardRarity();
        }

        public void LoadCardRarity()
        {
            if (card.cardRarity == Card.CardRarity.Common)
            {
                common.SetActive(true);
                uncommon.SetActive(false);
                rare.SetActive(false);
                epic.SetActive(false);
                legendary.SetActive(false);
                hidden.SetActive(false);
            }
            else if (card.cardRarity == Card.CardRarity.Uncommon)
            {
                common.SetActive(false);
                uncommon.SetActive(true);
                rare.SetActive(false);
                epic.SetActive(false);
                legendary.SetActive(false);
                hidden.SetActive(false);
            }
            else if (card.cardRarity == Card.CardRarity.Rare)
            {
                common.SetActive(false);
                uncommon.SetActive(false);
                rare.SetActive(true);
                epic.SetActive(false);
                legendary.SetActive(false);
                hidden.SetActive(false);
            }
            else if (card.cardRarity == Card.CardRarity.Epic)
            {
                common.SetActive(false);
                uncommon.SetActive(false);
                rare.SetActive(false);
                epic.SetActive(true);
                legendary.SetActive(false);
                hidden.SetActive(false);
            }
            else if (card.cardRarity == Card.CardRarity.Legendary)
            {
                common.SetActive(false);
                uncommon.SetActive(false);
                rare.SetActive(false);
                epic.SetActive(false);
                legendary.SetActive(true);
                hidden.SetActive(false);
            }
            else if (card.cardRarity == Card.CardRarity.Hidden_Card)
            {
                common.SetActive(false);
                uncommon.SetActive(false);
                rare.SetActive(false);
                epic.SetActive(false);
                legendary.SetActive(false);
                hidden.SetActive(true);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(dragPathRenderer != null)
                dragPathRenderer.StartDrag(transform.position + dragOffset);

            if (tooltipObject != null && card != null && card.cardType == Card.CardType.Attack)
            {
                tooltipObject.SetActive(true);
                UpdateTooltip();
            }

            if (GetCardType() == Card.CardType.Attack)
                cursorManager.SetAttackCursor();
            else if (GetCardType() == Card.CardType.Skill)
                cursorManager.SetSkillCursor();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.position = Input.mousePosition + dragOffset;

            if(dragPathRenderer != null)
                dragPathRenderer.UpdateDrag(Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10)));

            if (tooltipObject != null && card.cardType == Card.CardType.Attack)
            {
                UpdateTooltip();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(dragPathRenderer != null)
            {
                dragPathRenderer.EndDrag();
                cursorManager.SetDefaultCursor();
            }
            

            if (tooltipObject != null)
            {
                tooltipObject.SetActive(false);
            }
        }

        private Card.CardType GetCardType()
        {
            return Card.CardType.Attack;
        }

        public int GetPlayerPower()
        {
            return player.GetPlayerPower();
        }
        
        private void OnEnable()
        {
            animator.Play("HoverOffCard");
        }


        private void UpdateTooltip()
        {
            if (card.cardType == Card.CardType.Attack)
            {
                int baseDamage = card.GetCardEffectAmount();
                int strengthBonus = player.GetPlayerPower();
                int buffBonus = player.strength.buffValue;
                int totalDamage = baseDamage + strengthBonus + buffBonus;
                int critChance = player.ReturnCriticalPer();

                List<float> damageModifiers = new List<float>();

                if (player.weak.buffValue > 0){
                    damageModifiers.Add(0.7f);
                    Debug.Log("플레이어가 약화 상태입니다.");
                }
                
                if(player.insanity.buffValue > 0){
                    damageModifiers.Add(1.3f);
                    Debug.Log("플레이어가 광기 상태입니다.");
                }

                if (fighter.vulnerable.buffValue > 0){
                    damageModifiers.Add(1.3f);
                    Debug.Log("적이 취약 상태입니다.");
                }

                if(gameManager.PlayerHasRelic("3가지 보석이 박힌 왕관")){
                    damageModifiers.Add(1.2f);
                    Debug.Log("플레이어가 3가지 보석이 박힌 왕관을 가지고 있습니다.");
                }
                else
                {
                    if(gameManager.PlayerHasRelic("하얀 보석이 박힌 단검") && battleSceneManager.normalFight)
                    {
                        damageModifiers.Add(1.1f);
                    }

                    if(gameManager.PlayerHasRelic("푸른 보석이 박힌 반지") && battleSceneManager.eliteFight)
                    {
                        damageModifiers.Add(1.1f);
                    }

                    if(gameManager.PlayerHasRelic("붉은 보석이 박힌 귀걸이") && battleSceneManager.bossFight)
                    {
                        damageModifiers.Add(1.1f);
                    }
                }

                foreach (float modifier in damageModifiers)
                {
                    totalDamage = (int)(totalDamage * modifier);
                }

                int finalDamage = totalDamage;
                
                if(gameManager.PlayerHasRelic("용 조각상")) finalDamage = (int)(totalDamage*2f);
                else if(gameManager.PlayerHasRelic("달인의 반지")) finalDamage = (int)(totalDamage*1.75f);
                else finalDamage = (int)(totalDamage*1.5f);
                
                tooltipText.text = $"기본 피해량: {baseDamage} 파워: {strengthBonus} 힘 버프: {buffBonus}\n" +
                                $"크리티컬 확률 현재 : {critChance}%\n"+
                                $"총: {totalDamage}의 피해 (크리티컬시: {(int)(finalDamage)})";
            }
        }



        public void LoadCard(Card _card)
        {
            if (_card == null)
            {
                Debug.LogError("LoadCard 호출 시 카드 데이터가 null입니다.");
                return;
            }

            card = _card;

            if (cardTitleText != null)
                cardTitleText.text = card.cardTitle;

            if (cardDescriptionText != null)
                cardDescriptionText.text = card.GetCardDescriptionAmount();

            if (cardCostText != null)
                cardCostText.text = card.GetCardCostAmount().ToString();

            if (cardImage != null)
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
                Debug.LogWarning("타겟이 설정되지 않았습니다. 공격 카드를 사용할 수 없습니다.");
                ResetCardPosition();  // 카드 원래 위치로 복귀
                return;
            }

            if (card.cardType == Card.CardType.Skill)
            {
                if (Input.mousePosition.y < rectTransform.position.y + 850)
                {
                    Debug.LogWarning("카드가 충분히 높이 드래그되지 않았습니다. 스킬 카드를 사용할 수 없습니다.");
                    ResetCardPosition();  // 카드 원래 위치로 복귀
                    return;
                }

                Debug.Log("스킬 카드 사용 조건 만족. 마우스 Y 위치: " + Input.mousePosition.y);
            }

            if (card.cardType == Card.CardType.Power)
            {
                if (Input.mousePosition.y < rectTransform.position.y + 850)
                {
                    Debug.LogWarning("카드가 충분히 높이 드래그되지 않았습니다. 파워 카드를 사용할 수 없습니다.");
                    ResetCardPosition();  // 카드 원래 위치로 복귀
                    return;
                }

                Debug.Log("파워 카드 사용 조건 만족. 마우스 Y 위치: " + Input.mousePosition.y);
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
