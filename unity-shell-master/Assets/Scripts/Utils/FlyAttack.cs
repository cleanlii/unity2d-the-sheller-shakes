using System;
using UnityEngine;

public class FlyAttack : MonoBehaviour
{
    public Vector2 moveSpeed = new Vector2(0, 0);
    public Rigidbody2D rb;
    public float attackValue = 0.0f;
    public String attackTag = "";
    public LayerMask attackLayer;

    public void Start()
    {
        rb.velocity = moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((attackTag.Length > 0 && other.gameObject.CompareTag(attackTag)) ||
            (attackLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.gameObject.TryGetComponent<IBattle>(out var battle))
            {
                battle.Damage(attackValue);
            }
            Destroy(gameObject);
        }
        else if (!other.gameObject.TryGetComponent<FlyAttack>(out var attack))
        {
            Destroy(gameObject);
        }
    }
}