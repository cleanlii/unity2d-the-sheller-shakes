using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

[Serializable]
public class SpritePlanItem
{
    [SerializeField] public string category;
    [SerializeField] public string label;
}

[Serializable]
public class SpritePlan
{
    [SerializeField] public string planName;
    [SerializeField] public List<SpritePlanItem> items = new List<SpritePlanItem>();
}

public class PlayerSpriteManager : MonoBehaviour
{
    [SerializeField] public List<SpritePlan> plans;

    private readonly Dictionary<string, int> planNameToID = new Dictionary<string, int>();

    public Dictionary<string, SpriteResolver> spriteResolvers = new Dictionary<string, SpriteResolver>();

    private void Start()
    {
        // 遍历所有方案
        for (int pid = 0; pid < plans.Count; pid++)
        {
            // 
            var planName = plans[pid].planName;
            if (planNameToID.ContainsKey(planName))
            {
                Debug.LogWarningFormat("Plan Name exist! {0}, Game InstanceID: {1}", planName,
                    gameObject.GetInstanceID());
                continue;
            }
            planNameToID[planName] = pid;
            foreach (var item in plans[pid].items.Where(item => !spriteResolvers.ContainsKey(item.category)))
            {
                spriteResolvers.Add(item.category, null);
            }
        }
        // 遍历子组件SpriteResolver
        foreach (var resolver in GetComponentsInChildren<SpriteResolver>())
        {
            // 如果需要这个子组件
            if (spriteResolvers.ContainsKey(resolver.GetCategory()))
            {
                // 用分类名字对应组件
                spriteResolvers[resolver.GetCategory()] = resolver;
            }
        }
        // 如果某一个分类的名字找不到对应的组件
        foreach (var item in spriteResolvers.Where(item => item.Value == null))
        {
            Debug.LogWarningFormat("Sprite Resolver {0} not found! , Game InstanceID: {1}", item.Key,
                gameObject.GetInstanceID());
        }
    }

    public bool SetSpritePlan(string planName)
    {
        if (!planNameToID.ContainsKey(planName))
        {
            Debug.LogWarningFormat("The sprite plan {0} is not existed", planName);
            return false;
        }
        foreach (var setting in plans[planNameToID[planName]].items)
        {
            if (this.spriteResolvers.ContainsKey(setting.category))
                this.spriteResolvers[setting.category].SetCategoryAndLabel(setting.category, setting.label);
        }
        return true;
    }
}