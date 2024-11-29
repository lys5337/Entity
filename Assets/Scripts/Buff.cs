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
            strength, //힘 증가하는 만큼 대미지가 쌔짐
            block, // 방어력
            vulnerable, //취약 받피즘
            weak, // 공격력 약화
            ritual, //
            enrage,
            poison, //독 디버프, 매 턴이 끝날때마다 수치만큼 피해를 주고, -1를 한다. 0이 되면 제거
            bleeding, //출혈 디버스, 매턴이 끝날때마다 1의 피해를 준다. -1를 한다. 0이 되면 제거
            deathMark, //5중첩이 되면 적 체력 1로 변경, 감소 없음
            infectedBleeding, // 감염된 출혈 (추가 상태)
            excessiveBleeding, // 과다 출혈 (추가 상태)
            thornArmor,   // 출혈 가시 (적이 공격하면 출혈 부여)
            adrenaline,   // 아드레날린 (카드 사용 시 힘 증가)
            goddessBlessing, // 여신의 가호 (받는 피해 90% 감소)
            insanity,
            defenseStance,
            solidity
        }

        public Sprite buffIcon;
        [Range(0, 999)]
        public int buffValue;
        public BuffUI buffGO;
    }
}
