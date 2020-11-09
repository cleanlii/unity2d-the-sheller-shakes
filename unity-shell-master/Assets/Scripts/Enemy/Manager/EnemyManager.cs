using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum EnemyType
    {
        UNKNOWN = 0,
        NORMAL = 1,
        BOSS = 2,
    };

    private const int EnemyTypeTot = 3;

    private Dictionary<int, KeyValuePair<GameObject, GameObject>>[] _enemies = null;

    private void Awake()
    {
        _enemies = new Dictionary<int, KeyValuePair<GameObject, GameObject>>[EnemyTypeTot];
        for (int i = 0; i < EnemyTypeTot; i++)
        {
            _enemies[i] = new Dictionary<int, KeyValuePair<GameObject, GameObject>>();
        }
    }

    public void Register(GameObject enemy)
    {
        // 如果组件不存在
        if (!enemy.TryGetComponent<EnemyResolver>(out var resolver))
        {
            // 注册新的组件
            resolver = enemy.AddComponent<EnemyResolver>();
        }
        this.Register(resolver);
    }

    private GameObject CloneGameObject(GameObject enemy, bool active)
    {
        // 克隆对象
        var clone = Instantiate(enemy, enemy.transform.parent, true);
        // 获取克隆对象的ID
        var id = clone.GetInstanceID();
        clone.SetActive(active);
        // 获取克隆对象的解决方案
        var cloneResolver = clone.GetComponent<EnemyResolver>();
        cloneResolver.manager = this;
        cloneResolver.baseObject = clone;
        return clone;
    }

    public void Register(EnemyResolver resolver)
    {
        GameObject enemy = resolver.gameObject;
        var cid = (int) resolver.classify;
        // 存在基础副本，则删除基础副本
        if (resolver.baseObject != null)
        {
            this.Deregister(resolver.baseObject.GetComponent<EnemyResolver>());
            // Debug.LogWarningFormat("Found a unregistered BaseGameObject %d", baseID);
            resolver.baseObject = null;
        }
        var clone = CloneGameObject(enemy, false);
        // 添加记录
        _enemies[cid].Add(clone.GetInstanceID(), new KeyValuePair<GameObject, GameObject>(clone, enemy));
    }

    public void Deregister(EnemyResolver resolver)
    {
        var cid = (int) resolver.classify;
        int id = resolver.gameObject.GetInstanceID();
        if (!_enemies[cid].ContainsKey(id)) return;
        var clone = _enemies[cid][id].Key;
        _enemies[cid].Remove(id);
        //Debug.Log(clone.GetInstanceID());
        Destroy(clone);
    }

    public void ResetEnemy()
    {
        this.ResetEnemy(EnemyType.NORMAL);
    }

    public void ResetEnemy(EnemyType type)
    {
        foreach (var pair in _enemies[(int) type])
        {
            Destroy(pair.Value.Value);
            CloneGameObject(pair.Value.Key, true);
        }
    }
}