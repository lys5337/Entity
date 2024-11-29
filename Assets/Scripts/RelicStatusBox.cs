using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 사용

namespace TJ
{
    public class RelicStatusBox : MonoBehaviour
    {
        public Transform relicIconContainer; // 유물 아이콘들을 담을 부모 오브젝트
        public GameObject relicIconPrefab;   // 유물 아이콘을 표시할 프리팹
        public Button openButton;            // 열기 버튼
        public Button closeButton;           // 닫기 버튼
        public GameObject relicPanel;        // 유물 아이콘이 표시되는 패널

        // 설명 패널 관련 UI 요소
        public GameObject descriptionPanel;  // 설명 패널
        public Image descriptionIcon;        // 설명 패널에 표시할 아이콘
        public TextMeshProUGUI descriptionName; // 설명 패널에 표시할 이름 (TextMeshPro)
        public TextMeshProUGUI descriptionText; // 설명 패널에 표시할 설명 (TextMeshPro)
        public TextMeshProUGUI storyText;    // 설명 패널에 표시할 스토리 (TextMeshPro)
        public Button descriptionCloseButton; // 설명 패널 닫기 버튼

        private void Start()
        {
            // 초기 상태: 패널 비활성화
            relicPanel.SetActive(false);
            descriptionPanel.SetActive(false); // 설명 패널도 비활성화

            // 버튼 이벤트 연결
            if (openButton != null)
            {
                openButton.onClick.AddListener(DisplayRelicIcons);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(HideRelicPanel);
            }

            if (descriptionCloseButton != null)
            {
                descriptionCloseButton.onClick.AddListener(HideDescriptionPanel);
            }
        }

        private void DisplayRelicIcons()
        {
            // 패널 활성화
            relicPanel.SetActive(true);

            // 이전에 생성된 아이콘 제거하여 초기화
            foreach (Transform child in relicIconContainer)
            {
                Destroy(child.gameObject);
            }

            // GameManager의 relics 리스트를 가져와서 아이콘만 표시
            List<Relic> relics = GameManager.Instance.relics;
            Debug.Log($"Relics Count: {relics.Count}");

            foreach (Relic relic in relics)
            {
                if (relic != null && relic.relicIcon != null)
                {
                    // 프리팹을 생성하고 RelicIcon 스크립트를 사용하여 아이콘 설정
                    GameObject icon = Instantiate(relicIconPrefab, relicIconContainer);
                    RelicIcon relicIconScript = icon.GetComponent<RelicIcon>();
                    if (relicIconScript != null)
                    {
                        relicIconScript.Initialize(relic.relicIcon, relic.relicName, relic.relicDescription, relic.relicStory, this);
                    }
                    else
                    {
                        Debug.LogError("RelicIcon script not found on RelicIconPrefab.");
                    }
                }
                else
                {
                    Debug.LogWarning("Relic or Relic Icon is null.");
                }
            }
        }

        // 설명 패널에 유물 정보를 표시하는 메서드
        public void DisplayRelicDescription(Sprite icon, string name, string description, string story)
        {
            if (descriptionPanel != null)
            {
                descriptionIcon.sprite = icon;
                descriptionName.text = name;
                descriptionText.text = description;
                storyText.text = story;
                descriptionPanel.SetActive(true); // 설명 패널 활성화
            }
        }

        private void HideDescriptionPanel()
        {
            if (descriptionPanel != null)
            {
                descriptionPanel.SetActive(false); // 설명 패널 비활성화
            }
        }

        private void HideRelicPanel()
        {
            // 패널 비활성화
            relicPanel.SetActive(false);
        }
    }
}
