using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class BuffUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image buffImage;
        public TMP_Text buffAmountText;
        public Animator animator;

        public GameObject buffInfoPanel; // 버프 설명을 표시할 패널
        public TMP_Text buffDescriptionText; // 버프 설명을 표시할 TextMesh Pro 텍스트
        private Fighter fighter; // Fighter 컴포넌트를 참조

        private void Awake()
        {
            animator = GetComponent<Animator>();

            // Fighter 컴포넌트를 부모 오브젝트에서 찾음
            fighter = GetComponentInParent<Fighter>();
            if (fighter == null)
            {
                Debug.LogError("Fighter 컴포넌트를 찾을 수 없습니다.");
            }

            // 시작할 때 버프 설명 패널을 비활성화
            if (buffInfoPanel != null)
            {
                buffInfoPanel.SetActive(false);
            }
        }

        // 버프 표시
        public void DisplayBuff(Buff b)
        {
            // 버프 아이콘과 수치 설정
            if (b.buffIcon != null)
            {
                buffImage.sprite = b.buffIcon;
            }
            else
            {
                Debug.LogError("버프 아이콘이 설정되지 않았습니다.");
            }

            buffAmountText.text = b.buffValue.ToString();

            // Animator가 존재하는지 확인하고, 파괴되지 않았다면 애니메이션 실행
            if (animator != null && animator.gameObject != null)
            {
                animator.Play("IntentSpawn");
            }
            else
            {
                Debug.LogWarning("Animator가 파괴되었거나 존재하지 않습니다.");
            }
        }

        // 마우스를 버프 아이콘 위에 올렸을 때 설명 패널을 활성화
        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowBuffDescription();
        }

        // 마우스를 버프 아이콘에서 벗어났을 때 설명 패널을 비활성화
        public void OnPointerExit(PointerEventData eventData)
        {
            HideBuffDescription();
        }

        // 아이콘에 해당하는 타입을 찾아 설명을 패널과 텍스트로 출력
        private void ShowBuffDescription()
        {
            if (fighter == null) return;

            string description = GetDescriptionByIcon(buffImage.sprite);
            if (buffDescriptionText != null)
            {
                buffDescriptionText.text = description;
            }

            if (buffInfoPanel != null)
            {
                buffInfoPanel.SetActive(true); // 패널 활성화
            }
        }

        // 아이콘에 해당하는 타입을 찾아 설명을 반환하는 메서드
        private string GetDescriptionByIcon(Sprite icon)
        {
            if (icon == fighter.strength.buffIcon)
                return "힘 증가\n대미지가 증가합니다.";
            else if (icon == fighter.weak.buffIcon)
                return "약화\n주는 피해가 감소합니다.";
            else if (icon == fighter.vulnerable.buffIcon)
                return "취약\n받는 피해가 증가합니다.";
            else if (icon == fighter.ritual.buffIcon)
                return "의식\n턴마다 힘이 증가합니다.";
            else if (icon == fighter.enrage.buffIcon)
                return "격노\n공격할 때마다 피해가 증가합니다.";
            else if (icon == fighter.poison.buffIcon)
                return "독\n매 턴 종료 시 수치만큼 대미지를 입히고, 수치가 1 줄어듭니다.";
            else if (icon == fighter.bleeding.buffIcon)
                return "출혈\n매 턴마다 1의 피해를 입히며, 수치가 1 줄어듭니다.";
            else if (icon == fighter.deathMark.buffIcon)
                return "데스마크\n5중첩 시 체력을 1로 만듭니다.";
            else if (icon == fighter.infectedBleeding.buffIcon)
                return "감염된 출혈\n피해가 점점 더 커집니다.";
            else if (icon == fighter.excessiveBleeding.buffIcon)
                return "과다 출혈\n심한 출혈로 추가 피해를 입힙니다.";
            else if (icon == fighter.thornArmor.buffIcon)
                return "가시 갑옷\n공격받으면 수치만큼 피해를 입힙니다.";
            else if (icon == fighter.adrenaline.buffIcon)
                return "아드레날린\n카드 사용 시 힘이 증가합니다.";
            else if (icon == fighter.goddessBlessing.buffIcon)
                return "여신의 가호\n받는 피해가 90% 감소합니다.";
            else if (icon == fighter.insanity.buffIcon)
                return "광기\n주는피해와 받는피해가 30% 증가합니다.";
            else if (icon == fighter.defenseStance.buffIcon)
                return "방어 자세\n턴 종료시 방어력이 감소하지 않습니다.";
            else
                return "알 수 없는 버프입니다.";
        }

        // 버프 설명 패널을 숨기는 메서드
        private void HideBuffDescription()
        {
            if (buffInfoPanel != null)
            {
                buffInfoPanel.SetActive(false); // 패널 비활성화
            }
        }
    }
}
