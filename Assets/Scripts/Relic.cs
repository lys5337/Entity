using UnityEngine;

namespace TJ
{
    [CreateAssetMenu(fileName = "New Relic", menuName = "Relic")]
    public class Relic : ScriptableObject
    {
        public string relicName;       // 유물 이름
        public string relicDescription; // 유물 설명
        public Sprite relicIcon;       // 유물 아이콘 (필요시)
    }
}
