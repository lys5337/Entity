using System.Collections;
using System.Collections.Generic;
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
        [HideInInspector]public int currentBlockUI;

        [HideInInspector]public int currentHealth;
        [HideInInspector]public int maxHealth;
        [HideInInspector]public int currentBlock = 0;

        public FighterHealthBar fighterHealthBar;

        [Header("Buffs")]
        public Buff vulnerable;
        public Buff weak;
        public Buff strength;
        public Buff ritual;
        public Buff enrage;
        public Buff poison; // 독 버프 추가
        public Buff bleeding; // 출혈 버프 추가
        public Buff deathMark;
        public GameObject buffPrefab;
        public Transform buffParent;
        public bool isPlayer;
        Enemy enemy;
        BattleSceneManager battleSceneManager;
        GameManager gameManager;
        public GameObject damageIndicator;
        public List<Buff> buffs;
        private Animator animator;

        private void Awake()
        {
            cameraController = FindObjectOfType<CameraController>();
            animator = GetComponent<Animator>();
            if (isPlayer)
            {
                maxHealth = 60;
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
        }

        private void Start()
        {
            enemy = GetComponent<Enemy>();
            battleSceneManager = FindObjectOfType<BattleSceneManager>();
            gameManager = FindObjectOfType<GameManager>();

            if (isPlayer)
            {
                gameManager.DisplayHealth(currentHealth, maxHealth);
            }

            fighterHealthBar.healthSlider.maxValue = maxHealth;
            fighterHealthBar.DisplayHealth(currentHealth);
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
            baseStrengthUI = enemyUI.enemyInfo.enemyBaseStrengthUI;
            currentBlockUI = enemyUI.enemyInfo.enemyBaseBlockUI;

            Debug.Log("Fighter Enemy Name: " + enemyNameUI);
            Debug.Log("Fighter Enemy Base Strength: " + baseStrengthUI);
            Debug.Log("Fighter Enemy Max Health: " + maxHealthUI);
            Debug.Log("Fighter Enemy Base Block: " + currentBlockUI);
        }

        public void clear_monster() //Invoke()를 사용하기 위해 제작한 메소드
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

        public void TrueDamage(int amount)
        {
            Debug.Log("Fighter Max Health: " + enemyNameUI);

            if (enemy != null && enemy.wiggler && currentHealth == maxHealth)
                enemy.CurlUP();

            Debug.Log($"dealt {amount} damage");

            if (amount > 0)
            {
                DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                di.DisplayDamage(amount);
                Destroy(di, 2f);
            }
            else
            {
                int Tempamount = -amount;
                DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                di.DisplayDamage(Tempamount);
                Destroy(di, 2f);
            }

            currentHealth -= amount;

            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            UpdateHealthUI(currentHealth);

            Vector3 targetPosition;

            if (transform.position.x <= -100)
            {
                targetPosition = new Vector3(transform.position.x + 500, transform.position.y, transform.position.z - 40);
            }
            else if (transform.position.x >= 100)
            {
                targetPosition = new Vector3(transform.position.x - 500, transform.position.y, transform.position.z - 40);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 40);
            }

            Debug.Log($"Target camera position for zoom: {targetPosition}");

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
        }

        private void ResetCamera()
        {
            cameraController.ZoomOut(0.2f);  // 원래 위치로 돌아가기 (지속 시간 설정)
        }
        

        public void TakeDamage(int amount)
        {
            Debug.Log("Fighter Max Health: " + enemyNameUI);

            if (currentBlock > 0)
                amount = BlockDamage(amount);

            if (enemy != null && enemy.wiggler && currentHealth == maxHealth)
                enemy.CurlUP();

            Debug.Log($"dealt {amount} damage");

            if (amount > 0)
            {
                DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                di.DisplayDamage(amount);
                Destroy(di, 2f);
            }
            else
            {
                int Tempamount = -amount;
                DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
                di.DisplayDamage(Tempamount);
                Destroy(di, 2f);
            }

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
                targetPosition = new Vector3(transform.position.x - 120, transform.position.y, transform.position.z - 22);
            }
            else
            {
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 22);
            }

            Debug.Log($"Target camera position for zoom: {targetPosition}");

            StartCoroutine(HandleDamageAndCamera(targetPosition, amount));
        }

        private IEnumerator HandleDamageAndCamera(Vector3 targetPosition, int amount)
        {
            cameraController.ZoomInTo(targetPosition, 330.0f, 0.1f);

            yield return new WaitForSeconds(0.1f);

            if (currentHealth == 0)
            {
                Debug.Log("적의 체력이 0이 되었습니다. 카메라를 원래 위치로 복귀하고 적 처리.");
                ResetCamera();

                yield return new WaitForSeconds(0.5f);

                clear_monster();
            }
            else
            {
                Invoke("ResetCamera", 1.3f);
            }
        }

