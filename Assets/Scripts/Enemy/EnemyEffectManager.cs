using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectManager : MonoBehaviour
{
    public GameObject effectPackage;
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        AddEnemyEffectsRecursively(effectPackage.transform);
    }

    private void AddEnemyEffectsRecursively(Transform parent, string currentPath = "")
    {
        foreach (Transform child in parent)
        {
            string fullPath = string.IsNullOrEmpty(currentPath) ? child.name : currentPath + "/" + child.name;

            if (child.gameObject.GetComponent<ParticleSystem>() != null)
            {
                if (!effects.ContainsKey(fullPath))
                {
                    effects.Add(fullPath, child.gameObject);
                }
            }
            AddEnemyEffectsRecursively(child, fullPath);
        }
    }

    public void PlayEnemyEffect(string effectName, Vector3 position)
    {
        Debug.Log("Enemy이펙트가 실행됭 이름은: " + effectName);
        if (effects.ContainsKey(effectName))
        {
            Instantiate(effects[effectName], position, Quaternion.identity);
        }
    }
}