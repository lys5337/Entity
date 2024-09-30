using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace TJ
{
    class QTEResults
    {
        public int perfectCount = 0;
        public int successCount = 0;
        public int failCount = 0;
    }

    public class CardActions : MonoBehaviour
    {
        Card card;
        Enemy enemy;
        public Fighter target;
        public Fighter player;
        BattleSceneManager battleSceneManager;
        public EffectManager effectManager;
        private CameraController cameraController;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            cameraController = FindObjectOfType<CameraController>();
        }

        public void PerformAction(Card _card, Fighter _fighter)
        {
            card = _card;
            target = _fighter;

            if (card.cardType == Card.CardType.Attack)
            {
            }

            switch (card.cardTitle)
            {
                case "신성한 참격":
                    DivineSlash();
                    break;

                case "베기":
                    Strike();
                    break;

                case "방어":
                    Defend();
                    break;

                case "약점 공략":
                    Bash();
                    break;

                case "힘줄 베기":
                    Clothesline();
                    break;

                case "분노":
                    Inflame();
                    break;

                case "아이언 웨이브":
                    IronWave();
                    break;

                case "연속 베기": 
                    DoubleStrike();
                    break;

                case "독성 나이프":
                    PoisonKnife();
                    break;

                case "손목 베기":
                    WristFlick();
                    break;

                case "충격 대비":
                    ShrugItOff();
                    break;

                case "방패 밀치기":
                    BodySlam();
                    break;

                case "참호": 
                    Entrench();
                    break;

                case "죽음의 낙인":
                    Stigma();
                    break;

                case "인파이팅":
                    Infighting();
                    break;

                case "상처 도려내기":
                    Cutoutwounds();
                    break;
                
                case "예리한 둔기":
                    SharpInstrument();
                    break;

                case "묵직한 한방":
                    DoomFist();
                    break;

                case "유연한 공격": //공격하고 한장 뽑기
                    FlexibleAttack();
                    break;
                
                case "대검": //힘 비례 대미지
                    GiantSword();
                    break;
                
                case "이판사판 태클": // 약화, 취약 동시 부여
                    DeathTackle();
                    break;

                case "강화된 방패": //방어 강화 버전
                    EnhancedShield();
                    break;

                case "피의 폭발": //출혈 관련 카드
                    BloodyExplosion();
                    break;

                case "정기 흡수": //데미지 5당 에너지 1 생성
                    EnergyDrain();
                    break;
                
                case "흡혈 베기": //피해를 주고 체력회복
                    VampiricAttack();
                    break;

                case "혈액 강탈": //출혈 1당 0.5회복, 강화시 1당 1회복
                    VampiricAbsorb();
                    break;
                
                case "심장파괴자": //궁극기
                    HeartBreaker_QTE();
                    break;
                
                case "정확한 패링":
                    AccurateParrying_QTE();
                    break;

                case "격돌":
                    Confrontation_QTE();
                    break;

                case "나뭇잎 검":
                    LeafSword();
                    break;
                
                case "세게 베기":
                    PowerStrike();
                    break;

                default:
                    Debug.LogError("알 수 없는 카드 제목입니다: " + card.cardTitle);
                    break;
            }
            
        }

        /////////////////////////////////////////////////////////////////
        // 카드 액션 함수들 - 카드별로 구현된 액션을 수행                  /
        ////////////////////////////////////////////////////////////////

        private void LeafSword()
        {
            CallEffect("Nature_Effect/CFXR3 Hit Leaves A (Lit)");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            int totalEnerge = card.GetBuffAmount();
            battleSceneManager.DrawCards(totalEnerge);
            battleSceneManager.energy += totalEnerge;
            AttackEnemy();
        }

        private void PowerStrike()
        {
            CallEffect();
            AttackEnemy();
        }

        private void Confrontation_QTE()
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }

            QTEResults results = new QTEResults();

            UnityAction onPerfect = () =>
            {
                if (target.currentHealth <= 0)
                {
                    Debug.Log("적의 체력이 0 이하입니다. QTE를 중단합니다.");
                    return;
                }
                AudioManager.Instance.PlaySound("일반 타격2");
                target.TakeDamage(totalDamage);

                Vector3 effectPosition = new Vector3(target.transform.position.x, target.transform.position.y+120, target.transform.position.z - 99);
                effectManager.PlayEffect("Impacts_Effect/CFXR Hit A (Red)", effectPosition);

                results.perfectCount++;
                Debug.Log("대성공! 방어력을 얻고, 얻은 방어력 만큼 피해를 준다.");
            };

            UnityAction onSuccess = () =>
            {
                if (target.currentHealth <= 0)
                {
                    Debug.Log("적의 체력이 0 이하입니다. QTE를 중단합니다.");
                    return;
                }
                AudioManager.Instance.PlaySound("일반 타격2");
                target.TakeDamage(totalDamage / 2);

                Vector3 effectPosition = new Vector3(target.transform.position.x+70, target.transform.position.y+50, target.transform.position.z - 99);
                effectManager.PlayEffect("Impacts_Effect/CFXR Hit A (Red)", effectPosition);

                results.successCount++;
                Debug.Log("방어력을 얻는다.");
            };

            UnityAction onFail = () =>
            {
                results.failCount++;
                Debug.Log("QTE 액션 실패");
            };

            StartCoroutine(ExecuteMultipleQTERings(onPerfect, onSuccess, onFail, results));
        }

        private IEnumerator ExecuteMultipleQTERings(UnityAction onPerfect, UnityAction onSuccess, UnityAction onFail, QTEResults results)
        {
            bool firstQTEComplete = false;
            battleSceneManager.StartQTERing(new Vector2(200, 200), 1.0f, 1.5f,
                () =>
                {
                    onPerfect.Invoke();
                    firstQTEComplete = true;
                },
                () =>
                {
                    onSuccess.Invoke();
                    firstQTEComplete = true;
                },
                () =>
                {
                    onFail.Invoke();
                    firstQTEComplete = true;
                }
            );

            yield return new WaitUntil(() => firstQTEComplete);

            if (target.currentHealth <= 0)
            {
                Debug.Log("적의 체력이 0 이하입니다. QTE 이벤트를 종료합니다.");
                yield break;
            }

            bool secondQTEComplete = false;
            battleSceneManager.StartQTERing(new Vector2(300, 225), 1.0f, 1.5f,
                () =>
                {
                    onPerfect.Invoke();
                    secondQTEComplete = true;
                },
                () =>
                {
                    onSuccess.Invoke();
                    secondQTEComplete = true;
                },
                () =>
                {
                    onFail.Invoke();
                    secondQTEComplete = true;
                }
            );

            yield return new WaitUntil(() => secondQTEComplete);

            if (target.currentHealth <= 0)
            {
                Debug.Log("적의 체력이 0 이하입니다. QTE 이벤트를 종료합니다.");
                yield break;
            }

            bool thirdQTEComplete = false;
            battleSceneManager.StartQTERing(new Vector2(400, 250), 1.0f, 1.5f,
                () =>
                {
                    onPerfect.Invoke();
                    thirdQTEComplete = true;
                },
                () =>
                {
                    onSuccess.Invoke();
                    thirdQTEComplete = true;
                },
                () =>
                {
                    onFail.Invoke();
                    thirdQTEComplete = true;
                }
            );

            yield return new WaitUntil(() => thirdQTEComplete);

            Debug.Log($"QTE 완료 - Perfect: {results.perfectCount}, Success: {results.successCount}, Fail: {results.failCount}");
        }


        private void HeartBreaker_QTE()
        {
            ApplyBuff(Buff.Type.bleeding, card.GetBuffAmount());
            CallEffect("Liquids_Effect/CFXR2 Blood (Directional)");
            UnityEngine.Events.UnityAction onSuccess = () =>
            {
                Debug.Log("QTE 성공! 적에게 공격을 실행합니다.");
                AttackEnemy();
                CallEffect("Misc_Effect/CFXR2 Broken Heart");
            };

            UnityEngine.Events.UnityAction onFail = () =>
            {
                Debug.Log("QTE 실패! 공격이 취소되었습니다.");
            };
            //인자 정보 vector2는 좌표, 1.0f 크기, 3은 난도,onSuccess는 QTE가 성공했을 때 호출될 콜백 함수를 전달하는 인자 ,실패했을 때 호출될 콜백 함수를 전달하는 인자야. 이렇게 구성
            battleSceneManager.StartQTESmash(new Vector2(500, 300), 0.9f, 2, onSuccess, onFail); //난도는 숫자가 낮을수록 쉬움 0~4 사이로 입력, 소수점 단위도 가능한듯?
        }

        private void AccurateParrying_QTE()
        {
            if (battleSceneManager.enemies[0].turns[battleSceneManager.enemies[0].turnNumber].intentType == EnemyAction.IntentType.Attack||
                battleSceneManager.enemies[0].turns[battleSceneManager.enemies[0].turnNumber].intentType == EnemyAction.IntentType.AttackDebuff)
            {
                int enemyDamage = battleSceneManager.enemies[0].turns[battleSceneManager.enemies[0].turnNumber].amount 
                                + battleSceneManager.enemies[0].thisEnemy.strength.buffValue;

                if (battleSceneManager.enemies[0].thisEnemy.weak.buffValue > 0)
                {
                    enemyDamage = (int)(enemyDamage * 0.75f);
                }

                UnityEngine.Events.UnityAction onPerfect = () =>
                {
                    Debug.Log("대성공! 적의 피해량만큼 방어력을 얻고, 그 방어력만큼 적에게 피해를 준다.");
                    AudioManager.Instance.PlaySound("패링");
                    player.AddBlock(enemyDamage);

                    battleSceneManager.cardTarget.TakeDamage(player.currentBlock);
                };

                UnityEngine.Events.UnityAction onSuccess = () =>
                {
                    Debug.Log("성공! 적의 피해량만큼 방어력을 얻는다.");
                    AudioManager.Instance.PlaySound("패링");
                    player.AddBlock(enemyDamage);
                };

                UnityEngine.Events.UnityAction onFail = () =>
                {
                    Debug.Log("QTE 실패! 아무 일도 일어나지 않는다.");
                };
                battleSceneManager.StartQTERing(new Vector2(400, 250), 1.0f, 0.5f, onPerfect, onSuccess, onFail);
            }
            else
            {
                Debug.Log("적의 의도가 공격이 아닙니다. QTE가 실행되지 않습니다.");
            }
        }


        private void VampiricAbsorb()
        {
            int bleedingAmount = target.bleeding.buffValue;
            int healingAmount = (int)(bleedingAmount * 0.5f);

            int currentHealth = player.currentHealth;
            int maxHealth = player.maxHealth;

            int totalHealing = healingAmount;
            int expectedHealthAfterHeal = currentHealth + totalHealing;

            if (expectedHealthAfterHeal > maxHealth)
            {
                totalHealing = maxHealth - currentHealth;
            }

            player.TakeDamage(-totalHealing);

            target.ReductBuff(Buff.Type.bleeding, bleedingAmount);
        }

        private void VampiricAttack()
        {
            bool checkValue = battleSceneManager.MaxHealthCheck();
            
            if (checkValue)
            {
                int amount = AttackEnemyReturn();
                int totalAmount = (int)(amount * 0.5f);

                int currentHealth = player.currentHealth;
                int maxHealth = player.maxHealth;

                int totalDamage = -totalAmount;
                int expectedHealthAfterHeal = currentHealth - totalDamage;

                if (expectedHealthAfterHeal > maxHealth)
                {
                    totalDamage = -(maxHealth - currentHealth);
                }

                player.TakeDamage(totalDamage);
            }
            else
            {
                AttackEnemy();
            }
        }
        
        private void EnergyDrain()
        {
            int amount = AttackEnemyReturn();
            int totalAmount = amount/5;
            battleSceneManager.energy += totalAmount;
        }

        private void BloodyExplosion()
        {
            int totalBuffCount = target.bleeding.buffValue;
            int totalDamage = totalBuffCount*4;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            ReduceBuff(Buff.Type.bleeding, totalBuffCount);
            target.TakeDamage(totalDamage);
        }

        private void EnhancedShield()
        {
            Defend();
        }

        private void DeathTackle()
        {
            AttackEnemy();
            ApplyBuff(Buff.Type.weak, card.GetBuffAmount());
            ApplyBuff(Buff.Type.vulnerable, card.GetBuffAmount());
        }

        private void GiantSword()
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue*2;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            target.TakeDamage(totalDamage);
        }

        public void Strike()
        {
            AudioManager.Instance.PlaySound("LightPunch");

            CallEffect();
            AttackEnemy();
        }

        private void Clothesline()
        {
            AttackEnemy();
            ApplyBuff(Buff.Type.weak, card.GetBuffAmount());
        }

        private void Bash()
        {
            AttackEnemy();
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
            AudioManager.Instance.PlaySound("일반 타격");
            CallEffect("Fire_Effect/CFXR3 Hit Fire B (Air)");
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
            Defend();
            battleSceneManager.DrawCards(1);
        }

        private void WristFlick()
        {
            AttackEnemy();
            CallEffect("Liquids_Effect/CFXR2 Blood (Directional)");
            ApplyBuff(Buff.Type.bleeding, card.GetBuffAmount());
        }

        private void PoisonKnife()
        {
            AttackEnemy();
            ApplyBuff(Buff.Type.poison, card.GetBuffAmount());
        }

        private void DoubleStrike()
        {
            AttackEnemy();
            CallEffect();
            AudioManager.Instance.PlaySound("일반 타격2");
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
            for (int i = 1; i <= 5; i++)
            {
                Invoke("TrueAttackEnemy", 0.1f * i);
            }
        }

        private void Stigma()
        {
            ApplyBuff(Buff.Type.deathMark, card.GetBuffAmount());
        }

        private void Cutoutwounds() //상처 도려내기
        {
            int totalDamage = card.GetCardEffectAmount();
            int totalEnerge = card.GetBuffAmount();
            battleSceneManager.energy += totalEnerge;
            player.TakeDamage(totalDamage);
        }

        private void SharpInstrument() //예둔
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            if (target.bleeding.buffValue > 0)
            {
                totalDamage = totalDamage * 2;
                CallEffect("Liquids_Effect/CFXR2 Blood (Directional)");
                target.TakeDamage(totalDamage);
            }
            else
            {
                CallEffect("Impacts_Effect/CFXR2 Ground Hit");
                target.TakeDamage(totalDamage);
            }
            
        }   

        private void DoomFist()
        {
            CallEffect("Explosions_Effect/CFXR2 WW Explosion");
            AttackEnemy();
        }

        private void FlexibleAttack()
        {
            CallEffect();
            AttackEnemy();
            battleSceneManager.DrawCards(1);
        }

        /////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        public void CallEffect(string effectName = "Slash_Effect/Basic Slash Blue") //d이펙트 호출하는 놈/기본 오버라이딩으로 베기 이펙트 넣어 놓음
        {
            int positionVal=0;
            if(effectName=="Slash_Effect/Basic Slash Blue")
            {
                positionVal = -10;
            }
            else
            {
                positionVal = -1;
            }
            Vector3 effectPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z + positionVal);
            effectManager.PlayEffect(effectName, effectPosition);
        }

        private void ApplyBuff(Buff.Type t, int amount)
        {
            target.AddBuff(t, amount);
        }

        private void ReduceBuff(Buff.Type t, int amount)
        {
            target.ReductBuff(t, amount);
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            player.AddBuff(t, card.GetBuffAmount());
        }

        private void AttackEnemy()
        {   
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }

            target.TakeDamage(totalDamage);
        }

        private int AttackEnemyReturn()
        {
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue;
            if (target.vulnerable.buffValue > 0)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            return target.TakeDamageReturn(totalDamage);
        }

        private void TrueAttackEnemy()
        {
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

            target.TrueDamage(totalDamage);
        }

        private void Defend()
        {
            int blockAmount = card.GetCardEffectAmount();
            player.AddBlock(blockAmount);
        }
    }
}
