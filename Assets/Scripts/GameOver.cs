using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TJ
{
    public class GameOver : MonoBehaviour
    {
        // 버튼 컴포넌트
        public Button continueButton;
        public Button titleScreenButton;

        private GameManager gameManager;

        private void Awake()
        {
            // GameManager 인스턴스 가져오기
            gameManager = FindObjectOfType<GameManager>();

            // 버튼에 메서드 할당
            if (continueButton != null)
                continueButton.onClick.AddListener(OnContinueButton);

            if (titleScreenButton != null)
                titleScreenButton.onClick.AddListener(OnTitleScreenButton);
        }

        // 이어하기 버튼 메서드
        public void OnContinueButton()
        {
            // 이어하기 버튼을 눌렀을 때의 동작
            Debug.Log("이어하기 버튼이 눌렸습니다. 여기에서 이어하기 로직을 추가하세요.");
        }

        // 타이틀 화면으로 돌아가는 버튼 메서드
        public void OnTitleScreenButton()
        {
            // GameData 파괴
            DestroyGameData();

            // "Main" 씬을 로드해서 타이틀 화면으로 돌아가기
            SceneManager.LoadScene("Main");
            Debug.Log("타이틀 화면으로 돌아갑니다.");
        }

        // GameData 파괴 메서드
        private void DestroyGameData()
        {
            if (gameManager != null)
            {
                Destroy(gameManager.gameObject); // GameManager가 속한 GameObject 파괴
                Debug.Log("GameData가 파괴되었습니다.");
            }
        }
    }
}
