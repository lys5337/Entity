using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class LoadLoad : MonoBehaviour
    {
        public Button loadSlot1Button;
        public Button loadSlot2Button;
        public Button loadSlot3Button;

        public TextMeshProUGUI loadSlot1Text;
        public TextMeshProUGUI loadSlot2Text;
        public TextMeshProUGUI loadSlot3Text;

        public SaveDataManager saveDataManager;
        public LoadDataManager loadDataManager;

        private void Start()
        {
            UpdateSlotTexts();

            loadSlot1Button.onClick.AddListener(() => loadDataManager.LoadGame(1));
            loadSlot2Button.onClick.AddListener(() => loadDataManager.LoadGame(2));
            loadSlot3Button.onClick.AddListener(() => loadDataManager.LoadGame(3));
        }

        private void SaveAndUpdateButtonText(int slot, TextMeshProUGUI saveButtonText, TextMeshProUGUI loadButtonText)
        {
            saveDataManager.SaveGame(slot);

            // GameManager 인스턴스를 통해 playerName과 씬 이름 가져오기
            string playerName = saveDataManager.gameManager != null ? saveDataManager.gameManager.playerName : "UnknownPlayer";
            string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            string displayText = $"{playerName} - {currentSceneName}";

            if (saveButtonText != null && loadButtonText != null)
            {
                saveButtonText.text = displayText;
                loadButtonText.text = displayText;
            }
            else
            {
                Debug.LogWarning("할당된 TextMeshProUGUI 컴포넌트가 없습니다.");
            }
        }

        private void UpdateSlotTexts()
        {
            loadSlot1Text.text = saveDataManager.GetSavedSceneName(1);
            loadSlot2Text.text = saveDataManager.GetSavedSceneName(2);
            loadSlot3Text.text = saveDataManager.GetSavedSceneName(3);
        }
    }
}
