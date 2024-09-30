using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class SaveLoadUI : MonoBehaviour
    {
        public Button saveSlot1Button;
        public Button saveSlot2Button;
        public Button saveSlot3Button;
        public Button loadSlot1Button;
        public Button loadSlot2Button;
        public Button loadSlot3Button;

        public GameManager gameManager;
        public Fighter fighter;
        public SaveDataManager saveDataManager;
        public LoadDataManager loadDataManager;

        private void Start()
        {
            saveSlot1Button.onClick.AddListener(() => saveDataManager.SaveGame(1));
            saveSlot2Button.onClick.AddListener(() => saveDataManager.SaveGame(2));
            saveSlot3Button.onClick.AddListener(() => saveDataManager.SaveGame(3));

            loadSlot1Button.onClick.AddListener(() => loadDataManager.LoadGame(1));
            loadSlot2Button.onClick.AddListener(() => loadDataManager.LoadGame(2));
            loadSlot3Button.onClick.AddListener(() => loadDataManager.LoadGame(3));
        }
    }
}