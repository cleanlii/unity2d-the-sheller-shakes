using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Sky : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    public GameObject player;
    public float attackValue = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(0, -moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerBattle>(out var playerBattle))
            {
                playerBattle.Damage(attackValue);
            }
        }
        if (!other.gameObject.CompareTag("EnemyAttack"))
        {
            Destroy(gameObject);
        }
    }
}
