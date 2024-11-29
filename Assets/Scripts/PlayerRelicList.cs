using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
    public class PlayerRelicList : MonoBehaviour
    {
        public GameObject relicViewPanel; // Relic 정보를 표시할 패널
        public GameObject relicPrefab; // Relic UI 프리팹
        public Transform relicContainer; // Relic 프리팹이 배치될 부모 오브젝트
        public TMP_Text relicNameText; // Relic 이름을 표시할 TextMeshPro 텍스트
        public TMP_Text relicDescriptionText; // Relic 설명을 표시할 TextMeshPro 텍스트
        public Image relicIconImage; // Relic 아이콘을 표시할 Image 컴포넌트
        public TMP_Text relicStoryText; // Relic 스토리를 표시할 TextMeshPro 텍스트

        private List<Relic> playerRelics; // 플레이어가 보유한 Relic 리스트

        private void Start()
        {
            LoadRelics();
        }

        // 플레이어의 Relic 정보를 불러와서 UI에 표시
        private void LoadRelics()
        {
            // GameManager에서 플레이어의 Relic 리스트 가져오기
            playerRelics = GameManager.Instance.relics;

            // 기존 자식 객체 제거
            foreach (Transform child in relicContainer)
            {
                Destroy(child.gameObject);
            }

            // Relic 리스트 순회하며 UI에 표시
            foreach (Relic relic in playerRelics)
            {
                GameObject relicObject = Instantiate(relicPrefab, relicContainer);
                relicObject.GetComponent<Image>().sprite = relic.relicIcon; // Relic 아이콘 설정

                // RelicScript 스크립트를 통해 클릭 이벤트 연결
                RelicScript relicScript = relicObject.GetComponent<RelicScript>();
                if (relicScript != null)
                {
                    relicScript.SetRelicData(relic); // Relic 데이터 설정
                    relicScript.OnRelicClicked += DisplayRelicDetails; // 클릭 이벤트 연결
                }
            }
        }

        // Relic 정보를 UI에 표시
        private void DisplayRelicDetails(Relic relic)
        {
            if (relic != null)
            {
                relicNameText.text = relic.relicName;
                relicDescriptionText.text = relic.relicDescription;
                relicIconImage.sprite = relic.relicIcon;
                relicStoryText.text = relic.relicStory;
            }
        }
    }
}
