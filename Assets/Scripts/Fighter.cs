using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

namespace TJ
{
    public class Fighter : MonoBehaviour
    {
        private CameraController cameraController;
        [Header("Enemy Data")]
        public EnemyUI enemyUI;
        [HideInInspector]public string enemyNameUI;
        [HideInInspector]public int maxHealthUI;
        [HideInInspector]public int baseStrengthUI;
        [HideInInspector]public int baseAmorUI;
        [HideInInspector]public int currentBlockUI;

        [HideInInspector]public int currentHealth;
        [HideInInspector]public int maxHealth;
        [HideInInspector]public int currentBlock = 0;
        public int additionalCritical=0;

        public FighterHealthBar fighterHealthBar;

        [Header("Buffs")]
        public Buff vulnerable;
        public Buff weak;
        public Buff strength;
        public Buff ritual;
        public Buff enrage;
        public Buff poison;
        public Buff bleeding;
        public Buff deathMark;
        public Buff infectedBleeding;
        public Buff excessiveBleeding;
        public Buff thornArmor;
        public Buff adrenaline;
        public Buff goddessBlessing;
        public Buff insanity;
        public Buff defenseStance;
        public Buff confusion;
        public Buff solidity;

        public GameObject buffPrefab;
        public Transform buffParent;
        public bool isPlayer;
        private Player player;
        Enemy enemy;
        BattleSceneManager battleSceneManager;
        GameManager gameManager;
        CardActions cardActions;

        public GameObject damageIndicator;
        public GameObject healIndicator;
        public GameObject trueDamageIndicator;
        public GameObject criticalDamageIndicator;
        public GameObject armorDamageIndicator;
        int infectedBleedingDamage = 1;

        public FighterEffectManager fighterEffectManager;

        public List<Buff> buffs;


        [Header("Animator")]
        private Animator animator;
        private Animator enemyAnimator;

        public GameObject enemyIcon;
        public GameObject playerIcon;

        public int playerPower, playerAmor, playerLuck;

        [Header("Enemy Stats")]
        private static int enemyPower_fighter;
        private int enemyAmor;

        [HideInInspector]public bool insanityJudge;

        public int CriticPer;

        private void Awake()
        {   
            gameManager = FindObjectOfType<GameManager>();
            insanityJudge = false;
        }

        private void Start()
        {
            cameraController = FindObjectOfType<CameraController>();
            
            if (isPlayer)
            {
                maxHealth = GameManager.Instance.playerMaxHealth;
                currentHealth = maxHealth;
                Debug.Log($"플레이어의 초기 체력이 설정되었습니다: {maxHealth}");
            }
            else
            {
                InitializeStatsFromEnemyUI_Fighter();
                maxHealth = maxHealthUI;
                currentHealth = maxHealthUI;
                
                Debug.Log($"적의 최대 체력이 설정되었습니다: {maxHealth}");
            }
            fighterHealthBar.healthSlider.maxValue = maxHealth;
            fighterHealthBar.DisplayHealth(currentHealth);
            
            animator = playerIcon.GetComponent<Animator>();
            enemyAnimator = enemyIcon.GetComponent<Animator>();

            cardActions = FindObjectOfType<CardActions>();

            enemy = GetComponent<Enemy>();
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            
            if (isPlayer)
            {
                GameManager.Instance.DisplayHealth(currentHealth, maxHealth);
            }

            fighterHealthBar.healthSlider.maxValue = maxHealth;
            fighterHealthBar.DisplayHealth(currentHealth);
            InitializePlayerStatus();
        }

        public int ReturnCriticalPer()
        {
            CriticPer = 10 + playerLuck;

            if(gameManager.PlayerHasRelic("용 조각상"))
            {
                CriticPer += 20;
            }
            else if(gameManager.PlayerHasRelic("십자 조준선"))
            {
                CriticPer += 15;
            }

            if(CriticPer >= 100)
            {
                CriticPer = 100;
            }

            return CriticPer;
        }

        public int GetEnemyPower()
        {
            return enemyPower_fighter;
        }

        private void Update()
        {
            if(player)
            {
                UpdateMaxHealth(GameManager.Instance.playerMaxHealth);
                UpdateHealthUI(GameManager.Instance.playerCurrentHealth);
            }
            
        }

        private void InitializePlayerStatus()
        {
            playerPower = gameManager.playerPower;
            playerAmor = gameManager.playerAmor;
            playerLuck = gameManager.playerLuck;
        }

        public int GetPlayerPower()
        {
            InitializePlayerStatus();
            return playerPower;
        }

        public int GetPlayerAmor()
        {
            InitializePlayerStatus();
            return playerAmor;
        }

        public int GetPlayerLuck()
        {
            InitializePlayerStatus();
            return playerLuck;
        }

