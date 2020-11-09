using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBattle : MonoBehaviour, IBattle
{
    public float currentHealth = 0.0f, maxHealth = 100.0f;
    public float attackValue;

    public float attackBackTimes = 1.0f;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public float Recovery(float health)
    {
        health = Math.Min(maxHealth - currentHealth, health);
        currentHealth += health;
        return health;
    }

    public virtual float Damage(float damage)
    {
        if (currentHealth <= damage)
        {
            damage = currentHealth;
            currentHealth = 0.0f;
            Death();
            return damage;
        }
        currentHealth -= damage;
        return damage;
    }

    public virtual bool Death(bool force = false)
    {
        GameManager.EnemyCalculator();
        Destroy(gameObject);
        return true;
    }

    public virtual float MaxHealth()
    {
        return maxHealth;
    }

    public virtual float CurrentHealth()
    {
        return currentHealth;
    }

    public void ChangeAttack(float value, float time = 0)
    {
        attackValue += value;
        if (time > 0)
        {
            StartCoroutine(Utils.WaitAndDo(time, () => { attackValue -= value; }));
        }
    }

    public virtual void AttackBack(Vector3 position, float times = 1)
    {
        times *= attackBackTimes;
        var pos = transform.position - position;
        pos.z = 0;
        pos.Normalize();
        pos *= times;
        transform.position += pos;
    }
}