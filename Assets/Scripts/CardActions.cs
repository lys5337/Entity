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
        private GameManager gameManager;
        private Fighter fighter;

        

        public bool playerDrainEffectOnOff = true;

        public int CardActionEnemyPower, CardActionEnemyAmor, CardActionEnemyMaxhealth;

        
        private Animator animator;
        public GameObject playerIcon; // PlayerIcon 오브젝트를 참조할 변수

        public GameObject cardGenerateUI;

        private void Awake()
        {
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            cameraController = FindObjectOfType<CameraController>();
            gameManager = FindObjectOfType<GameManager>();
            fighter = GetComponent<Fighter>();
            enemy = GetComponent<Enemy>();

            if (battleSceneManager != null)
            {
                player = battleSceneManager.player; // 플레이어 참조 가져오기
            }
        }

        void Start()
        {
            animator = playerIcon.GetComponent<Animator>();
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

                case "손목 베기": //기본출혈 카드, 출혈 3부여
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
                
                case "예리한 둔기": // 출혈 상태일 경우 2배 피해
                    SharpInstrument();
                    break;

                case "성운멸쇄권":
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

                case "피의 폭발": // 출혈 관련 카드, 출혈 중첩당 피해
                    BloodyExplosion();
                    break;

                case "정기 흡수": //데미지 5당 에너지 1 생성
                    EnergyDrain();
                    break;
                
                case "흡혈 베기": //피해를 주고 체력회복
                    VampiricAttack();
                    break;

                case "혈액 강탈": // 출혈 1당 0.5회복, 강화시 1당 1회복
                    VampiricAbsorb();
                    break;
                
                case "심장파괴자": //출혈카드, 30피해를 주고, QTE성공시 출혈 30부여
                    HeartBreaker_QTE();
                    break;
                
                case "정확한 패링":
                    AccurateParrying_QTE();
                    break;

                case "격돌":
                    Confrontation_QTE();
                    break;

                case "자연의 일격":
                    LeafSword();
                    break;
                
                case "세게 베기":
                    PowerStrike();
                    break;

                case "아드로핀": //체력을 잃고 힘 버프
                    Atropine();
                    break;

                case "판별-수비": //적의 의도가 공격일 때 방어력 보너스
                    Discrimination_Defend();
                    break;
                    
                case "판별-공격": //적의 의도가 방어일때 트루어택
                    Discrimination_Attack();
                    break;

                case "시체털이": //적에게 피해를 주고 적이 죽으면 적의 총 체력만큼 골드 흭득
                    StealGold();
                    break;

                case "약자무시": //적의 체력이 50% 이하일 때 2배의 피해
                    IgnoreWeak();
                    break;
                
                case "선수필승": //적의 체력이 가득 찬 상태일 경우 2배의 피해
                    PreemptiveStrike();
                    break;
                
                case "맹독": //독 중첩 5 추가
                    Venomous();
                    break;

                case "여신의 가호": //여신의 가호 버프 3중첩 흭득, 1중첩당 1턴동안 받는 피해 90% 감소 3턴 지속
                    GoddessBlessing();
                    break;
                
                case "정밀 단도": //50%확률로 1.5배의 피해
                    PrecisionDagger();
                    break;

                case "응급처치": //체력 10 회복, 50% 미만일경우 회복 효과 50%상승
                    FirstAid();
                    break;
                
                case "독살": //적의 독 수치가 없을경우 독 10을 부여
                    Poisoning();
                    break;

                case "고혈압": //출혈, 적의 출혈 수치를 2배로 증가
                    Hypertension();
                    break;
                
                case "자해": //자신에게 출혈 5부여, 다음의 자신의 턴 에너지+1, 카드 1장 뽑기
                    SelfHarm();
                    break;
                
                case "파상풍": //적이 출혈 상태일 경우 출혈 디버프를 감염된 출혈로 변경
                    Tetanus();
                    break;
                
                case "혈액팩": //자신이 출혈 상태일경우 출혈 중첩을 5 제거하고, 체력을 10 회복한다.
                    BloodPack();
                    break;

                case "피의 소용돌이": //자신의 모든 출혈 수치를 제거하고, 제거한 수치 1당 5의 피해를 주고, 1당 1의 체력을 회복한다.
                    BloodVortex();
                    break;

                case "피바라기": //플레이어, 적 중 출혈 상태인 경우 즉, 플레이어가 출혈이거나, 적이 출혈이거나, 둘다 출혈인 경우 혹은 모두 출혈이 아닌경우, 출혈 상태인 인원의 수가 0일경우 기본 효과, 1일 경우 피해를 주고 준피를 회복, 2일 경우 모든 출혈을 합하고 그 수치의 2배만큼 피해를 주고 회복한다.
                    BloodBath();
                    break;

                case "광기의 지배":
                    Insanity();
                    break;
                
                case "처단자": //적의 체력이 25% 이하일 경우 적을 처형한다., 키워드-처형: 적의 체력을 0으로 되도록 피해를 줌|| 연산법: 적을 현재 채력을 불러오고, 현재 카드의 최종 피해량과 적의 현재 체력/최대체력으로 계산하여 적의 체력이 25% 이하일 때, 적의 현재 체력 만큼 적에게 피해량을 준다.
                    Executioner();
                    break;
                
                case "끈적한 피": //자신의 출혈을 제거하고, 제거한 출혈 1당 1의 체력을 회복한다.
                    StickyBlood();
                    break;

                case "톱날 단검": //적에게 2의 피해를 주고, 출혈 5를 부여한다.
                    SawtoothDagger();
                    break;

                case "주머니 탐색":
                    SearchPocket();
                    break;

                case "만병통치약"://플레이어에게 걸린 모든 버프 제거
                    Panacea();
                    break;
                
                case "가시 갑옷": //버프명: 출혈가시, 카드 사용시 플레이어에게 출혈가시 버프 부여 ,출혈가시 효과 : 적이 플레이어를 공격할 경우 적에게 출혈 2를 부여한다.
                    ThornArmor();
                    break;

                case "방어태세":
                    DefenseStance();
                    break;
                
                case "위기 모면": //치명적인 피해를 받을 경우 체력 체력 10% 회복
                    //CrisisAversion();
                    break;

                case "질량 증가": //다음에 주는 카드의 피해량이 1.5배 증가
                    //MassIncrease();
                    break;

                case "피의 노래": //공격 카드를 사용할 때마다 체력 1회복
                    //SongOfBlood();
                    break;
                
                case "아드레날린": //아드레날린 버프 흭득, 효과: 카드를 사용할때마다 힘 1증가, 최대 3중첩, 턴 종료시 해당 버프 삭제
                    //Adrenaline();
                    break;
                
                case "증오의 함성": //이번 적의 의도가 공격으로 변경
                    //CryOfHatred();
                    break;
                
                case "전술적 판단": //3개 선택지중 하나 선택(공격, 방어, 카드 뽑기) //기능 개발 필요, 기능명: 선택
                    //TacticalJudgment();
                    break;
                    
                

                default:
                    Debug.LogError("알 수 없는 카드 제목입니다: " + card.cardTitle);
                    break;
            }
        }
        /////////////////////////////////////////////////////////////////
        // 카드 액션 함수들 - 카드별로 구현된 액션을 수행                  /
        ////////////////////////////////////////////////////////////////

        private void DefenseStance()
        {
            ApplyBuffToSelf(Buff.Type.defenseStance);
            BattleSceneManager battleSceneManager = FindObjectOfType<BattleSceneManager>();
            battleSceneManager.defenseStanceJudge = true;
        }

        private void Insanity()
        {
            ApplyBuffToSelf(Buff.Type.insanity);
            Fighter fighter = FindObjectOfType<Fighter>();
            fighter.insanityJudge = true;
        }

        private void ThornArmor()
        {
            ApplyBuffToSelf(Buff.Type.thornArmor);
        }

        private void Panacea()
        {
            player.resetDebuffs(); // 플레이어의 모든 버프 및 디버프를 제거
            Debug.Log("플레이어의 모든 버프와 디버프가 제거되었습니다.");
        }

        private void SearchPocket()
        {
            AudioManager.Instance.PlaySound("카드 넘기는 소리");
            cardGenerateUI.SetActive(true);
        }

        private void SawtoothDagger()
        {
            CallEffect("SwordTrail_Effect/CFXR4 Sword Trail FIRE (360 Thin Spiral)");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            AttackEnemy();
            ApplyBuff(Buff.Type.bleeding, card.GetBuffAmount());
        }


        private void StickyBlood()
        {
            int bleedingAmount = player.bleeding.buffValue;
            player.ReductBuff(Buff.Type.bleeding, bleedingAmount);
            player.HealEntity((int)(bleedingAmount*card.GetAditionalValueAmountFloat()));
        }

        private void Executioner() // 처단자
        {
            
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            BattleSceneManager battleSceneManager = FindObjectOfType<BattleSceneManager>();

            if (battleSceneManager != null)
            {
                bool isEliteOrBoss = battleSceneManager.eliteFight || battleSceneManager.bossFight;

                AttackEnemy();

                if (!isEliteOrBoss && target.currentHealth <= target.maxHealth * 0.25f)
                {
                    TakeDamageCardAction(target.currentHealth);
                    Debug.Log("적이 체력 25% 미만이므로 처형이 적용되었습니다.");
                    CallEffect("Misc_Effect/CFXR2 Broken Heart");
                }
                else if (isEliteOrBoss)
                {
                    CallEffect();
                    Debug.Log("엘리트나 보스 몬스터이므로 처형이 적용되지 않습니다.");
                }
            }
            else
            {
                Debug.LogError("BattleSceneManager를 찾을 수 없습니다.");
            }
        }

        public void BloodBath()
        {
            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            int baseDamage = 10;
            int totalDamage = 0;
            int bleedingEntities = 0;

            if (player.bleeding.buffValue > 0)
            {
                bleedingEntities++;
            }
            if (target.bleeding.buffValue > 0)
            {
                bleedingEntities++;
            }

            switch (bleedingEntities)
            {
                case 0:
                    totalDamage = baseDamage;
                    break;
                case 1:
                    totalDamage = baseDamage + player.bleeding.buffValue + target.bleeding.buffValue;
                    break;
                case 2:
                    totalDamage = (int)((baseDamage + player.bleeding.buffValue + target.bleeding.buffValue) * card.GetAditionalValueAmountFloat());
                    break;
            }

            player.HealEntity(totalDamage);
            TakeDamageCardAction(totalDamage);
        }


        private void BloodVortex()
        {
            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            int bleedingAmount = player.bleeding.buffValue;
            player.ReductBuff(Buff.Type.bleeding, bleedingAmount);

            int totaldamage = bleedingAmount * card.GetAditionalValueAmount();

            TakeDamageCardAction(totaldamage);
            player.HealEntity(totaldamage);
        }

        private void SelfHarm() //자해
        {
            ApplyBuffToSelf(Buff.Type.bleeding);
            battleSceneManager.energy += card.GetAditionalValueAmount();
            battleSceneManager.DrawCards(card.GetAditionalValueAmount());
        }


        private void Tetanus() //파상풍
        {
            if(card.IsCardUpgraded())
            {
                ApplyBuff(Buff.Type.bleeding, 5);
                ApplyBuff(Buff.Type.infectedBleeding, target.bleeding.buffValue);
                target.ReductBuff(Buff.Type.bleeding, target.bleeding.buffValue);
            }
            else
            {
                ApplyBuff(Buff.Type.infectedBleeding, target.bleeding.buffValue);
                target.ReductBuff(Buff.Type.bleeding, target.bleeding.buffValue);
            }
        }


        private void BloodPack()
        {
            if (player.bleeding.buffValue >= 5)
            {
                player.ReductBuff(Buff.Type.bleeding, 5);
                player.HealEntity(10);
            }
            else
            {
                player.HealEntity(player.bleeding.buffValue);
                player.ReductBuff(Buff.Type.bleeding, player.bleeding.buffValue);
            }
        }

        private void Venomous()
        {
            ApplyBuff(Buff.Type.poison, 5); // 독 5 중첩 부여
        }

        private void GoddessBlessing()
        {
            ApplyBuffToSelf(Buff.Type.goddessBlessing); // 버프 3 중첩 부여
        }



        private void Hypertension() //고혈압
        {
            if (target.bleeding.buffValue > 0)
            {
                target.bleeding.buffValue = (int)(target.bleeding.buffValue * card.GetAditionalValueAmountFloat());
                target.bleeding.buffGO.DisplayBuff(target.bleeding);
            }
        }


        private void Poisoning() //독살
        {
            if (target.poison.buffValue >= 0)
            {
                ApplyBuff(Buff.Type.poison, card.GetAditionalValueAmount()); //독 10 중첩 부여
            }
            else
            {
                ApplyBuff(Buff.Type.poison, card.GetBuffAmount());
            }
        }


        private void FirstAid() // 응급처치
        {
            int healingAmount = 10 * card.GetAditionalValueAmount();

            if (player.currentHealth < player.maxHealth / 2)
            {
                healingAmount = (int)(healingAmount * 1.5f);
            }

            int currentHealth = player.currentHealth;
            int maxHealth = player.maxHealth;
            int expectedHealthAfterHeal = currentHealth + healingAmount;

            if (expectedHealthAfterHeal > maxHealth)
            {
                healingAmount = maxHealth - currentHealth;
            }

            CallEffectSelf("Magic_Effect_addition/Buff_03a");

            player.HealEntity(healingAmount);
        }


        private void PrecisionDagger() //정밀단도
        {
            animator.Play("PlayerGoAttack");
            CallEffect("SwordTrail_Effect/CFXR4 Sword Hit ICE (Cross)");
            AudioManager.Instance.PlaySound("칼던지는 소리");

            int totalDamage = card.GetCardEffectAmount();
            if (Random.value < 0.5f)
            {
                totalDamage = (int)(totalDamage * 1.5f);
            }
            TakeDamageCardAction(totalDamage);
        }


        private void PreemptiveStrike()
        {
            animator.Play("PlayerGoAttack");
            CallEffect("Misc_Effect/CFXR3 Hit Misc A");
            AudioManager.Instance.PlaySound("칼던지는 소리");

            int totalDamage = card.GetCardEffectAmount();
            if (target.currentHealth == target.maxHealth)
            {
                totalDamage *= 2;
            }
            TakeDamageCardAction(totalDamage);
        }


        private void IgnoreWeak()
        {
            animator.Play("PlayerGoAttack");
            CallEffect("Impacts_Effect/CFXR Hit D 3D (Yellow)");
            AudioManager.Instance.PlaySound("칼던지는 소리");

            int totalDamage = card.GetCardEffectAmount();
            if (target.currentHealth <= target.maxHealth / 2)
            {
                totalDamage *= 2;
            }
            TakeDamageCardAction(totalDamage);
        }

        private void StealGold()
        {
            animator.Play("PlayerGoAttack");
            CallEffect("Light_Effect/CFXR3 Hit Light B (Air)");
            

            AttackEnemy();
            if (target.currentHealth <= 0)
            {
                int goldToSteal = target.maxHealth;
                gameManager.AddGold(goldToSteal);
                Debug.Log($"{goldToSteal} 골드를 흭득했습니다.");
                AudioManager.Instance.PlaySound("동전 드롭 조금");
            }
            else
                AudioManager.Instance.PlaySound("칼던지는 소리");
        }

        private void Atropine()
        {
            player.TakeDamage(card.GetCardEffectAmount());
            ApplyBuffToSelf(Buff.Type.strength);
        }

        private void Discrimination_Attack()
        {
            CallEffect();
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");
            if (battleSceneManager.enemies[0].turns[battleSceneManager.enemies[0].turnNumber].intentType == EnemyAction.IntentType.Block)
            {
                TrueAttackEnemy();
            }
            else
            {
                AttackEnemy();
            }
        }

        private void Discrimination_Defend()
        {
            EnemyAction.IntentType[] attackIntents = new EnemyAction.IntentType[]
            {
                EnemyAction.IntentType.Attack,
                EnemyAction.IntentType.AttackDebuff,
                EnemyAction.IntentType.AttackBuff,
                EnemyAction.IntentType.EliteWolf_0Stage,
                EnemyAction.IntentType.BossBear_0Stage,
                EnemyAction.IntentType.RangedAttack,
                EnemyAction.IntentType.RangedAttackDebuff,
                EnemyAction.IntentType.SpecialAttack,
                EnemyAction.IntentType.GoldStealAttack
            };


            var enemy = battleSceneManager.enemies[0];
            var currentTurn = enemy.turns[enemy.turnNumber];
            if (System.Array.Exists(attackIntents, intent => intent == currentTurn.intentType))
            {
                int blockAmount = card.GetCardEffectAmount() + player.solidity.buffValue;
                player.AddBlock(blockAmount*2);
            }
            else
            {
                Defend();
            }
        }

        private void LeafSword()
        {
            CallEffect("Nature_Effect/CFXR3 Hit Leaves A (Lit)");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            int totalEnerge = card.GetBuffAmount();
            battleSceneManager.DrawCards(totalEnerge);
            battleSceneManager.energy += totalEnerge;
            AttackEnemy();
        }

        private void PowerStrike()
        {
            CallEffect();
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");
            AttackEnemy();
        }

        private void Confrontation_QTE()
        {
            int totalDamage = card.GetCardEffectAmount();

            QTEResults results = new QTEResults();
            
            animator.Play("PlayerGoAttackLong");

            UnityAction onPerfect = () =>
            {
                if (target.currentHealth <= 0)
                {
                    Debug.Log("적의 체력이 0 이하입니다. QTE를 중단합니다.");
                    return;
                }
                AudioManager.Instance.PlaySound("일반 타격2");
                TakeDamageCardAction(totalDamage);

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
                TakeDamageCardAction(totalDamage / 2);

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
            battleSceneManager.StartQTERing(new Vector2(330, 60), 1.0f, 1.5f,
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
            yield return new WaitForSeconds(0.5f); 

            if (target.currentHealth <= 0)
            {
                Debug.Log("적의 체력이 0 이하입니다. QTE 이벤트를 종료합니다.");
                yield break;
            }

            bool secondQTEComplete = false;
            battleSceneManager.StartQTERing(new Vector2(410, 90), 1.0f, 1.5f,
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
            yield return new WaitForSeconds(0.5f); 

            if (target.currentHealth <= 0)
            {
                Debug.Log("적의 체력이 0 이하입니다. QTE 이벤트를 종료합니다.");
                yield break;
            }

            bool thirdQTEComplete = false;
            battleSceneManager.StartQTERing(new Vector2(400, 10), 1.0f, 1.5f,
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
            AudioManager.Instance.PlaySound("칼던지는 소리");

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
            var enemy = battleSceneManager.enemies[0];
            var currentTurn = enemy.turns[enemy.turnNumber];

            EnemyAction.IntentType[] attackIntents = new EnemyAction.IntentType[]
            {
                EnemyAction.IntentType.Attack,
                EnemyAction.IntentType.AttackDebuff,
                EnemyAction.IntentType.AttackBuff,
                EnemyAction.IntentType.EliteWolf_0Stage,
                EnemyAction.IntentType.BossBear_0Stage,
                EnemyAction.IntentType.RangedAttack,
                EnemyAction.IntentType.RangedAttackDebuff,
                EnemyAction.IntentType.SpecialAttack,
                EnemyAction.IntentType.GoldStealAttack
            };

            if (System.Array.Exists(attackIntents, intent => intent == currentTurn.intentType))
            {
                int enemyDamage = CalculateEnemyDamage(enemy);

                UnityEngine.Events.UnityAction onPerfect = () =>
                {
                    Debug.Log("QTE: Perfect 성공!");
                    AudioManager.Instance?.PlaySound("패링");
                    player.AddBlock(enemyDamage);

                    if (battleSceneManager.cardTarget != null)
                    {
                        battleSceneManager.cardTarget.TakeDamage(player.currentBlock + player.strength.buffValue);
                    }
                    else
                    {
                        Debug.LogWarning("QTE 반격 타겟이 없습니다!");
                    }
                };

                UnityEngine.Events.UnityAction onSuccess = () =>
                {
                    Debug.Log("QTE: 성공!");
                    AudioManager.Instance?.PlaySound("패링");
                    player.AddBlock(enemyDamage);
                };

                UnityEngine.Events.UnityAction onFail = () =>
                {
                    Debug.Log("QTE 실패! 아무 일도 일어나지 않는다.");
                };

                battleSceneManager.StartQTERing(new Vector2(400, 250), 1.0f, 1.2f, onPerfect, onSuccess, onFail);
            }
            else
            {
                Debug.Log("적의 의도가 공격이 아닙니다. QTE가 실행되지 않습니다.");
            }
        }

        private int CalculateEnemyDamage(Enemy enemy)
        {
            int baseDamage = enemy.turns[enemy.turnNumber].amount;
            int buffedDamage = baseDamage + enemy.thisEnemy.strength.buffValue + target.GetEnemyPower();

            if (enemy.thisEnemy.weak.buffValue > 0)
            {
                buffedDamage = (int)(buffedDamage * 0.75f);
            }

            if (player.vulnerable.buffValue > 0)
            {
                buffedDamage = (int)(buffedDamage * 1.5f);
            }

            Debug.Log($"계산된 적의 대미지: {buffedDamage}");
            return buffedDamage;
        }



        private void VampiricAbsorb()
        {
            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            int bleedingAmount = target.bleeding.buffValue;
            int healingAmount = (int)(bleedingAmount * 0.5f);
            player.HealEntity(healingAmount);
            target.ReductBuff(Buff.Type.bleeding, bleedingAmount);
        }


        private void VampiricAttack()
        {
            bool checkValue = battleSceneManager.MaxHealthCheck();

            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            if (checkValue)
            {
                int amount = AttackEnemyReturn();
                int totalAmount = (int)(amount * card.GetAditionalValueAmountFloat());
                player.HealEntity(totalAmount);
            }
            else
            {
                AttackEnemy();
            }
        }

        
        private void EnergyDrain()
        {
            CallEffect("Impacts_Effect/CFXR Impact Glowing HDR (Blue)");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            int amount = AttackEnemyReturn();
            int totalAmount = amount/card.GetAditionalValueAmount();
            battleSceneManager.energy += totalAmount;
        }

        private void BloodyExplosion()
        {
            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            CallEffect("Liquids_Effect/CFXR2 Blood (Directional)");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            int totalBuffCount = target.bleeding.buffValue;
            int totalDamage = totalBuffCount*card.GetAditionalValueAmount();

            ReduceBuff(Buff.Type.bleeding, totalBuffCount);
            TakeDamageCardAction(totalDamage);
        }

        private void EnhancedShield()
        {
            Defend();
        }

        private void DeathTackle()
        {
            CallEffect("temp/CFX_Hit_C White");
            AudioManager.Instance.PlaySound("일반 타격");
            animator.Play("PlayerGoAttack");

            ApplyBuff(Buff.Type.weak, card.GetBuffAmount());
            ApplyBuff(Buff.Type.vulnerable, card.GetBuffAmount());
            AttackEnemy();
        }

        private void GiantSword()
        {
            CallEffect("Slash_Effect/Shiny Slash");
            AudioManager.Instance.PlaySound("검으로 베는 소리");
            animator.Play("PlayerGoAttack");

            int amount = card.GetCardEffectAmount();
            int additionalAmount = card.GetAditionalValueAmount()*player.strength.buffValue;

            amount += additionalAmount;
            TakeDamageCardAction(amount);
        }

        private void Clothesline()
        {
            CallEffect("Slash_Effect/Shiny Slash");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            AttackEnemy();
            ApplyBuff(Buff.Type.weak, card.GetBuffAmount());
        }

        private void Bash()
        {
            AudioManager.Instance.PlaySound("일반 타격");
            animator.Play("PlayerGoAttack");
            CallEffect();
            AttackEnemy();
            ApplyBuff(Buff.Type.vulnerable, card.GetBuffAmount());
        }

        private void Entrench()
        {
            int amount =0;
            amount = player.currentBlock*card.GetAditionalValueAmount();
            player.AddBlock(amount+ player.solidity.buffValue);
        }

        private void BodySlam()
        {
            player.AddBlock(card.GetAditionalValueAmount());
            int totalDamage = player.currentBlock;
            AudioManager.Instance.PlaySound("일반 타격");
            CallEffect("Fire_Effect/CFXR3 Hit Fire B (Air)");
            TakeDamageCardAction(totalDamage);
        }

        private void Inflame()
        {
            ApplyBuffToSelf(Buff.Type.strength);
        }

        private void IronWave()
        {
            CallEffect("temp/CFX_Hit_C White");
            AudioManager.Instance.PlaySound("일반 타격");
            animator.Play("PlayerGoAttack");

            int totalDamage = card.GetCardEffectAmount();
            totalDamage=TakeDamageCardActionReturn(totalDamage);
            player.AddBlock(totalDamage+ player.solidity.buffValue);
        }

        private void ShrugItOff()
        {
            Defend();
            battleSceneManager.DrawCards(card.GetAditionalValueAmount());
        }

        private void WristFlick()
        {
            CallEffect("Liquids_Effect/CFXR2 Blood Shape Splash");
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            AttackEnemy();
            CallEffect("Liquids_Effect/CFXR2 Blood (Directional)");
            ApplyBuff(Buff.Type.bleeding, card.GetBuffAmount());
        }

        private void PoisonKnife()
        {
            CallEffect("temp/CFX4 Bubbles Whirl");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            AttackEnemy();
            ApplyBuff(Buff.Type.poison, card.GetBuffAmount());
        }

        private void DoubleStrike()
        {
            AttackEnemy();
            CallEffect("Slash_Effect/Double Slash");
            AudioManager.Instance.PlaySound("일반 타격2");
            animator.Play("PlayerGoAttack");
            Invoke("AttackEnemy", 0.15f);
        }

        private void DivineSlash()
        {
            animator.Play("PlayerDivineSlash");

            TrueAttackEnemy();
            CallEffect("SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)");
            CallEffect();
            AudioManager.Instance.PlaySound("칼던지는 소리");

            StartCoroutine(PerformDelayedAttacks());
        }

        private IEnumerator PerformDelayedAttacks()
        {
            for (int i = 1; i <= 2; i++)
            {
                yield return new WaitForSeconds(0.5f); 
                TrueAttackEnemy();
                CallEffect("SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)");
                CallEffect();
                AudioManager.Instance.PlaySound("칼던지는 소리");
            }
        }


        private void Infighting()
        {
            TrueAttackEnemy();
            animator.Play("PlayerGoAttack");
            for (int i = 1; i <= card.GetAditionalValueAmount(); i++)
            {
                CallEffect("Explosions_Effect/CFXR2 WW Explosion");
                AudioManager.Instance.PlaySound("칼던지는 소리");
                Invoke("TrueAttackEnemy", 0.1f * i);
            }
        }

        private void Stigma()
        {
            CallEffect("temp/CFX3_Skull_Explosion");

            ApplyBuff(Buff.Type.deathMark, card.GetBuffAmount());
        }

        private void Cutoutwounds() //상처 도려내기
        {
            
            AudioManager.Instance.PlaySound("출혈 공격");
            animator.Play("PlayerGoAttack");

            int totalDamage = card.GetCardEffectAmount();
            int totalEnerge = card.GetBuffAmount();
            battleSceneManager.energy += totalEnerge;
            player.TakeDamage(totalDamage);
        }

        private void SharpInstrument() //예둔
        {
            int totalDamage = card.GetCardEffectAmount();

            if (target.bleeding.buffValue > 0)
            {
                totalDamage = totalDamage * card.GetAditionalValueAmount();
                CallEffect("Explosions_Effect/CFXR2 WW Explosion");
                AudioManager.Instance.PlaySound("일반 타격2");
                TakeDamageCardAction(totalDamage);
            }
            else
            {
                CallEffect("Impacts_Effect/CFXR2 Ground Hit");
                AudioManager.Instance.PlaySound("일반 타격2");
                TakeDamageCardAction(totalDamage);
            }
            animator.Play("PlayerGoAttack");
        }

        private void DoomFist()
        {
            CallEffect("Explosions_Effect/CFXR2 WW Explosion");
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");
            AttackEnemy();
        }

        private void FlexibleAttack()
        {
            CallEffect();
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");

            AttackEnemy();
            battleSceneManager.DrawCards(card.GetAditionalValueAmount());
        }

        public void Strike()
        {
            CallEffect();
            AudioManager.Instance.PlaySound("칼던지는 소리");
            animator.Play("PlayerGoAttack");
            AttackEnemy();
        }

        /////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        public void playerMaxHealthIncrease(int amount)
        {
            player.maxHealth += amount;
            GameManager.Instance.IncreasePlayerMaxHealth(amount);
            player.UpdateMaxHealth(player.maxHealth);
        }

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

        public void CallEffectSelf(string effectName)
        {
            int positionZVal = -22;
            Vector3 effectPosition = new Vector3(player.transform.position.x-30, player.transform.position.y-90, player.transform.position.z + positionZVal);
            
            if (effectManager != null)
            {
                effectManager.PlayEffect(effectName, effectPosition);
            }
            else
            {
                Debug.LogError("EffectManager가 할당되지 않았습니다. EffectManager 인스턴스를 확인하세요.");
            }
        }


        private void ApplyBuff(Buff.Type t, int amount)
        {
            target.AddBuff(t, amount);

            Enemy enemyInstance = target.GetComponent<Enemy>();
            if (enemyInstance != null)
            {
                enemyInstance.DisplayIntent();
            }
            else
            {
                Debug.LogWarning("타겟이 Enemy 인스턴스가 아닙니다. DisplayIntent()를 호출할 수 없습니다.");
            }
        }

        private void ReduceBuff(Buff.Type t, int amount)
        {
            target.ReductBuff(t, amount);
            Enemy enemyInstance = target.GetComponent<Enemy>();
            if (enemyInstance != null)
            {
                enemyInstance.DisplayIntent();
            }
            else
            {
                Debug.LogWarning("타겟이 Enemy 인스턴스가 아닙니다. DisplayIntent()를 호출할 수 없습니다.");
            }
        }

        private void ApplyBuffToSelf(Buff.Type t)
        {
            player.AddBuff(t, card.GetBuffAmount());

            BattleSceneManager battleSceneManager = FindObjectOfType<BattleSceneManager>();
            if (battleSceneManager != null)
            {
                foreach (Enemy enemyInstance in battleSceneManager.enemies)
                {
                    if (enemyInstance != null)
                    {
                        enemyInstance.DisplayIntent();
                    }
                }
            }
            else
            {
                Debug.LogWarning("BattleSceneManager가 존재하지 않습니다. 적의 의도를 갱신할 수 없습니다.");
            }
        }


        ///대미지 연산 구간///

        private void AttackEnemy()
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. AttackEnemy를 처리할 수 없습니다.");
                return;
            }

            int baseDamage = card.GetCardEffectAmount() + player.strength.buffValue + player.GetPlayerPower();

            List<float> damageModifiers = new List<float>();

            if (player.weak.buffValue > 0)
                damageModifiers.Add(0.7f);

            if (target.vulnerable.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(player.insanity.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(gameManager.PlayerHasRelic("3가지 보석이 박힌 왕관"))
            {
                damageModifiers.Add(1.2f);
            }
            else
            {
                if(gameManager.PlayerHasRelic("하얀 보석이 박힌 단검") && battleSceneManager.normalFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("푸른 보석이 박힌 반지") && battleSceneManager.eliteFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("붉은 보석이 박힌 귀걸이") && battleSceneManager.bossFight)
                {
                    damageModifiers.Add(1.1f);
                }
            }

            int totalDamage = baseDamage;
            foreach (float modifier in damageModifiers)
            {
                totalDamage = (int)(totalDamage * modifier);
            }

            target.TakeDamage(totalDamage);
            EnemyStatsDrain();
            Debug.Log($"최종 대미지: {totalDamage}");
        }


        private void TakeDamageCardAction(int amount)
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. TakeDamageCardAction을 처리할 수 없습니다.");
                return;
            }

            // 대미지 계산에 플레이어의 힘 추가
            amount = amount + player.strength.buffValue + player.GetPlayerPower();

            // 대미지 수정자 목록
            List<float> damageModifiers = new List<float>();

            if (player.weak.buffValue > 0)
                damageModifiers.Add(0.7f);

            if (target.vulnerable.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(player.insanity.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(gameManager.PlayerHasRelic("3가지 보석이 박힌 왕관"))
            {
                damageModifiers.Add(1.2f);
            }
            else
            {
                if(gameManager.PlayerHasRelic("하얀 보석이 박힌 단검") && battleSceneManager.normalFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("푸른 보석이 박힌 반지") && battleSceneManager.eliteFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("붉은 보석이 박힌 귀걸이") && battleSceneManager.bossFight)
                {
                    damageModifiers.Add(1.1f);
                }
            }

            // 대미지 수정자 적용
            foreach (float modifier in damageModifiers)
            {
                amount = (int)(amount * modifier);
            }

            // 대미지 적용
            target.TakeDamage(amount);
            EnemyStatsDrain();
        }

        private int TakeDamageCardActionReturn(int amount)
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. TakeDamageCardActionReturn을 처리할 수 없습니다.");
                return 0;
            }

            amount = amount + player.strength.buffValue + player.GetPlayerPower();

            // 대미지 수정자 목록
            List<float> damageModifiers = new List<float>();

            if (player.weak.buffValue > 0)
                damageModifiers.Add(0.7f);

            if (target.vulnerable.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(player.insanity.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(gameManager.PlayerHasRelic("3가지 보석이 박힌 왕관"))
            {
                damageModifiers.Add(1.2f);
            }
            else
            {
                if(gameManager.PlayerHasRelic("하얀 보석이 박힌 단검") && battleSceneManager.normalFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("푸른 보석이 박힌 반지") && battleSceneManager.eliteFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("붉은 보석이 박힌 귀걸이") && battleSceneManager.bossFight)
                {
                    damageModifiers.Add(1.1f);
                }
            }

            foreach (float modifier in damageModifiers)
            {
                amount = (int)(amount * modifier);
            }

            target.TakeDamage(amount);
            EnemyStatsDrain();
            return amount;
        }

        private int AttackEnemyReturn()
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. AttackEnemyReturn을 처리할 수 없습니다.");
                return 0;
            }
            int totalDamage = card.GetCardEffectAmount() + player.strength.buffValue + player.GetPlayerPower();

            List<float> damageModifiers = new List<float>();

            if (player.weak.buffValue > 0)
                damageModifiers.Add(0.7f);

            if (target.vulnerable.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(player.insanity.buffValue > 0)
                damageModifiers.Add(1.3f);

            if(gameManager.PlayerHasRelic("3가지 보석이 박힌 왕관"))
            {
                damageModifiers.Add(1.2f);
            }
            else
            {
                if(gameManager.PlayerHasRelic("하얀 보석이 박힌 단검") && battleSceneManager.normalFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("푸른 보석이 박힌 반지") && battleSceneManager.eliteFight)
                {
                    damageModifiers.Add(1.1f);
                }

                if(gameManager.PlayerHasRelic("붉은 보석이 박힌 귀걸이") && battleSceneManager.bossFight)
                {
                    damageModifiers.Add(1.1f);
                }
            }

            foreach (float modifier in damageModifiers)
            {
                totalDamage = (int)(totalDamage * modifier);
            }

            int tempnum = target.TakeDamageReturn(totalDamage);
            EnemyStatsDrain();
            return tempnum;
            
        }

        private void TrueAttackEnemy()
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 이미 파괴되었습니다. TrueAttackEnemy를 처리할 수 없습니다.");
                return;
            }

            int baseDamage = card.GetCardEffectAmount() + player.strength.buffValue + player.GetPlayerPower();
            
            target.TrueDamage(baseDamage);
            EnemyStatsDrain();
        }

        private void EnemyStatsDrain()
        {

            if(target.currentHealth <= 0 && playerDrainEffectOnOff && gameManager.PlayerHasRelic("빛나는 보석이 박힌 검"))
            {
                gameManager.DrainEnemyStats(3, 3, 3, 5);
                player.HealEntity(gameManager.playerMaxHealth/10);
                gameManager.currentTendencyValue += 3;
            }
            else if (target.currentHealth <= 0 && playerDrainEffectOnOff && gameManager.PlayerHasRelic("빛이 새어나오는 보석"))
            {
                if (battleSceneManager.bossFight)
                {
                    gameManager.DrainEnemyStats(3, 3, 3, 10);
                }
                else if (battleSceneManager.eliteFight)
                {
                    gameManager.DrainEnemyStats(2, 2, 2, 5);
                }
                else
                {
                    int randomIndex = Random.Range(0, 3);

                    int first = 0;
                    int second = 0;
                    int third = 0;

                    if (randomIndex == 0)
                    {
                        first = 1;
                    }
                    else if (randomIndex == 1)
                    {
                        second = 1;
                    }
                    else
                    {
                        third = 1;
                    }
                    gameManager.DrainEnemyStats(first, second, third, 2);
                    Debug.Log(randomIndex);
                }
                playerDrainEffectOnOff = false;
                gameManager.currentTendencyValue += 3;
            }
            player.UpdateMaxHealth(GameManager.Instance.playerMaxHealth);
            player.UpdateHealthUI(GameManager.Instance.playerCurrentHealth);
        }

        private int PlayerStatsCalculation(int amount)
        {
            int playerPower = player.playerPower;
            amount = amount + playerPower/2;

            return amount;
        }

        private void Defend()
        {
            int blockAmount = card.GetCardEffectAmount();
            
            player.AddBlock(blockAmount+player.solidity.buffValue);
        }

    }
}
