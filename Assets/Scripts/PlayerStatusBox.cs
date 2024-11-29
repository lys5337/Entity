using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TJ
{
    public class PlayerStatusBox : MonoBehaviour
    {
        private GameManager gameManager;
        private Fighter player;

        public TMP_Text healthDisplayText; // Health 표시용 TextMeshPro 텍스트
        public TMP_Text moneyAmountText;   // Money 표시용 TextMeshPro 텍스트
        public TMP_Text playerNameText;    // 플레이어 이름을 표시할 TextMeshPro 텍스트
        public Transform relicParent;
        public GameObject relicPrefab;
        public GameObject playerStatusBoxObject;
        public GameObject tooltipObject; // 툴팁 오브젝트 (비활성화 상태로 시작)
        public TMP_Text tooltipText;     // 툴팁 설명을 표시할 TextMeshPro 텍스트
        
        public TMP_Text powerText;
        public TMP_Text defenseText;
        public TMP_Text luckText;
        

        private void awake()
        {
            DisplayHealth();
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerStatusBox = this;
                DisplayPlayerName();
                
            }
            else
            {
                Debug.LogError("GameManager 인스턴스가 설정되지 않았습니다. PlayerStatsUI가 제대로 설정되지 않았습니다.");
            }

            if (tooltipObject != null)
                tooltipObject.SetActive(false);

            if (playerStatusBoxObject != null)
                playerStatusBoxObject.SetActive(false);
        }

        public void DisplayPlayerName()
        {
            if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.playerName))
            {
                playerNameText.text = GameManager.Instance.playerName;
            }
            else
            {
                playerNameText.text = "Unknown";
                Debug.LogWarning("플레이어 이름이 설정되지 않았습니다.");
            }
        }

        public void UpdatePlayerStats(int power, int defense, int luck)
        {
            powerText.text = $"힘: {power}";
            defenseText.text = $"수비: {defense}";
            luckText.text = $"운: {luck}";

        }


        public void DisplayHealth()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
        }
        
        public void DisplayRelics()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager 인스턴스가 null입니다. 유물 정보를 표시할 수 없습니다.");
                return;
            }

            if (playerStatusBoxObject == null)
            {
                Debug.LogError("playerStatusBoxObject가 설정되지 않았습니다.");
                return;
            }

            if (GameManager.Instance.relics == null || GameManager.Instance.relics.Count == 0)
            {
                Debug.LogWarning("유물 리스트가 비어 있거나 null입니다.");
                return;
            }

            foreach (Transform c in relicParent)
                Destroy(c.gameObject);

            foreach (Relic r in GameManager.Instance.relics)
            {
                if (r != null)
                {
                    GameObject relicObject = Instantiate(relicPrefab, relicParent);
                    relicObject.GetComponent<UnityEngine.UI.Image>().sprite = r.relicIcon;

                    RelicScript relicScript = relicObject.GetComponent<RelicScript>();
                    if (relicScript != null)
                    {
                        relicScript.SetRelicData(r);
                        relicScript.tooltipText = tooltipText;
                        relicScript.tooltipObject = tooltipObject;
                    }
                }
                else
                {
                    Debug.LogWarning("유물 리스트에 null 값이 포함되어 있습니다.");
                }
            }
        }
    }
}
