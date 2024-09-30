using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject effectPackage;
    private Dictionary<string, GameObject> effects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        AddEffectsRecursively(effectPackage.transform);
    }

    private void AddEffectsRecursively(Transform parent, string currentPath = "")
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
                else
                {
                    Debug.LogWarning($"Effect {fullPath} is already added. Skipping duplicate.");
                }
            }
            AddEffectsRecursively(child, fullPath);
        }
    }

    public void PlayEffect(string effectName, Vector3 position)
    {
        if (effects.ContainsKey(effectName))
        {
            Debug.Log($"실행된 효과명{effectName}");
            Instantiate(effects[effectName], position, Quaternion.identity);
        }
        else
        {
            Debug.LogError($"Effect {effectName} not found!");
        }
    }
}