using UnityEngine;

public class BatAI : EnemyBattle
{
    private Rigidbody2D rb;
    public Transform leftPoint, rightPoint, player;
    public LayerMask playerLayer;
    private float xLeft, xRight;
    public GameObject shellPrefab;

    public float speed;
    private bool faceLeft = false, alive = true;

    // Start is called before the first frame update
    void Start()
    {
        xLeft = leftPoint.position.x;
        xRight = rightPoint.position.x;

        rb = GetComponent<Rigidbody2D>();

        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        if (currentHealth > 0)
        {
            Movement();
            //health -= 1;  测试死亡
        }
        else
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player") && alive)
        {
            if (coll.gameObject.TryGetComponent<PlayerBattle>(out var playerBattle))
            {
                playerBattle.Damage(attackValue);
            }
            //Debug.Log("kill you");
        }
    }

    void Movement()
    {
        var position = transform.position;
        RaycastHit2D playerFindRight =
            Physics2D.Raycast(position, Vector2.right, (float) 10.0, playerLayer); //右射线判断
        RaycastHit2D playerFindLeft =
            Physics2D.Raycast(position, Vector2.left, (float) 10.0, playerLayer); //左射线判断
        //shell.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (playerFindLeft || playerFindRight)
        {
            float faceLeftx = player.position.x - transform.position.x;
            //Debug.Log("find you");
            if (faceLeftx < 0)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = true;
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            if (faceLeft)
            {
                //AudioManager.closeCheckAudio();
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                //Debug.Log("move");
                if (transform.position.x < xLeft)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    faceLeft = false;
                }
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (transform.position.x > xRight)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    faceLeft = true;
                }
            }
        }
    }

    private void CreateShell()
    {
        GameObject.Instantiate(shellPrefab, transform.position, transform.rotation);
    }

    public override bool Death(bool force = false)
    {
        CreateShell();
        return base.Death(force);
    }
}