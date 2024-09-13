using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    [System.Serializable]
    public struct Buff
    {
        public Type type;

        public enum Type
        {
            strength,
            block,
            vulnerable,
            weak,
            ritual,
            enrage,
            poison, //독 디버프, 매 턴이 끝날때마다 수치만큼 피해를 주고, -1를 한다. 0이 되면 제거
            bleeding, //출혈 디버스, 매턴이 끝날때마다 1의 피해를 준다. -1를 한다. 0이 되면 제거
            deathMark //5중첩이 되면 적 체력 1로 변경, 감소 없음
        }

        public Sprite buffIcon;
        [Range(0, 999)]
        public int buffValue;
        public BuffUI buffGO;
    }
}
