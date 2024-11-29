using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace TJ
{
    public class PlayerStatus : MonoBehaviour
    {
        private Fighter player;
        private GameManager gameManager;

        public GameObject playerInfoButton;
        public GameObject playerInfoBox;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }

        public void DisplayPlayerInfo()
        {
            playerInfoBox.SetActive(!playerInfoBox.activeSelf);
            
            DisplayPlayerStats();
        }

        public void DisplayPlayerStats()
        {
            if (gameManager != null)
            {
                gameManager.DisplayPlayerStats();
            }
            else
            {
                Debug.LogError("GameManager 인스턴스가 null입니다.");
            }
            gameManager.UpdatePlayerStatsUI();
        }
    }
}
