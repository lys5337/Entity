using UnityEngine;

namespace TJ
{
    [CreateAssetMenu(fileName = "New Relic", menuName = "Relic")]
    public class Relic : ScriptableObject
    {
        public string relicName;       // 유물 이름
        
        public Sprite relicIcon;       // 유물 아이콘
        
        [TextArea(3, 10)]              // 유물 설명을 위한 넓은 텍스트 필드 (3줄 최소, 10줄 최대)
        public string relicDescription; 
        
        
        [TextArea(3, 10)]              // 유물 스토리를 위한 넓은 텍스트 필드 (3줄 최소, 10줄 최대)
        public string relicStory; 
    }
}