        private void InitializeStatsFromEnemyUI_Fighter()
        {
            if (enemyUI == null)
            {
                Debug.LogError("enemyUI가 null입니다. EnemyUI가 할당되지 않았습니다.");
                return;
            }

            enemyNameUI = enemyUI.enemyInfo.enemyNameUI;
            maxHealthUI = enemyUI.enemyInfo.enemyMaxHealthUI;
            enemyPower_fighter = enemyUI.enemyInfo.enemyBaseStrengthUI;
            enemyAmor = enemyUI.enemyInfo.enemyBaseAmorUI;
            currentBlockUI = enemyUI.enemyInfo.enemyBaseBlockUI;
        }

        private void CallEnemyStats()
        {
            cardActions.CardActionEnemyPower = enemyPower_fighter;
            cardActions.CardActionEnemyAmor = enemyAmor;
            cardActions.CardActionEnemyMaxhealth = maxHealthUI;
        }

        public void clear_monster()
        {
            if (currentHealth <= 0)
            {
                if (enemy != null)
                    battleSceneManager.EndFight(true);
                else
                    battleSceneManager.EndFight(false);

                Destroy(gameObject);
            }

            ResetCamera();
        }

        public void HealEntity(int amount)
        {
            if(gameManager.PlayerHasRelic("작은 요정"))
            {
                amount = (int)(amount * 1.3f);
            }

            currentHealth += amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            HealIndicator hi = Instantiate(healIndicator, this.transform).GetComponent<HealIndicator>();
            hi.DisplayHeal(amount);
            Destroy(hi.gameObject, 2f);

            UpdateHealthUI(currentHealth);
        }

        public int TakeDamageReturn(int amount)
        {
            if(gameManager.PlayerHasRelic("용 조각상"))
            {
                additionalCritical += 20;
            }
            else if(gameManager.PlayerHasRelic("십자 조준선"))
            {
                additionalCritical += 15;
            }
            float criticalChance = 10 + additionalCritical + playerLuck;
            bool isCriticalHit = !isPlayer && Random.Range(0f, 100f) <criticalChance ;

            if (isCriticalHit)
            {
                amount = (int)(amount * 1.5f);
            }

            if (currentBlock > 0)
            {
                if (currentBlock >= amount)
                {
                    currentBlock -= amount;

                    DamageIndicator adi = Instantiate(armorDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    adi.DisplayArmorDamage(amount);
                    Destroy(adi.gameObject, 2f);

                    amount = 0;
                }
                else
                {
                    DamageIndicator adi = Instantiate(armorDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    adi.DisplayArmorDamage(currentBlock);
                    Destroy(adi.gameObject, 2f);

                    amount -= currentBlock;
                    currentBlock = 0;
                }

                fighterHealthBar.DisplayBlock(currentBlock);
            }

            if (amount > 0)
            {
                if (isCriticalHit)
                {
                    DamageIndicator cdi = Instantiate(criticalDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    cdi.DisplayCriticalDamage(amount);
                    Destroy(cdi.gameObject, 2f);
                }
                else
                {
                    DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                    di.DisplayDamage(amount);
                    Destroy(di.gameObject, 2f);
                }

                currentHealth -= amount;
                if (currentHealth < 0)
                {
                    currentHealth = 0;
                }

                UpdateHealthUI(currentHealth);

                if (!isPlayer)
                {
                    enemyAnimator.Play("Attacked");
                }
                else
                {
                    animator.Play("PlayerAttacked");
                }
            }

            if (isPlayer)
            {
                int newMaxHealth = GameManager.Instance.playerMaxHealth;
                UpdateMaxHealth(newMaxHealth);
            }

            Debug.Log($"Dealt {amount} damage");

            Vector3 targetPosition;
            if (transform.position.x <= -100)
            {
                targetPosition = new Vector3(transform.position.x + 120, transform.position.y, transform.position.z - 22);
            }
            else if (transform.position.x >= 100)
            {
                targetPosition = new Vector3(transform.position.x - 120, transform.position.y+100, transform.position.z - 22);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 22);
            }

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
            return amount;
        }


        public void TrueDamage(int amount)
        {

            DamageIndicator tdi = Instantiate(trueDamageIndicator, this.transform).GetComponent<DamageIndicator>();
            tdi.DisplayDamage(amount);
            Destroy(tdi.gameObject, 2f);

            enemyAnimator.Play("Attacked");

            Debug.Log($"dealt {amount} damage");

            currentHealth -= amount;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            UpdateHealthUI(currentHealth);

            Vector3 targetPosition;
            if (transform.position.x <= -100)
            {
                targetPosition = new Vector3(transform.position.x + 120, transform.position.y, transform.position.z - 22);
            }
            else if (transform.position.x >= 100)
            {
                targetPosition = new Vector3(transform.position.x - 120, transform.position.y+100, transform.position.z - 22);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 22);
            }

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
        }

        public void TakeBuffDamage(int amount)
        {
            {
                if (!isPlayer)
                {
                    enemyAnimator.Play("Attacked");
                }
                else
                {
                    animator.Play("PlayerAttacked");
                }
            }

            DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
            di.DisplayDamage(amount);
            Destroy(di, 2f);

            currentHealth -= amount;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            UpdateHealthUI(currentHealth);

            Vector3 targetPosition;

            if (transform.position.x <= -100)
            {
                targetPosition = new Vector3(transform.position.x + 120, transform.position.y, transform.position.z - 22);
            }
            else if (transform.position.x >= 100)
            {
                targetPosition = new Vector3(transform.position.x - 120, transform.position.y+100, transform.position.z - 22);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 22);
            }

            Debug.Log($"Target camera position for zoom: {targetPosition}");

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
        }


