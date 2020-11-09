using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesController : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float stopTime;
    public float maxMoveDis;
    public float attackValue;
    public bool startPath;
    public int mode;
    [SerializeField] private float stopNow;
    [SerializeField] private PolygonCollider2D rb;
    [SerializeField] private float startY;
    [SerializeField] private float nowY;
    void Start()
    {
        startY = transform.position.y;
        stopNow = 0;
        rb = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == 1)
        {
            ModeOne();
        }
        else if(mode == 2)
        {
            ModeTwo();
        }
    }

    private void Move()
    {
        if (startPath)
        {
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }
        else
        {
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerBattle>(out var playerBattle))
            {
                playerBattle.Damage(attackValue);
            }
        }
    }

    private void ChangeY()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1, 1);
    }

    private void ModeOne()
    {
        nowY = transform.position.y;
        if ((nowY <= (startY + maxMoveDis) && (nowY >= startY)) || (nowY >= (startY + maxMoveDis) && nowY <= startY))
        {
            Move();
        }
        else
        {
            if (stopNow == 0)
            {
                ChangeY();
            }
            if (stopNow < stopTime)
            {
                stopNow += Time.deltaTime;
            }
            else
            {
                if (startPath)
                {
                    transform.position = new Vector3(transform.position.x, startY + maxMoveDis, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, startY, transform.position.z);

                }
                startPath = !startPath;
                stopNow = 0;
            }
        }
    }

    private void ModeTwo()
    {
        if (stopNow < stopTime)
        {
            stopNow += Time.deltaTime;
        }
        else
        {
            stopNow = 0;
            rb.enabled = !rb.enabled;
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
        }
    }
}
