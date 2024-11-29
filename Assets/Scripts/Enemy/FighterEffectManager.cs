using System.Collections.Generic;
using UnityEngine;

public class FighterEffectManager : MonoBehaviour
{
    public GameObject effectPackage;
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        AddFighterEffectsRecursively(effectPackage.transform);
    }

    private void AddFighterEffectsRecursively(Transform parent, string currentPath = "")
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
            AddFighterEffectsRecursively(child, fullPath);
        }
    }

    public void PlayFighterEffect(string effectName, Vector3 position)
    {
        Debug.Log("Fighter이펙트가 실행됭 이름은: " + effectName);
        if (effects.ContainsKey(effectName))
        {
            Instantiate(effects[effectName], position, Quaternion.identity);
        }
    }
}