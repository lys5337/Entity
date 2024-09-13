using UnityEngine;

[CreateAssetMenu]
public class EnemyUI : ScriptableObject
{
    public EnemyType enemyType;
    public enum EnemyType { Normal, Special, Elite, Mid_Boss, Boss, Hidden_Boss }

    public EnemyStage enemyStage;
    public enum EnemyStage { Stage0, Stage1, Stage2, Stage3, Stage4, Stage5, Stage6, Last_Stage, Hidden_Stage }

    public EnemyAttribute enemyAttribute;
    public enum EnemyAttribute { Non, Darkest, Divine }

    public EnemyRace enemyRace;
    public enum EnemyRace { Non, Darkest, Divine }

    public EnemyInfo enemyInfo; 

    [System.Serializable]
    public struct EnemyInfo
    {
        public string enemyNameUI;
        public int enemyMaxHealthUI;
        public int enemyBaseStrengthUI;
        public int enemyBaseBlockUI;
        public int enemyClearGoldMaxUI;
        public int enemyClearGoldMinUI;
    }

    public string getEnemyType()
    {
        Debug.Log("Enemy Type: " + enemyType.ToString());
        return enemyType.ToString();
    }

    public string getEnemyStage()
    {
        Debug.Log("Enemy Stage: " + enemyStage.ToString());
        return enemyStage.ToString();
    }

    public string getEnemyAttribute()
    {
        Debug.Log("Enemy Attribute: " + enemyAttribute.ToString());
        return enemyAttribute.ToString();
    }

    public string getEnemyRace()
    {
        Debug.Log("Enemy Race: " + enemyRace.ToString());
        return enemyRace.ToString();
    }
}