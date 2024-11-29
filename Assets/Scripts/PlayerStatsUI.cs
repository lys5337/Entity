using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 사용을 위한 네임스페이스

namespace TJ
{
    public class PlayerStatsUI : MonoBehaviour
    {
        private GameManager gameManager;
        private Fighter player;

        public TMP_Text healthDisplayText; // Health 표시용 TextMeshPro 텍스트
        public TMP_Text moneyAmountText;   // Money 표시용 TextMeshPro 텍스트
        public TMP_Text floorText;         // Floor 표시용 TextMeshPro 텍스트
        public TMP_Text playerNameText;    // 플레이어 이름을 표시할 TextMeshPro 텍스트
        public Transform relicParent;
        public GameObject relicPrefab;
        public GameObject playerStatsUIObject;
        public GameObject tooltipObject; // 툴팁 오브젝트 (비활성화 상태로 시작)
        public TMP_Text tooltipText;     // 툴팁 설명을 표시할 TextMeshPro 텍스트

        public GameObject messagePanel;  // 메시지 패널
        public TMP_Text messageText;     // 메시지를 표시할 TextMeshPro 텍스트

        private void awake()
        {
            DisplayHealth();
        }

        private void Start()
        {
            // GameManager.Instance가 null인지 확인
            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerStatsUI = this;
                DisplayPlayerName(); // 플레이어 이름을 표시
                
            }
            else
            {
                Debug.LogError("GameManager 인스턴스가 설정되지 않았습니다. PlayerStatsUI가 제대로 설정되지 않았습니다.");
            }

            // 툴팁을 처음에는 숨김 상태로 설정
            if (tooltipObject != null)
                tooltipObject.SetActive(false);

            // 메시지 패널과 텍스트를 숨김 상태로 설정
            if (messagePanel != null)
                messagePanel.SetActive(false);
            if (messageText != null)
                messageText.gameObject.SetActive(false);
        }

        // 플레이어 이름을 표시하는 함수
        public void DisplayPlayerName()
        {
            if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.playerName))
            {
                playerNameText.text = GameManager.Instance.playerName; // GameManager에서 플레이어 이름을 가져와 표시
            }
            else
            {
                playerNameText.text = "Unknown"; // 기본값 설정
            }
        }


        public void DisplayHealth()
        {
            gameManager = FindObjectOfType<GameManager>();
            gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
        }
        
        public void DisplayRelics()
        {
            // GameManager.Instance가 null인지 확인
            if (GameManager.Instance == null)
            {
                Debug.LogError("GameManager 인스턴스가 null입니다. 유물 정보를 표시할 수 없습니다.");
                return;
            }

            // playerStatsUIObject가 null인지 확인
            if (playerStatsUIObject == null)
            {
                return;
            }

            // 유물 리스트가 null인지 확인
            if (GameManager.Instance.relics == null || GameManager.Instance.relics.Count == 0)
            {
                return;
            }

            // 기존 자식 객체 제거
            foreach (Transform c in relicParent)
                Destroy(c.gameObject);

            // 유물 리스트 순회하며 UI에 표시
            foreach (Relic r in GameManager.Instance.relics)
            {
                if (r != null)
                {
                    GameObject relicObject = Instantiate(relicPrefab, relicParent);
                    relicObject.GetComponent<UnityEngine.UI.Image>().sprite = r.relicIcon;

                    // RelicScript 스크립트를 찾아서 유물 정보 할당 및 툴팁 설정
                    RelicScript relicScript = relicObject.GetComponent<RelicScript>();
                    if (relicScript != null)
                    {
                        relicScript.SetRelicData(r);              // 유물 데이터 할당
                        relicScript.tooltipText = tooltipText;     // 툴팁 텍스트 연결
                        relicScript.tooltipObject = tooltipObject; // 툴팁 오브젝트 연결
                    }
                }
                else
                {
                    Debug.LogWarning("유물 리스트에 null 값이 포함되어 있습니다.");
                }
            }

            // 메시지를 표시하는 메서드
            
        }

        public void ShowMessage(string message)
        {
            if (messageText != null && messagePanel != null)
            {
                messageText.text = message;
                messagePanel.SetActive(true); // 메시지 패널 활성화
                messageText.gameObject.SetActive(true);

                // 일정 시간 후에 메시지를 비활성화할 수 있도록 코루틴 사용
                StartCoroutine(HideMessageAfterDelay(3f));
            }
        }

        // 일정 시간 후에 메시지를 숨기는 코루틴
        private IEnumerator HideMessageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (messagePanel != null)
                messagePanel.SetActive(false); // 메시지 패널 비활성화
            if (messageText != null)
                messageText.gameObject.SetActive(false);
        }
    }
}
