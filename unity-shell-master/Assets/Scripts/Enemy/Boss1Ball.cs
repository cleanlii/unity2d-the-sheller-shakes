using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Ball : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private Rigidbody2D rb;
    public GameObject player;
    public float attackValue;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        Vector3 direction = player.transform.localPosition - transform.localPosition;
        direction.y += 2.5f;
        //Debug.LogWarning("new:" + transform.localPosition.ToString() + ",player:" + player.transform.localPosition);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        attackValue = 20f;
    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * moveSpeed;
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
        Destroy(gameObject);
    }

}