// cameraController.RotateCamera(0.5f, 10f);                 // 동시에 10도 회전
// cameraController.ShakeCamera(0.4f, 0.5f);                 // 0.4초 동안 약간의 진동 추가

        public int TakeDamageReturn(int amount)
        {
            Debug.Log("Fighter Max Health: " + enemyNameUI);


            if (currentBlock > 0)
                amount = BlockDamage(amount);

            if (enemy != null && enemy.wiggler && currentHealth == maxHealth) //wigger의 체력이 가득 있을경우(wigger 전용 이벤트)
                enemy.CurlUP();

            Debug.Log($"dealt {amount} damage");

            DamageIndicator di = Instantiate(damageIndicator, this.transform).GetComponent<DamageIndicator>();
            di.DisplayDamage(amount);
            Destroy(di, 2f);

            currentHealth -= amount;
            
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }

            UpdateHealthUI(currentHealth);

            Invoke("clear_monster",2.0f); //Invoke는 대문자임

            return amount;
        }



        public void execution(Fighter enemy)
        {
            currentHealth = 1;
            Debug.Log($"Execution successful! {enemy.name}'s health is now {enemy.currentHealth}");
            UpdateHealthUI(currentHealth); // 체력 UI 갱신
            //deathMark버프 제거

        }

        public void RemoveBuff(Buff buff)
        {
            buffs.Remove(buff); // Buff 리스트에서 제거
            if (buff.buffGO != null)
            {
                Destroy(buff.buffGO.gameObject); // Buff UI 제거
            }
        }

        public void UpdateHealthUI(int newAmount) //newAmount: 변경된 체력량
        {
            currentHealth = newAmount;
            fighterHealthBar.DisplayHealth(newAmount);

            if (isPlayer)
                gameManager.DisplayHealth(newAmount, maxHealth);
        }

        public void AddBlock(int amount)
        {
            currentBlock += amount;
            fighterHealthBar.DisplayBlock(currentBlock);
        }

        private void Die()
        {
            this.gameObject.SetActive(false);
        }

        private int BlockDamage(int amount) //amount: 데미지량
        {
            if (currentBlock >= amount)
            {
                // Block all
                currentBlock -= amount;
                amount = 0;
            }
            else
            {
                // Can't block all
                amount -= currentBlock;
                currentBlock = 0;
            }

            fighterHealthBar.DisplayBlock(currentBlock);
            return amount;
        }

        public void AddBuff(Buff.Type type, int amount) // 버프 추가
        {
            switch (type)
            {
                case Buff.Type.vulnerable:
                    if (vulnerable.buffValue <= 0) // 새로운 버프 객체가 필요할 때만 생성
                    {
                        vulnerable.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    vulnerable.buffValue += amount;
                    vulnerable.buffGO.DisplayBuff(vulnerable);
                    Debug.Log($"취약 버프 적용: {vulnerable.buffValue}");
                    break;

                case Buff.Type.weak:
                    if (weak.buffValue <= 0)
                    {
                        weak.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    weak.buffValue += amount;
                    weak.buffGO.DisplayBuff(weak);
                    Debug.Log($"약화 버프 적용: {weak.buffValue}");
                    break;

                case Buff.Type.strength:
                    if (strength.buffValue <= 0)
                    {
                        strength.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    strength.buffValue += amount;
                    strength.buffGO.DisplayBuff(strength);
                    Debug.Log($"힘 버프 적용: {strength.buffValue}");
                    break;

                case Buff.Type.poison:
                    if (poison.buffValue <= 0)
                    {
                        poison.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    poison.buffValue += amount;
                    poison.buffGO.DisplayBuff(poison);
                    Debug.Log($"독 버프 적용: {poison.buffValue}");
                    break;

                case Buff.Type.bleeding:
                    if (bleeding.buffValue <= 0)
                    {
                        bleeding.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    bleeding.buffValue += amount;
                    bleeding.buffGO.DisplayBuff(bleeding);
                    Debug.Log($"출혈 버프 적용: {bleeding.buffValue}");
                    break;

                case Buff.Type.deathMark:
                    if (deathMark.buffValue <= 0)
                    {
                        deathMark.buffGO = Instantiate(buffPrefab, buffParent).GetComponent<BuffUI>();
                    }
                    deathMark.buffValue += amount;
                    deathMark.buffGO.DisplayBuff(deathMark);
                    Debug.Log($"데스마크 버프 적용: {deathMark.buffValue}");
                    break;

                default:
                    Debug.LogWarning("알 수 없는 버프 타입입니다.");
                    break;
            }

            // 적에 대한 버프 상태를 업데이트 (적에만 해당)
            if (enemy != null)
            {
                enemy.DisplayIntent();
            }
        }

        public void ReductBuff(Buff.Type type, int amount)
        {
            switch (type)
            {
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

                default:
                    Debug.LogWarning("알 수 없는 버프 타입입니다.");
                    break;
            }

            if (enemy != null)
            {
                enemy.DisplayIntent();
            }
        }




        public void EvaluateBuffsAtTurnEnd() // 턴 끝날 때 버프 적용
        {
            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue -= 1;
                if (vulnerable.buffGO != null) // BuffGO가 null인지 확인
                {
                    vulnerable.buffGO.DisplayBuff(vulnerable);
                }

                if (vulnerable.buffValue <= 0)
                {
                    if (vulnerable.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(vulnerable.buffGO.gameObject);
                    }
                }
            }

            if (weak.buffValue > 0)
            {
                weak.buffValue -= 1;
                if (weak.buffGO != null) // BuffGO가 null인지 확인
                {
                    weak.buffGO.DisplayBuff(weak);
                }

                if (weak.buffValue <= 0)
                {
                    if (weak.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(weak.buffGO.gameObject);
                    }
                }
            }

            if (ritual.buffValue > 0)
            {
                AddBuff(Buff.Type.strength, ritual.buffValue);
            }

            // 독 버프 처리
            if (poison.buffValue > 0)
            {
                TakeDamage(poison.buffValue); // 독 대미지
                poison.buffValue -= 1;
                if (poison.buffGO != null) // BuffGO가 null인지 확인
                {
                    poison.buffGO.DisplayBuff(poison);
                }

                if (poison.buffValue <= 0)
                {
                    if (poison.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(poison.buffGO.gameObject);
                    }
                }
            }

            // 출혈 버프 처리
            if (bleeding.buffValue > 0)
            {
                TakeDamage(1); // 출혈 대미지
                bleeding.buffValue -= 1;
                if (bleeding.buffGO != null) // BuffGO가 null인지 확인
                {
                    bleeding.buffGO.DisplayBuff(bleeding);
                }

                if (bleeding.buffValue <= 0)
                {
                    if (bleeding.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                    {
                        Destroy(bleeding.buffGO.gameObject);
                    }
                }
            }

            // 데스마크 처리
            if (deathMark.buffValue >= 5)
            {
                Debug.Log("데스마크 발동! 처형 중...");
                execution(this);
                if (deathMark.buffGO != null) // BuffGO가 null인지 다시 확인 후 파괴
                {
                    Destroy(deathMark.buffGO.gameObject);
                }
            }
        }



        public void ResetBuffs()
        {
            if (vulnerable.buffValue > 0)
            {
                vulnerable.buffValue = 0;
                Destroy(vulnerable.buffGO.gameObject);
            }
            else if (weak.buffValue > 0)
            {
                weak.buffValue = 0;
                Destroy(weak.buffGO.gameObject);
            }
            else if (strength.buffValue > 0)
            {
                strength.buffValue = 0;
                Destroy(strength.buffGO.gameObject);
            }
            else if (poison.buffValue > 0) // 독 버프 초기화
            {
                poison.buffValue = 0;
                Destroy(poison.buffGO.gameObject);
            }
            else if (bleeding.buffValue > 0) // 출혈 버프 초기화
            {
                bleeding.buffValue = 0;
                Destroy(bleeding.buffGO.gameObject);
            }

            // Reset block
            currentBlock = 0;
            fighterHealthBar.DisplayBlock(0);
        }
    }
}
