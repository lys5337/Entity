using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TJ
{
public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    [HideInInspector]public int clearGoldMinUI;
    [HideInInspector]public int clearGoldMaxUI;
    [HideInInspector]public string nameUI;
    public EnemyUI enemyUI;
    public TextMeshProUGUI enemyNameText; 
    public TextMeshProUGUI enemyIntentNameText;

	public List<EnemyAction> enemyActions;
	public List<EnemyAction> turns;

    public int turnNumber;
    public bool shuffleActions;
    public Fighter thisEnemy;
    public GameObject enemyObject;
    
    [Header("UI")]
    public Image intentIcon;
    public TMP_Text intentAmount;
    public BuffUI intentUI;
    public EnemyEffectManager enemyEffectManager;

    [Header("Specifics")]
    [HideInInspector]public int goldDrop;
    public bool normalCheck;
    public bool eliteCheck;
    public bool bossCheck;
    public bool hiddenEnemyCheck;
    public bool wiggler;
    public GameObject wigglerBuff;
    [HideInInspector]public GameObject nobBuff;
    BattleSceneManager battleSceneManager;
    Fighter player;
    Animator animator;
    public bool midTurn;
    GameManager gameManager;

    private int playerPower,playerAmor,playerLuck;
    private int enemyPower, enemyAmor;

    public string enemyEffectName;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        player = battleSceneManager.player;
        thisEnemy = GetComponent<Fighter>();
        animator = GetComponent<Animator>();

        InitializePlayerStatus();
        InitializeStatsFromEnemyUI(); // 적의 정보를 enemyUI에서 가져오는 함수
        DisplayEnemyName();           // 적의 이름을 화면에 표시하는 함수
        
        if (shuffleActions)
            GenerateTurns();

        DisplayIntent();
    }

    private void InitializeStatsFromEnemyUI()
    {
        clearGoldMinUI = enemyUI.enemyInfo.enemyClearGoldMinUI;
        clearGoldMaxUI = enemyUI.enemyInfo.enemyClearGoldMaxUI;
        nameUI = enemyUI.enemyInfo.enemyNameUI;
        goldDrop = Random.Range(clearGoldMinUI, clearGoldMaxUI);

        enemyPower = enemyUI.enemyInfo.enemyBaseStrengthUI;
        enemyAmor = enemyUI.enemyInfo.enemyBaseAmorUI;

        enemyActions = new List<EnemyAction>(enemyUI.enemyActions);
        turns = new List<EnemyAction>(enemyUI.turns);

        int baseGoldDrop = goldDrop;
        float bonusMultiplier = (playerLuck / 5) * 0.05f;

        if(gameManager.PlayerHasRelic("희귀하고 화려한 고대의 주화"))
        {
            bonusMultiplier += 1.0f;
        }   
        else
        {
            string[] relicNames = { "평범한 동전", "화려한 동전", "고대의 동전" };
            float[] bonusMultipliers = { 0.1f, 0.2f, 0.3f };
            for (int i = 0; i < relicNames.Length; i++)
            {
                if (gameManager.PlayerHasRelic(relicNames[i]))
                {
                    bonusMultiplier += bonusMultipliers[i];
                }
            }
        }

        goldDrop = (int)(baseGoldDrop * (1 + bonusMultiplier));
    }

    private void LoadEnemy()
    {
        battleSceneManager = FindObjectOfType<BattleSceneManager>();
        player = battleSceneManager.player;
        thisEnemy = GetComponent<Fighter>();

        if(shuffleActions)
            GenerateTurns();
    }

    private void InitializePlayerStatus()
    {
        playerPower = gameManager.playerPower;
        playerAmor = gameManager.playerAmor;
        playerLuck = gameManager.playerLuck;

        Debug.Log("플레이어 주스텟: " + playerPower + ", " + playerAmor + ", " + playerLuck);
    }

    private void DisplayEnemyName()
    {
        if (enemyNameText != null)
        {
            enemyNameText.text = nameUI;
            Debug.Log($"적의 이름이 설정되었습니다: {nameUI}");
        }
        else
        {
            Debug.LogWarning("Enemy Name Text가 설정되지 않았습니다.");
            Debug.Log($"적의 이름이 설정되지 않았습니다: {nameUI}");
        }
    }

    public void TakeTurn()
    {
        intentUI.animator.Play("IntentFade");

        switch (turns[turnNumber].intentType)
        {
            case EnemyAction.IntentType.Waiting:
                DisplayIntent();
                StartCoroutine(Waiting_());
                break;
        
            case EnemyAction.IntentType.Attack:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                break;

            case EnemyAction.IntentType.Block:
                DisplayIntent();
                PerformBlock();
                StartCoroutine(ApplyBuff());
                break;

            case EnemyAction.IntentType.StrategicBuff:
                StartCoroutine(ApplyBuff());
                DisplayIntent();
                ApplyBuffToSelf(turns[turnNumber].buffType);
                break;

            case EnemyAction.IntentType.StrategicDebuff:
                StartCoroutine(ApplyBuff());
                DisplayIntent();
                ApplyDebuffToPlayer(turns[turnNumber].buffType);
                break;

            case EnemyAction.IntentType.AttackDebuff:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                StartCoroutine(ApplyDebuffAfterAttack(turns[turnNumber].buffType));
                break;

            case EnemyAction.IntentType.AttackBuff:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                ApplyBuffToSelf(turns[turnNumber].buffType);
                break;

            case EnemyAction.IntentType.SpecialAttack:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                break;

            case EnemyAction.IntentType.EliteWolf_0Stage:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                StartCoroutine(ApplyDebuffAfterAttack(turns[turnNumber].buffType));
                break;

            case EnemyAction.IntentType.BossBear_0Stage:
                DisplayIntent();
                StartCoroutine(AttackPlayer());
                StartCoroutine(ApplyDebuffAfterAttack(turns[turnNumber].buffType));
                break;

            case EnemyAction.IntentType.RangedAttack:
                DisplayIntent();
                StartCoroutine(AttackPlayerRanged());
                break;

            case EnemyAction.IntentType.RangedAttackDebuff:
                DisplayIntent();
                StartCoroutine(AttackPlayerRanged());
                StartCoroutine(ApplyDebuffAfterAttack(turns[turnNumber].buffType));
                break;
            
            case EnemyAction.IntentType.SelfBuffPlayerDeuff:
                DisplayIntent();
                ApplyBuffToSelf(turns[turnNumber].buffType);
                ApplyDebuffToPlayer2(turns[turnNumber].buffType2);
                StartCoroutine(ApplyBuff());
                break;

            case EnemyAction.IntentType.GoldStealAttack:
                DisplayIntent();
                PlayerGoldSteal(100);
                StartCoroutine(AttackPlayer());
                break;

            default:
                Debug.Log("GET AWAY FROM ME");
                break;
        }
    }

    private void PlayerGoldSteal(int amount)
    {
        gameManager.goldAmount -= amount;
    }

    private IEnumerator ApplyDebuffAfterAttack(Buff.Type buffType)
    {
        yield return new WaitForSeconds(0.5f);
        ApplyDebuffToPlayer(buffType);
    }

    public void GenerateTurns()
    {
        foreach(EnemyAction eA in enemyActions)
        {
            for (int i = 0; i < eA.chance; i++)
            {
                turns.Add(eA);
            }
        }
        turns.Shuffle();
    }

    public int TakeDamageReturn(int amount)
    {
        thisEnemy.currentHealth -= amount;

        if (thisEnemy.currentHealth < 0)
        {
            thisEnemy.currentHealth = 0;
        }

        if (thisEnemy.currentHealth == 0)
        {
            Debug.Log($"{thisEnemy.name}이(가) 쓰러졌습니다!");

            StartCoroutine(thisEnemy.HandleDamageAndCamera(thisEnemy.transform.position, amount));
        }

        Debug.Log($"{thisEnemy.name}이(가) {amount}의 피해를 입었습니다.");
        return amount;
    }

    private IEnumerator Waiting_()
    {
        yield return new WaitForSeconds(0.5f);
        WrapUpTurn();
    }

    private IEnumerator AttackPlayer()
    {
        animator.Play("Attack");
        yield return new WaitForSeconds(0.3f);
        CallEnemyEffect(enemyEffectName);

        int totalDamage = turns[turnNumber].amount + thisEnemy.strength.buffValue - player.GetPlayerAmor() + enemyPower;
        Debug.Log(player.GetPlayerAmor());

        List<float> damageModifiers = new List<float>();

        if (thisEnemy.weak.buffValue > 0)
            damageModifiers.Add(0.7f);

        if (player.vulnerable.buffValue > 0)
            damageModifiers.Add(1.3f);

        if (player.goddessBlessing.buffValue > 0)
            damageModifiers.Add(0.1f);

        if (player.insanity.buffValue > 0)
            damageModifiers.Add(1.3f);

        foreach (float modifier in damageModifiers)
        {
            totalDamage = (int)(totalDamage * modifier);
        }

        player.TakeDamage(totalDamage);

        if (player.thornArmor.buffValue > 0)
        {
            int thornDamage = player.thornArmor.buffValue;
            Debug.Log($"가시 갑옷 효과 발동: {nameUI}이(가) {thornDamage}의 피해를 입습니다.");
            thisEnemy.TakeDamageReturn(thornDamage);
        }
        yield return new WaitForSeconds(0.5f);
        WrapUpTurn();

    }

    private IEnumerator AttackPlayerRanged()
    {
        //animator.Play("Attack");
        yield return new WaitForSeconds(0.3f);
        CallEnemyEffect(enemyEffectName);

        int totalDamage = turns[turnNumber].amount + thisEnemy.strength.buffValue - player.GetPlayerAmor() + enemyPower;
        Debug.Log(player.GetPlayerAmor());

        List<float> damageModifiers = new List<float>();

        if (thisEnemy.weak.buffValue > 0)
            damageModifiers.Add(0.7f);

        if (player.vulnerable.buffValue > 0)
            damageModifiers.Add(1.3f);

        if (player.goddessBlessing.buffValue > 0)
            damageModifiers.Add(0.1f);

        if (player.insanity.buffValue > 0)
            damageModifiers.Add(1.3f);

        foreach (float modifier in damageModifiers)
        {
            totalDamage = (int)(totalDamage * modifier);
        }

        player.TakeDamage(totalDamage);

        yield return new WaitForSeconds(0.5f);
        WrapUpTurn();
    }



    public void CallEnemyEffect(string effectName = "Slash_Effect/Basic Slash Blue")
    {
        int xpos = 0;
        int ypos = 0;
        
        if(effectName =="Magic_Effect_addition/Buff_02aDown")
        {
            xpos = -30;
            ypos = -190;
        }

        int positionVal = 0;
        if (effectName == "Slash_Effect/Basic Slash Blue")
        {
            positionVal = -10;
        }
        else
        {
            positionVal = -1;
        }
        Vector3 effectPosition = new Vector3(player.transform.position.x+xpos, player.transform.position.y+ypos, player.transform.position.z + positionVal);
        enemyEffectManager.PlayEnemyEffect(effectName, effectPosition);
    }


    private IEnumerator ApplyBuff()
    {
        yield return new WaitForSeconds(1f);
        WrapUpTurn();
    }

    private void WrapUpTurn()
    {
        turnNumber++;
        if(turnNumber==turns.Count)
            turnNumber=0;

        thisEnemy.EvaluateBuffsAtTurnEnd();
        midTurn = false;
    }

    private void ApplyBuffToSelf(Buff.Type t)
    {
        thisEnemy.AddBuff(t, turns[turnNumber].amount);
    }

    private void ApplyDebuffToPlayer(Buff.Type t)
    {
        if(player==null)
            LoadEnemy();

        CallEnemyEffect(enemyEffectName);
        
        player.AddBuff(t, turns[turnNumber].debuffAmount);
    }

    private void ApplyDebuffToPlayer2(Buff.Type t)
    {
        if(player==null)
            LoadEnemy();
        player.AddBuff(t, turns[turnNumber].debuffAmount2);
    }

    private void PerformBlock()
    {
        thisEnemy.AddBlock(turns[turnNumber].amount+enemyAmor);
    }

    public void DisplayIntent()
    {
        int baseDamage = 0;
        int baseBlock = 0;

        if (turns.Count == 0)
        {
            LoadEnemy();
            return;
        }

        InitializePlayerStatus();

        if (player == null || thisEnemy == null)
        {
            Debug.LogError("player 또는 thisEnemy가 null입니다.");
            return;
        }

        intentIcon.sprite = turns[turnNumber].icon;

        baseDamage = turns[turnNumber].amount- player.GetPlayerAmor();
        baseBlock = turns[turnNumber].amount;

        baseDamage += enemyPower;

        int totalDamage = baseDamage + thisEnemy.strength.buffValue;

        List<float> damageModifiers = new List<float>();

        if (player.vulnerable.buffValue >= 2)
            damageModifiers.Add(1.3f);

        if (player.goddessBlessing.buffValue >= 2)
            damageModifiers.Add(0.1f);

        if (player.insanity.buffValue >= 1)
            damageModifiers.Add(1.25f);

        if (thisEnemy.weak.buffValue >= 1)
            damageModifiers.Add(0.75f);

        

        foreach (float modifier in damageModifiers)
        {
            totalDamage = (int)(totalDamage * modifier);
        }

        int totalBlock = baseBlock + enemyAmor;

        string intentDescription = "";
        switch (turns[turnNumber].intentType)
        {
            case EnemyAction.IntentType.Attack:
            case EnemyAction.IntentType.AttackDebuff:
            case EnemyAction.IntentType.AttackBuff:
            case EnemyAction.IntentType.EliteWolf_0Stage:
            case EnemyAction.IntentType.BossBear_0Stage:
            case EnemyAction.IntentType.RangedAttack:
            case EnemyAction.IntentType.RangedAttackDebuff:
            case EnemyAction.IntentType.SpecialAttack:
            case EnemyAction.IntentType.GoldStealAttack:
                intentAmount.text = totalDamage.ToString();
                intentDescription = IntentDescriptionProvider.GetIntentDescription(this, turns[turnNumber].intentType);
                break;

            case EnemyAction.IntentType.Block:
                intentAmount.text = totalBlock.ToString();
                intentDescription = IntentDescriptionProvider.GetIntentDescription(this, turns[turnNumber].intentType);
                break;

            case EnemyAction.IntentType.StrategicBuff:
            case EnemyAction.IntentType.StrategicDebuff:
            case EnemyAction.IntentType.Waiting:
            case EnemyAction.IntentType.SelfBuffPlayerDeuff:
                intentAmount.text = "";
                intentDescription = IntentDescriptionProvider.GetIntentDescription(this, turns[turnNumber].intentType);
                break;
        }

        if (enemyIntentNameText != null)
        {
            enemyIntentNameText.text = intentDescription;
        }

        intentUI.animator.Play("IntentSpawn");
    }




    public void CurlUP()
    {
        wigglerBuff.SetActive(false);
        thisEnemy.AddBlock(5);
    }
}
}