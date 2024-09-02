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
            poison,
            bleeding,
            deathMark,
            target,
            counterattack,
            oiled,
            wet,
            grilled
        }

        public Sprite buffIcon;
        [Range(0, 999)]
        public int buffValue;
        public BuffUI buffGO;

        // 버프 로직 구현 파트
        public void ApplyBuff(Fighter target)
        {
            switch (type)
            {
                case Type.poison:
                    target.TakeDamage(buffValue); // 독 피해 적용
                    buffValue--; // 독 수치 감소
                    if (buffValue <= 0)
                    {
                        target.RemoveBuff(this); // 독 수치가 0이 되면 버프 제거
                    }
                    break;

                case Type.bleeding:
                    target.TakeDamage(buffValue);
                    buffValue--;
                    if (buffValue <= 0)
                    {
                        target.RemoveBuff(this); // Bleeding 버프 제거
                    }
                    break;

                case Type.deathMark:
                    target.TakeDamage(buffValue);
                    buffValue--;
                    if (buffValue <= 0)
                    {
                        target.RemoveBuff(this); // DeathMark 버프 제거
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
