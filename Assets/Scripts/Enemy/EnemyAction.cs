using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    [System.Serializable]
    public class EnemyAction
    {
        public IntentType intentType;
        public enum IntentType{None,Attack,Block,StrategicBuff,StrategicDebuff,AttackDebuff,AttackBuff, EliteWolf_0Stage,
        BossBear_0Stage, SpecialAttack ,Waiting, RangedAttack,RangedAttackDebuff, SelfBuffPlayerDeuff, GoldStealAttack }

        public IntentType2 intentType2;
        public enum IntentType2{None, Attack,Block,StrategicBuff,StrategicDebuff,AttackDebuff,AttackBuff, SpecialAttack}

        [HideInInspector]public string enemyEffectName;
        public int amount;
        public int debuffAmount;
        public Buff.Type buffType;

        public int amount2;
        public int debuffAmount2;
        public Buff.Type buffType2;

        public int chance;
        public Sprite icon;

        public bool triggerOnLowHealth;
        public float healthThreshold;
    }
}