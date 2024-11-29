using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // 씬 관리에 필요한 네임스페이스

namespace TJ
{
    public class ScriptOnOff : MonoBehaviour
    {
        public List<GameObject> panels; // 기본 패널 리스트 (디폴트 값)
        public List<GameObject> archerPanels; // 궁수 관련 패널 리스트
        public List<GameObject> magePanels; // 마법사 관련 패널 리스트

        public Button nextButton; // Next 버튼
        public GameObject continuebtn; // 마지막에 표시될 버튼

        private List<GameObject> activePanels = new List<GameObject>(); // 현재 활성화된 패널 리스트
        private int currentPanelIndex = 0; // 현재 활성화할 패널의 인덱스

        private void OnEnable()
        {
            // 씬이 로드될 때 이벤트 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            // 씬이 언로드될 때 이벤트 해제
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // 씬이 로드될 때 호출되는 메서드
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬이 로드될 때 GameManager의 partner 값을 가져와 설정
            string selectedCompanion = GetSelectedCompanion();
            SetActivePanels(selectedCompanion);

            // 패널 초기화
            currentPanelIndex = 0;
            if (activePanels.Count > 0)
            {
                activePanels[currentPanelIndex].SetActive(true);
            }
        }

        private void Start()
        {
            // 모든 패널을 비활성화한 상태로 시작
            DisableAllPanels();

            continuebtn.SetActive(false); // Continue 버튼 비활성화

            // 초기 설정
            string selectedCompanion = GetSelectedCompanion();
            SetActivePanels(selectedCompanion);

            // 첫 번째 패널을 활성화
            if (activePanels.Count > 0)
            {
                activePanels[currentPanelIndex].SetActive(true);
            }

            // 버튼 클릭 시 ActivateNextPanel 함수 실행
            if (nextButton != null)
            {
                nextButton.onClick.AddListener(ActivateNextPanel);
            }
        }

        // 모든 패널을 비활성화하는 함수
        private void DisableAllPanels()
        {
            foreach (GameObject panel in panels) panel.SetActive(false);
            foreach (GameObject panel in archerPanels) panel.SetActive(false);
            foreach (GameObject panel in magePanels) panel.SetActive(false);
        }

        // 선택한 동료에 따라 활성화할 패널 리스트 설정
        private void SetActivePanels(string companionName)
        {
            switch (companionName)
            {
                case "Archer":
                    activePanels = archerPanels;
                    break;
                case "Mage":
                    activePanels = magePanels;
                    break;
                default:
                    activePanels = panels; // 디폴트 패널 리스트
                    break;
            }
        }

        // GameData 오브젝트에서 GameManager의 partner 값을 가져오는 함수
        private string GetSelectedCompanion()
        {
            GameManager gameManager = GameObject.Find("GameData")?.GetComponent<GameManager>();
            if (gameManager != null)
            {
                return gameManager.partner; // GameManager에서 선택된 동료의 이름 반환
            }
            else
            {
                Debug.LogError("GameManager를 찾을 수 없습니다.");
                return "Default"; // 오류가 발생한 경우 디폴트 값 반환
            }
        }

        // 패널을 순서대로 활성화하는 함수
        public void ActivateNextPanel()
        {
            // 현재 패널 비활성화
            if (currentPanelIndex < activePanels.Count)
            {
                activePanels[currentPanelIndex].SetActive(false); // 현재 패널 비활성화
            }

            currentPanelIndex++; // 다음 패널로 이동

            // 다음 패널을 활성화
            if (currentPanelIndex < activePanels.Count)
            {
                activePanels[currentPanelIndex].SetActive(true); // 다음 패널 활성화
            }

            // 모든 패널이 활성화된 후에는 Next 버튼 비활성화 및 Continue 버튼 활성화
            if (currentPanelIndex >= activePanels.Count)
            {
                nextButton.gameObject.SetActive(false); // Next 버튼 비활성화
                continuebtn.gameObject.SetActive(true); // Continue 버튼 활성화
            }
        }
    }
}
