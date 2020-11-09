using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Land : MonoBehaviour
{
    public float attackValue = 0;

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerBattle>(out var playerBattle))
            {
                playerBattle.Damage(attackValue);
                Debug.Log("kill");
                if (TryGetComponent<Collider2D>(out var coll))
                {
                    Destroy(coll);
                }
                Destroy(gameObject, 0.5f);
            }
        }
    }
}