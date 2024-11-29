using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 씬 로드를 위한 네임스페이스 추가

namespace TJ
{
    public class Ending : MonoBehaviour
    {
        public Button ending1Button;
        public Button ending2Button;
        public Button ending3Button;
        public Button ending4Button;

        private GameManager gameManager;

        private void Start()
        {
            // GameManager 인스턴스 가져오기
            gameManager = FindObjectOfType<GameManager>();

            if (gameManager == null)
            {
                Debug.LogError("GameManager를 찾을 수 없습니다!");
                return;
            }

            // currentTendencyValue에 따라 버튼 활성화
            UpdateEndingButtons();

            // 각 버튼에 클릭 이벤트 추가
            ending1Button.onClick.AddListener(() => LoadEndingScene("Ending1"));
            ending2Button.onClick.AddListener(() => LoadEndingScene("Ending2"));
            ending3Button.onClick.AddListener(() => LoadEndingScene("Ending3"));
            ending4Button.onClick.AddListener(() => LoadEndingScene("Ending4"));
        }

        private void UpdateEndingButtons()
        {
            int currentTendencyValue = gameManager.currentTendencyValue;

            // 각 엔딩 조건에 따라 버튼 활성화
            ending1Button.gameObject.SetActive(currentTendencyValue >= 11 && currentTendencyValue <= 100);
            ending2Button.gameObject.SetActive(currentTendencyValue >= 50 && currentTendencyValue <= 100);
            ending3Button.gameObject.SetActive(currentTendencyValue <= 10);
            ending4Button.gameObject.SetActive(currentTendencyValue >= 11 && currentTendencyValue <= 100);
        }

        private void LoadEndingScene(string sceneName)
        {
            // 해당 엔딩 씬 로드
            SceneManager.LoadScene(sceneName);
        }
    }
}
