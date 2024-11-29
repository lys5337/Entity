using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RelicIcon : MonoBehaviour
    {
        public Image relicIconImage; // 유물 아이콘을 표시할 Image 컴포넌트
        public Button openButton; // 설명 패널을 여는 버튼

        private string relicName;
        private string relicDescription;
        private string relicStory;

        private Button relicButton; // 아이콘의 버튼 컴포넌트

        // RelicStatusBox를 참조
        private RelicStatusBox relicStatusBox;

        // 초기화 메서드
        private void Awake()
        {
            relicButton = GetComponent<Button>();
        }

        // RelicStatusBox와 연결하고 유물 데이터를 설정하는 메서드
        public void Initialize(Sprite iconSprite, string name, string description, string story, RelicStatusBox statusBox)
        {
            relicIconImage.sprite = iconSprite;
            relicName = name;
            relicDescription = description;
            relicStory = story;
            relicStatusBox = statusBox;

            // 버튼 클릭 시 설명 패널을 여는 이벤트 연결
            if (relicButton != null)
            {
                relicButton.onClick.AddListener(ShowDescription);
            }

            // openButton 클릭 시 설명 패널을 여는 이벤트 연결
            if (openButton != null)
            {
                openButton.onClick.AddListener(ShowDescription);
            }
        }

        // 설명 패널에 유물 정보를 전달하고 패널을 여는 메서드
        private void ShowDescription()
        {
            if (relicStatusBox != null)
            {
                relicStatusBox.DisplayRelicDescription(relicIconImage.sprite, relicName, relicDescription, relicStory);
            }
        }
    }
}