        public void TakeDamage(int amount)
        {
            if(gameManager.PlayerHasRelic("용 조각상"))
            {
                additionalCritical += 20;
            }
            else if(gameManager.PlayerHasRelic("십자 조준선"))
            {
                additionalCritical += 15;
            }
            float criticalChance = 10 + additionalCritical + playerLuck;
            bool isCriticalHit = !isPlayer && Random.Range(0f, 100f) <criticalChance ;

            if (isCriticalHit)
            {
                if(gameManager.PlayerHasRelic("용 조각상"))
                {
                    amount = (int)(amount * 2.0f);
                }
                else if(gameManager.PlayerHasRelic("달인의 반지"))
                {
                    amount = (int)(amount * 1.75f);
                }
                else
                {
                    amount = (int)(amount * 1.5f); // 크리티컬 대미지
                }
            }

            // 방어력이 있는 경우 대미지를 감소시키는 로직
            if (currentBlock > 0)
            {
                if (currentBlock >= amount) // 방어력이 충분할 경우
                {
                    currentBlock -= amount; // 방어력을 감소시킴

                    // 방어력 감소 인디케이터 활성화
                    DamageIndicator adi = Instantiate(armorDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    adi.DisplayArmorDamage(amount);
                    Destroy(adi.gameObject, 2f);

                    amount = 0; // 모든 피해가 방어력에 의해 막힘
                }
                else // 방어력이 충분하지 않을 경우
                {
                    // 방어력 감소 인디케이터 활성화
                    DamageIndicator adi = Instantiate(armorDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    adi.DisplayArmorDamage(currentBlock);
                    Destroy(adi.gameObject, 2f);

                    amount -= currentBlock; // 남은 피해량 계산
                    currentBlock = 0; // 방어력 소모
                }

                fighterHealthBar.DisplayBlock(currentBlock); // UI 갱신
            }

            if (amount > 0) // 남은 대미지가 있는 경우
            {
                if (isCriticalHit)
                {
                    // 크리티컬 인디케이터 활성화
                    DamageIndicator cdi = Instantiate(criticalDamageIndicator, this.transform).GetComponent<DamageIndicator>();
                    cdi.DisplayCriticalDamage(amount);
                    Destroy(cdi.gameObject, 2f);
                }
                else
                {
                    // 일반 대미지 인디케이터 활성화
                    DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                    di.DisplayDamage(amount);
                    Destroy(di.gameObject, 2f);
                }

                currentHealth -= amount; // 체력 감소
                if (currentHealth < 0)
                {
                    currentHealth = 0;
                }

                UpdateHealthUI(currentHealth); // 체력 UI 갱신

                // 대미지 애니메이션 실행
                if (!isPlayer)
                {
                    enemyAnimator.Play("Attacked");
                }
                else
                {
                    animator.Play("PlayerAttacked");
                }
            }

            if (isPlayer)
            {
                int newMaxHealth = GameManager.Instance.playerMaxHealth;
                UpdateMaxHealth(newMaxHealth);
            }

            Debug.Log($"Dealt {amount} damage");

            // 카메라 이동 관련 로직
            Vector3 targetPosition;
            if (transform.position.x <= -100)
            {
                targetPosition = new Vector3(transform.position.x + 120, transform.position.y+50, transform.position.z - 22);
            }
            else if (transform.position.x >= 100)
            {
                targetPosition = new Vector3(transform.position.x - 120, transform.position.y+100, transform.position.z - 22);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y+50, transform.position.z - 22);
            }

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
        }

        private void ResetCamera()
        {
            cameraController.ZoomOut(0.2f);  // 원래 위치로 돌아가기 (지속 시간 설정)
        }

        public IEnumerator HandleDamageAndCamera(Vector3 targetPosition, int amount)
        {
            // 플레이어의 경우 체력이 0일 때 카메라 확대를 방지
            if (isPlayer && currentHealth == 0)
            {
                yield break; // 코루틴 종료
            }

            cameraController.ZoomInTo(targetPosition, 330.0f, 0.1f);

            yield return new WaitForSeconds(0.1f);

            if (currentHealth == 0)
            {
                yield return new WaitForSeconds(1.3f); // 1.3초 동안 카메라 확대 유지

                yield return new WaitForSeconds(0.7f);

                ResetCamera();
                clear_monster();
            }
            else
            {
                Invoke("ResetCamera", 1.3f);
            }
        }


        private int BlockDamage(int amount)
        {
            if (currentBlock > 0)
            {
                if (currentBlock >= amount) // 방어력이 충분할 경우
                {
                    currentBlock -= amount; // 방어력을 감소시킴
                    amount = 0; // 모든 피해가 방어력에 의해 막힘
                }
                else // 방어력이 충분하지 않을 경우
                {
                    amount -= currentBlock; // 남은 피해량 계산
                    currentBlock = 0; // 방어력 소모
                }

                fighterHealthBar.DisplayBlock(currentBlock); // UI 갱신
            }

            return amount; // 남은 피해량 반환
        }

        public void CallBuffEffect(string effectName)
        {

            Debug.Log($" 이펙트를 호출한 넘은 Effect Caller: {this.name}");
            int tempval_x = 0;
            int tempval_y = 0;
            if(effectName == "Magic_Effect_addition/Buff_02a" || effectName == "Magic_Effect/CFXR3 Magic Aura A (Runic)")
            {
                tempval_x = -30;
                tempval_y = -190;
            }

            Vector3 effectPosition = new Vector3(transform.position.x+tempval_x, transform.position.y+tempval_y, transform.position.z - 10);
            Debug.Log($"Effect Position: {effectPosition}");
            fighterEffectManager.PlayFighterEffect(effectName, effectPosition);
        }

        //////////////////////////////////////체력 설정//////////////////////////////////////////////

        public void UpdateMaxHealth(int newMaxHealth)
        {
            maxHealth = newMaxHealth;

            if (fighterHealthBar != null)
            {
                fighterHealthBar.healthSlider.maxValue = maxHealth;
                fighterHealthBar.DisplayHealth(currentHealth);
            }

            if (isPlayer)
            {
                GameManager.Instance.playerMaxHealth = maxHealth;
            }

            fighterHealthBar.DisplayHealth(currentHealth);
            UpdateHealthUI(currentHealth);
        }

        public void UpdateHealthUI(int newAmount)
        {
            currentHealth = newAmount;

            if (fighterHealthBar != null)
            {
                fighterHealthBar.healthSlider.maxValue = maxHealth;
                fighterHealthBar.DisplayHealth(currentHealth);
            }

            if (isPlayer)
            {
                GameManager.Instance.DisplayHealth(currentHealth, maxHealth);
            }
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////
    

        


// cameraController.RotateCamera(0.5f, 10f);                 // 동시에 10도 회전
// cameraController.ShakeCamera(0.4f, 0.5f);                 // 0.4초 동안 약간의 진동 추가

        public void execution(Fighter enemy)
        {
            currentHealth = 1;
            Debug.Log($"Execution successful! {enemy.name}'s health is now {enemy.currentHealth}");
            UpdateHealthUI(currentHealth);
        }

        public void RemoveBuff(Buff buff)
        {
            buffs.Remove(buff);
            if (buff.buffGO != null)
            {
                Destroy(buff.buffGO.gameObject);
            }
        }

        public void AddBlock(int amount)
        {
            currentBlock = currentBlock + amount;
            fighterHealthBar.DisplayBlock(currentBlock);
        }

        private void Die()
        {
            this.gameObject.SetActive(false);
        }

        

        public void AddBuff(Buff.Type type, int amount) // 버프 추가
        {
            switch (type)
            {

                case Buff.Type.enrage: //피해 증가
                    if (enrage.buffValue <= 0) // 새로운 버프 객체가 필요할 때만 생성
                    {
                        enrage.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    CallBuffEffect("Magic_Effect_addition/Buff_02a");
                    enrage.buffValue += amount;
                    enrage.buffGO.DisplayBuff(enrage);
                    Debug.Log($"격노 버프 적용: {enrage.buffValue}");
                    break;

                case Buff.Type.vulnerable: //받는 피해 증가
                    if (vulnerable.buffValue <= 0) // 새로운 버프 객체가 필요할 때만 생성
                    {
                        vulnerable.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    CallBuffEffect("Magic_Effect_addition/Buff_02aDown");
                    vulnerable.buffValue += amount;
                    vulnerable.buffGO.DisplayBuff(vulnerable);
                    Debug.Log($"취약 버프 적용: {vulnerable.buffValue}");
                    break;

                case Buff.Type.weak: //주는 피해 감소
                    if (weak.buffValue <= 0)
                    {
                        weak.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    CallBuffEffect("Magic_Effect_addition/Buff_02aDown");
                    weak.buffValue += amount;
                    weak.buffGO.DisplayBuff(weak);
                    Debug.Log($"약화 버프 적용: {weak.buffValue}");
                    break;

                case Buff.Type.strength:
                    if (strength.buffGO == null)
                    {
                        strength.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }

                    if (amount > 0)
                    {
                        CallBuffEffect("Magic_Effect_addition/Buff_02a");
                    }
                    else if (amount < 0)
                    {
                        CallBuffEffect("Magic_Effect_addition/Buff_02aDown");
                    }

                    strength.buffValue += amount;

                    if (strength.buffValue == 0)
                    {
                        Destroy(strength.buffGO.gameObject);
                        strength.buffGO = null;
                        Debug.Log("힘 버프가 제거되었습니다.");
                    }
                    else
                    {
                        strength.buffGO.DisplayBuff(strength);
                        Debug.Log($"힘 버프 적용: {strength.buffValue}");
                    }
                    break;

                case Buff.Type.poison: //독 피해
                    if (poison.buffValue <= 0)
                    {
                        poison.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }

                    CallBuffEffect("Misc_Effect/CFXR2 Poison Cloud");

                    if(gameManager.PlayerHasRelic("깨진 플라스크"))
                    {
                        amount +=1;
                    }

                    poison.buffValue += amount;
                    poison.buffGO.DisplayBuff(poison);
                    Debug.Log($"독 버프 적용: {poison.buffValue}");
                    break;

                case Buff.Type.bleeding: //출혈 피해
                    if (bleeding.buffValue <= 0)
                    {
                        bleeding.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }

                    if(gameManager.PlayerHasRelic("단검의 파편"))
                    {
                        amount +=1;
                    }

                    CallBuffEffect("Liquids_Effect/CFXR2 Blood Shape Splash");

                    bleeding.buffValue += amount;
                    bleeding.buffGO.DisplayBuff(bleeding);
                    Debug.Log($"출혈 버프 적용: {bleeding.buffValue}");
                    break;

                case Buff.Type.deathMark: //데스마크
                    if (deathMark.buffValue <= 0)
                    {
                        deathMark.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    
                    CallBuffEffect("temp/CFX_Virus");

                    deathMark.buffValue += amount;
                    deathMark.buffGO.DisplayBuff(deathMark);
                    Debug.Log($"데스마크 버프 적용: {deathMark.buffValue}");
                    break;

                case Buff.Type.goddessBlessing: //여신의 가호
                    if (goddessBlessing.buffValue <= 0)
                    {
                        goddessBlessing.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }

                    CallBuffEffect("Magic_Effect/CFXR3 Magic Aura A (Runic)");
                    goddessBlessing.buffValue += amount;
                    goddessBlessing.buffGO.DisplayBuff(goddessBlessing);
                    Debug.Log($"여신의 가호 버프 적용: {goddessBlessing.buffValue} 중첩");
                    break;

                case Buff.Type.insanity: //광기
                    if (insanity.buffValue <= 0)
                    {
                        insanity.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    insanity.buffValue += amount;
                    insanity.buffGO.DisplayBuff(insanity);
                    Debug.Log($"광서커 온 버프 적용: {insanity.buffValue} 중첩");
                    break;
                
                case Buff.Type.thornArmor: //가시 갑옷
                    if (thornArmor.buffValue <= 0)
                    {
                        thornArmor.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    thornArmor.buffValue += amount;
                    thornArmor.buffGO.DisplayBuff(thornArmor);
                    Debug.Log($"출혈 가시 버프 적용: {thornArmor.buffValue} 중첩");
                    break;

                case Buff.Type.adrenaline: //아드레날린
                    if (adrenaline.buffValue <= 0)
                    {
                        adrenaline.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    adrenaline.buffValue += amount;
                    adrenaline.buffGO.DisplayBuff(adrenaline);
                    Debug.Log($"아드레날린 버프 적용: {adrenaline.buffValue} 중첩");
                    break;

                case Buff.Type.infectedBleeding: //감염된 출혈
                    if (infectedBleeding.buffValue <= 0)
                    {
                        infectedBleeding.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    infectedBleeding.buffValue += amount;
                    infectedBleeding.buffGO.DisplayBuff(infectedBleeding);
                    Debug.Log($"감염된 출혈 디버프 적용: {infectedBleeding.buffValue}");
                    break;
                
                case Buff.Type.excessiveBleeding: //과다 출혈
                    if (excessiveBleeding.buffValue <= 0)
                    {
                        excessiveBleeding.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    excessiveBleeding.buffValue += amount;
                    excessiveBleeding.buffGO.DisplayBuff(excessiveBleeding);
                    Debug.Log($"과다 출혈 디버프 적용: {excessiveBleeding.buffValue}");
                    break;

                case Buff.Type.defenseStance:
                    if (defenseStance.buffValue <= 0)
                    {
                        defenseStance.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    defenseStance.buffValue += amount;
                    defenseStance.buffGO.DisplayBuff(defenseStance);
                    Debug.Log($"광서커 온 버프 적용: {defenseStance.buffValue} 중첩");
                    break;

                //견고함
                case Buff.Type.solidity:
                    if (solidity.buffValue <= 0)
                    {
                        solidity.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    CallBuffEffect("Magic_Effect_addition/Buff_02a");
                    solidity.buffValue += amount;
                    solidity.buffGO.DisplayBuff(solidity);
                    Debug.Log($"견고함 버프 적용: {solidity.buffValue} 중첩");
                    break;

                default:
                    Debug.LogWarning("알 수 없는 버프 타입입니다.");
                    break;
            }
            if (enemy != null)
            {
                enemy.DisplayIntent();
            }
        }

        public void ReductBuff(Buff.Type type, int amount)
        {
            switch (type)
            {
                case Buff.Type.enrage:
                    if (enrage.buffValue > 0)
                    {
                        enrage.buffValue -= amount;
                        enrage.buffGO.DisplayBuff(enrage);
                        Debug.Log($"격노 버프 감소: {enrage.buffValue}");

                        if (enrage.buffValue <= 0)
                        {
                            Destroy(enrage.buffGO.gameObject);
                            enrage.buffGO = null;
                            Debug.Log("격노 버프 제거됨");
                        }
                    }

                    break;

                case Buff.Type.vulnerable:
                    if (vulnerable.buffValue > 0)
                    {
                        vulnerable.buffValue -= amount;
                        vulnerable.buffGO.DisplayBuff(vulnerable);
                        Debug.Log($"취약 버프 감소: {vulnerable.buffValue}");

                        if (vulnerable.buffValue <= 0)
                        {
                            Destroy(vulnerable.buffGO.gameObject);
                            vulnerable.buffGO = null;
                            Debug.Log("취약 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.weak:
                    if (weak.buffValue > 0)
                    {
                        weak.buffValue -= amount;
                        weak.buffGO.DisplayBuff(weak);
                        Debug.Log($"약화 버프 감소: {weak.buffValue}");

                        if (weak.buffValue <= 0)
                        {
                            Destroy(weak.buffGO.gameObject);
                            weak.buffGO = null;
                            Debug.Log("약화 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.strength:
                    if (strength.buffValue > 0)
                    {
                        strength.buffValue -= amount;
                        strength.buffGO.DisplayBuff(strength);
                        Debug.Log($"힘 버프 감소: {strength.buffValue}");

                        if (strength.buffValue <= 0)
                        {
                            Destroy(strength.buffGO.gameObject);
                            strength.buffGO = null;
                            Debug.Log("힘 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.poison:
                    if (poison.buffValue > 0)
                    {
                        poison.buffValue -= amount;
                        poison.buffGO.DisplayBuff(poison);
                        Debug.Log($"독 버프 감소: {poison.buffValue}");

                        if (poison.buffValue <= 0)
                        {
                            Destroy(poison.buffGO.gameObject);
                            poison.buffGO = null;
                            Debug.Log("독 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.bleeding:
                    if (bleeding.buffValue > 0)
                    {
                        bleeding.buffValue -= amount;
                        bleeding.buffGO.DisplayBuff(bleeding);
                        Debug.Log($"출혈 버프 감소: {bleeding.buffValue}");

                        if (bleeding.buffValue <= 0)
                        {
                            Destroy(bleeding.buffGO.gameObject);
                            bleeding.buffGO = null;
                            Debug.Log("출혈 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.deathMark:
                    if (deathMark.buffValue > 0)
                    {
                        deathMark.buffValue -= amount;
                        deathMark.buffGO.DisplayBuff(deathMark);
                        Debug.Log($"데스마크 버프 감소: {deathMark.buffValue}");

                        if (deathMark.buffValue <= 0)
                        {
                            Destroy(deathMark.buffGO.gameObject);
                            deathMark.buffGO = null;
                            Debug.Log("데스마크 버프 제거됨");
                        }
                    }
                    break;

                case Buff.Type.goddessBlessing:
                    if (goddessBlessing.buffValue > 0)
                    {
                        goddessBlessing.buffValue -= amount;
                        goddessBlessing.buffGO.DisplayBuff(goddessBlessing);
                        Debug.Log($"여신의 가호 감소: {goddessBlessing.buffValue}");

                        if (goddessBlessing.buffValue <= 0)
                        {
                            Destroy(goddessBlessing.buffGO.gameObject);
                            goddessBlessing.buffGO = null;
                            Debug.Log("여신의 가호 제거됨");
                        }
                    }
                    break;

                case Buff.Type.insanity:
                    if (insanity.buffValue > 0)
                    {
                        insanity.buffValue -= amount;
                        insanity.buffGO.DisplayBuff(insanity);

                        if (insanity.buffValue <= 0)
                        {
                            Destroy(insanity.buffGO.gameObject);
                            insanity.buffGO = null;
                        }
                    }
                    break;
                
                case Buff.Type.defenseStance:
                    if (defenseStance.buffValue > 0)
                    {
                        defenseStance.buffValue -= amount;
                        defenseStance.buffGO.DisplayBuff(defenseStance);

                        if (defenseStance.buffValue <= 0)
                        {
                            Destroy(defenseStance.buffGO.gameObject);
                            defenseStance.buffGO = null;
                        }
                    }
                    break;

                case Buff.Type.thornArmor:
                    if (thornArmor.buffValue > 0)
                    {
                        thornArmor.buffValue -= amount;
                        thornArmor.buffGO.DisplayBuff(thornArmor);

                        if (thornArmor.buffValue <= 0)
                        {
                            Destroy(thornArmor.buffGO.gameObject);
                            thornArmor.buffGO = null;
                        }
                    }
                    break;
                
                case Buff.Type.adrenaline:
                    if (adrenaline.buffValue > 0)
                    {
                        adrenaline.buffValue -= amount;
                        adrenaline.buffGO.DisplayBuff(adrenaline);

                        if (adrenaline.buffValue <= 0)
                        {
                            Destroy(adrenaline.buffGO.gameObject);
                            adrenaline.buffGO = null;
                        }
                    }
                    break;


                case Buff.Type.infectedBleeding:
                    if (infectedBleeding.buffValue > 0 )
                    {
                        infectedBleeding.buffValue -= amount;
                        infectedBleeding.buffGO.DisplayBuff(infectedBleeding);
                        Debug.Log($"감염된 출혈 감소: {infectedBleeding.buffValue}");

                        if (infectedBleeding.buffValue <= 0)
                        {
                            Destroy(infectedBleeding.buffGO.gameObject);
                            infectedBleeding.buffGO = null;
                            Debug.Log("감염된 출혈 제거됨");
                        }
                    }
                    break;
                
                case Buff.Type.solidity:
                    if(solidity.buffValue > 0)
                    {
                        solidity.buffValue -= amount;
                        solidity.buffGO.DisplayBuff(solidity);
                        Debug.Log($"견고함 감소: {solidity.buffValue}");

                        if (solidity.buffValue <= 0)
                        {
                            Destroy(solidity.buffGO.gameObject);
                            solidity.buffGO = null;
                            Debug.Log("견고함 제거됨");
                        }
                    }
                    break;

                default:
                    Debug.LogWarning("알 수 없는 버프 타입입니다.");
                    break;
            }

            if (enemy != null)
            {
                enemy.DisplayIntent();
            }
        }

        public void EvaluateBuffsAtTurnEnd()
        {

            if (enrage.buffValue > 0) // Rage 처리
            {
                AddBuff(Buff.Type.strength, enrage.buffValue);
                if (enrage.buffGO != null)
                {
                    enrage.buffGO.DisplayBuff(enrage);
                }

                Debug.Log($"격노 발동! 힘이 {enrage.buffValue}만큼 증가했습니다.");
            }

            if (vulnerable.buffValue > 0) //취약
            {
                vulnerable.buffValue -= 1;
                if (vulnerable.buffGO != null)
                {
                    vulnerable.buffGO.DisplayBuff(vulnerable);
                }

                if (vulnerable.buffValue <= 0)
                {
                    if (vulnerable.buffGO != null)
                    {
                        Destroy(vulnerable.buffGO.gameObject);
                    }
                }
            }

            if (weak.buffValue > 0) //약화
            {
                weak.buffValue -= 1;
                if (weak.buffGO != null)
                {
                    weak.buffGO.DisplayBuff(weak);
                }

                if (weak.buffValue <= 0)
                {
                    if (weak.buffGO != null)
                    {
                        Destroy(weak.buffGO.gameObject);
                    }
                }
            }

            if (ritual.buffValue > 0) 
            {
                AddBuff(Buff.Type.strength, ritual.buffValue);
            }

            if (poison.buffValue > 0) //독
            {
                TakeBuffDamage(poison.buffValue);
                poison.buffValue -= 1;
                if (poison.buffGO != null)
                {
                    poison.buffGO.DisplayBuff(poison);
                }

                if (poison.buffValue <= 0)
                {
                    if (poison.buffGO != null)
                    {
                        Destroy(poison.buffGO.gameObject);
                    }
                }
            }

            if (goddessBlessing.buffValue > 0)
            {
                goddessBlessing.buffValue -= 1;
                if (goddessBlessing.buffGO != null)
                {
                    goddessBlessing.buffGO.DisplayBuff(goddessBlessing);
                }

                if (goddessBlessing.buffValue <= 0)
                {
                    if (goddessBlessing.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(goddessBlessing.buffGO.gameObject);
                    }
                }
            }

            // 출혈 버프 처리
            if (bleeding.buffValue > 0)
            {
                TakeBuffDamage(1); // 출혈 대미지
                bleeding.buffValue -= 1;
                if (bleeding.buffGO != null) // BuffGO가 null인지 확인
                {
                    bleeding.buffGO.DisplayBuff(bleeding);
                }

                CallBuffEffect("temp/CFX2_Blood");

                if (bleeding.buffValue <= 0)
                {
                    if (bleeding.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(bleeding.buffGO.gameObject);
                    }
                }
            }

            
            
            if (infectedBleeding.buffValue > 0)
            {
                TakeBuffDamage(infectedBleedingDamage);
                infectedBleeding.buffValue -= 1;
                
                if (infectedBleeding.buffGO != null)
                {
                    infectedBleeding.buffGO.DisplayBuff(infectedBleeding);
                }

                if (infectedBleeding.buffValue <= 0)
                {
                    if (infectedBleeding.buffGO != null)
                    {
                        Destroy(infectedBleeding.buffGO.gameObject);
                    }
                    
                    infectedBleedingDamage = 1;
                }
                else
                {
                    infectedBleedingDamage += 2;
                }
            }


            // 데스마크 처리
            if (deathMark.buffValue >= 5)
            {
                Debug.Log("데스마크 발동!...");
                execution(this);
                if (deathMark.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                {
                    Destroy(deathMark.buffGO.gameObject);
                }
            }

            if (adrenaline.buffValue > 0)
            {
                adrenaline.buffValue = 0;
                Destroy(adrenaline.buffGO.gameObject);
                adrenaline.buffGO = null;
                Debug.Log("아드레날린 버프가 종료되었습니다.");
            }
            if (enemy != null)
            {
                enemy.DisplayIntent();
            }
        }

        public void ResetBuffs()
        {
            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue = 0;
                Destroy(vulnerable.buffGO.gameObject);
            }
            if (weak.buffValue > 0)
            {
                weak.buffValue = 0;
                Destroy(weak.buffGO.gameObject);
            }
            if (strength.buffValue > 0 || strength.buffValue < 0) // 힘 버프 초기화
            {
                strength.buffValue = 0;
                Destroy(strength.buffGO.gameObject);
            }
            if (poison.buffValue > 0) // 독 버프 초기화
            {
                poison.buffValue = 0;
                Destroy(poison.buffGO.gameObject);
            }
            if (bleeding.buffValue > 0) // 출혈 버프 초기화
            {
                bleeding.buffValue = 0;
                Destroy(bleeding.buffGO.gameObject);
            }

            if (goddessBlessing.buffValue > 0)
            {
                goddessBlessing.buffValue = 0;
                Destroy(goddessBlessing.buffGO.gameObject);
            }

            if (insanity.buffValue > 0)
            {
                insanity.buffValue = 0;
                insanityJudge = false;
                Destroy(insanity.buffGO.gameObject);
            }

            if (thornArmor.buffValue > 0)
            {
                thornArmor.buffValue = 0;
                Destroy(thornArmor.buffGO.gameObject);
            }

            if (adrenaline.buffValue > 0)
            {
                adrenaline.buffValue = 0;
                Destroy(adrenaline.buffGO.gameObject);
            }

            if (infectedBleeding.buffValue > 0)
            {
                infectedBleeding.buffValue = 0;
                Destroy(infectedBleeding.buffGO.gameObject);
            }

            if (excessiveBleeding.buffValue > 0)
            {
                excessiveBleeding.buffValue = 0;
                Destroy(excessiveBleeding.buffGO.gameObject);
            }

            if (defenseStance.buffValue > 0)
            {
                defenseStance.buffValue = 0;
                Destroy(defenseStance.buffGO.gameObject);
            }

            if (solidity.buffValue > 0)
            {
                solidity.buffValue = 0;
                Destroy(solidity.buffGO.gameObject);
            }

            if (ritual.buffValue > 0)
            {
                ritual.buffValue = 0;
                Destroy(ritual.buffGO.gameObject);
            }

            if (deathMark.buffValue > 0)
            {
                deathMark.buffValue = 0;
                Destroy(deathMark.buffGO.gameObject);
            }

            if (enrage.buffValue > 0)
            {
                enrage.buffValue = 0;
                Destroy(enrage.buffGO.gameObject);
            }

            // Reset block
            currentBlock = 0;
            fighterHealthBar.DisplayBlock(0);

            cardActions.playerDrainEffectOnOff = true;
            battleSceneManager.ResetFight();
        }

        public void resetDebuffs()
        {
            if (excessiveBleeding.buffValue > 0)
            {
                excessiveBleeding.buffValue = 0;
                Destroy(excessiveBleeding.buffGO.gameObject);
            }

            if (infectedBleeding.buffValue > 0)
            {
                infectedBleeding.buffValue = 0;
                Destroy(infectedBleeding.buffGO.gameObject);
            }

            if (poison.buffValue > 0) // 독 버프 초기화
            {
                poison.buffValue = 0;
                Destroy(poison.buffGO.gameObject);
            }

            if (bleeding.buffValue > 0) // 출혈 버프 초기화
            {
                bleeding.buffValue = 0;
                Destroy(bleeding.buffGO.gameObject);
            }

            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue = 0;
                Destroy(vulnerable.buffGO.gameObject);
            }
            if (weak.buffValue > 0)
            {
                weak.buffValue = 0;
                Destroy(weak.buffGO.gameObject);
            }
            
        }
    }
}
