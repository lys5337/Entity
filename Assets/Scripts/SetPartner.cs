using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 관리에 필요한 네임스페이스

namespace TJ
{
    public class SetPartner : MonoBehaviour
    {
        public Button archerButton; // 궁수 선택 버튼
        public Button mageButton; // 마법사 선택 버튼

        public Relic acherRelic;
        public Relic mageRelic;

        private GameManager gameManager; // GameManager 참조

        private void OnEnable()
        {
            // 활성화될 때 GameManager를 찾고 버튼 이벤트를 할당
            gameManager = GameObject.Find("GameData")?.GetComponent<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager를 찾을 수 없습니다.");
                return;
            }

            // 버튼 클릭 이벤트 할당
            AssignButtonActions();
        }

        // 버튼 클릭 이벤트를 할당하는 메서드
        private void AssignButtonActions()
        {
            if (archerButton != null)
            {
                
                archerButton.onClick.RemoveAllListeners(); // 기존 이벤트 제거
                archerButton.onClick.AddListener(() => SetPartnerType("Archer"));
            }

            if (mageButton != null)
            {
                
                mageButton.onClick.RemoveAllListeners(); // 기존 이벤트 제거
                mageButton.onClick.AddListener(() => SetPartnerType("Mage"));
            }
        }

        public void SetArcherRelic()
        {
            gameManager.relics.Add(acherRelic);
        }

        public void SetMageRelic()
        {
            gameManager.relics.Add(mageRelic);
        }


        // 선택된 파트너 타입을 설정하는 메서드
        private void SetPartnerType(string partnerType)
        {
            if (gameManager != null)
            {
                gameManager.partner = partnerType; // GameManager의 partner 변수 설정
                Debug.Log($"{partnerType}가 선택되었습니다.");
            }
            else
            {
                Debug.LogError("GameManager가 설정되지 않았습니다.");
            }
        }
    }
}
