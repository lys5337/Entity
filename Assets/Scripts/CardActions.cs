using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ
{
    public class CardActions : MonoBehaviour
    {
        Card card;
        Enemy enemy;
        public Fighter target;
        public Fighter player;
        BattleSceneManager battleSceneManager;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
        }

        public void PerformAction(Card _card, Fighter _fighter)
        {
            // 카드의 제목에 따라 각 행동을 실행
            card = _card;
            target = _fighter;
            
            switch (card.cardTitle)
            {
                case "신성한 참격": // Divine Slash
                    DivineSlash();
                    break;

                case "베기": // Strike
                    Strike();
                    break;

                case "방어": // Defend
                    Defend();
                    break;

                case "약점 공략": // Bash
                    Bash();
                    break;

                case "힘줄 베기": // Clothesline
                    Clothesline();
                    break;

                case "분노": // Inflame
                    Inflame();
                    break;

                case "아이언 웨이브": // Iron Wave
                    IronWave();
                    break;

                case "연속 베기": // Double Strike
                    DoubleStrike();
                    break;

                case "독성 나이프": // Poison Knife
                    PoisonKnife();
                    break;

                case "손목 베기": // Wrist Flick
                    WristFlick();
                    break;

                case "정의의 한방": // Fist Of Justice
                    FistOfJustice();
                    break;

                case "충격 대비": // Shrug It Off
                    ShrugItOff();
                    break;

                case "방패 밀치기": // Body Slam
                    BodySlam();
                    break;

                case "방어태세": // Entrench
                    Entrench();
                    break;

                case "낙인": // Stigma
                    Stigma();
                    break;

                case "인파이팅": // Infighting
                    Infighting();
                    break;
                case "상처 도려내기":
                    Bloodletting();
                    break;

                default:
                    Debug.LogError("알 수 없는 카드 제목입니다: " + card.cardTitle);
                    break;
            }
        }

        /////////////////////////////////////////////////////////////////
        // 카드 액션 함수들 - 카드별로 구현된 액션을 수행 //
        /////////////////////////////////////////////////////////////////

        private void Strike()
        {
            AttackEnemy(card, target);
        }

        private void Clothesline()
        {
            AttackEnemy(card, target);
            ApplyBuff(Buff.Type.weak, card.GetBuffAmount());
        }

        private void Bash()
        {
            AttackEnemy(card, target);
            ApplyBuff(Buff.Type.vulnerable, card.GetBuffAmount());
        }

        private void Entrench()
        {
            player.AddBlock(player.currentBlock);
        }

        private void BodySlam()
        {
            int totalDamage = player.currentBlock;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
        }

        private void Inflame()
        {
            ApplyBuffToSelf(Buff.Type.strength);
        }

        private void IronWave()
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
            player.AddBlock(totalDamage);
        }

        private void ShrugItOff()
        {
            UseSkill(card);
            battleSceneManager.DrawCards(1);
        }

        private void Bloodletting()
        {
            AttackSelf();
            battleSceneManager.energy += 2;
        }

        private void Defend()
        {
            UseSkill(card);
        }

        private void WristFlick()
        {
            AttackEnemy(card, target);
            ApplyBuff(Buff.Type.bleeding, card.GetBuffAmount());
        }

        private void PoisonKnife()
        {
            AttackEnemy(card, target);
            ApplyBuff(Buff.Type.poison, card.GetBuffAmount());
        }

        private void DoubleStrike()
        {
            AttackEnemy(card, target);
            Invoke("AttackEnemy", 0.15f);
        }

        private void DivineSlash()
        {
            TrueAttackEnemy();
            for (int i = 1; i <= 2; i++)
            {
                Invoke("TrueAttackEnemy", 0.15f * i);
            }
        }

        private void Infighting()
        {
            TrueAttackEnemy();
            for (int i = 1; i <= 3; i++)
            {
                Invoke("TrueAttackEnemy", 0.15f * i);
            }
        }

        private void Stigma()
        {
            ApplyBuff(Buff.Type.deathMark, card.GetBuffAmount());
        }

        private void FistOfJustice()
        {
            AttackEnemy(card, target);
        }

        /////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        private void ApplyBuff(Buff.Type t, int amount)
        {
            target.AddBuff(t, amount);
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            player.AddBuff(t, card.GetBuffAmount());
        }

        private void AttackSelf()
        {
            player.TakeDamage(2);
        }

        private void TrueAttackEnemy()
        {
            // 타겟이 null이거나 이미 파괴된 상태인지 확인
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. TrueAttackEnemy를 처리할 수 없습니다.");
                return;
            }

            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                float a = totalDamage * 1.5f;
                Debug.Log("대미지가 " + totalDamage + "에서 " + (int)a + "로 증가했습니다.");
                totalDamage = (int)a;
            }

            target.TrueDamage(totalDamage); // 타겟에 트루 대미지 적용
        }


        private void AttackEnemy(Card card, Fighter target)
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
        }

        private void UseSkill(Card card)
        {
            int blockAmount = card.GetCardEffectAmount();
            player.AddBlock(blockAmount);
        }
    }
}
