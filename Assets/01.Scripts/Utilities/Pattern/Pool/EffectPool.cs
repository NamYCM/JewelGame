using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : SingletonMono<EffectPool>
{
    [System.Serializable]
    public struct EffectPrefab
    {
        public EffectType type;
        public GameObject prefab;
    };

    [SerializeField] private EffectPrefab[] effectPrefabs;

    Dictionary<EffectType, GameObject> effectPrefabDict;
    Dictionary<EffectType, Queue<GameObject>> effectQueueDict;

    private void Awake() {
        effectQueueDict = new Dictionary<EffectType, Queue<GameObject>>();

        for (int i = 0; i < effectPrefabs.Length; i++)
        {
            if (!effectQueueDict.ContainsKey(effectPrefabs[i].type))
            {
                effectQueueDict.Add(effectPrefabs[i].type, new Queue<GameObject>());
            }
        }

        effectPrefabDict = new Dictionary<EffectType, GameObject>();

        for (int i = 0; i < effectPrefabs.Length; i++)
        {
            if (!effectPrefabDict.ContainsKey(effectPrefabs[i].type))
            {
                effectPrefabDict.Add(effectPrefabs[i].type, effectPrefabs[i].prefab);
            }
        }
    }

    private void GenerateEffect(EffectType type, int amount)
    {
        for (int count = 0; count < amount; count ++)
        {
            GameObject effect = Instantiate(effectPrefabDict[type], this.transform);
            effect.SetActive(false);
            effectQueueDict[type].Enqueue(effect);
        }
    }

    public GameObject GetEffect(EffectType type)
    {
        if (effectQueueDict[type].Count == 0)
        {
            GenerateEffect(type, 1);
        }

        GameObject effect = effectQueueDict[type].Dequeue();

        // piece.gameObject.SetActive(true);
        return effect;
    }
    public GameObject GetEffect(EffectType type, Transform parent)
    {
        if (effectQueueDict[type].Count == 0)
        {
            GenerateEffect(type, 1);
        }

        GameObject effect = effectQueueDict[type].Dequeue();
        effect.transform.SetParent(parent);
        return effect;
    }

    public void ReturnToPool(EffectAnimation effect)
    {
        EffectType type;
        
        if (effect.GetType() == typeof(LaserAnimation))
            type = EffectType.Laser;
        else if (effect.GetType() == typeof(FireAnimation))
            type = EffectType.Fire;
        else
        {
            return;
        }

        effect.gameObject.SetActive(false);
        effectQueueDict[type].Enqueue(effect.gameObject);
    }
    public void ReturnToPool(GameObject effect, EffectType type)
    {
        effect.gameObject.SetActive(false);
        effectQueueDict[type].Enqueue(effect.gameObject);
    }
}
