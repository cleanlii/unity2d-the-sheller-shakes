using UnityEngine;

public class SpiderAI : EnemyBattle
{
    private Rigidbody2D rb;
    public Transform leftPoint, rightPoint, player;
    public GameObject shellPrefab;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private float xLeft, xRight;
    public float speed;
    private bool faceLeft = true, alive = true;
    private bool isGround = true;

    private float speedJump = 7;

    // Start is called before the first frame update
    void Start()
    {
        xLeft = leftPoint.position.x;
        xRight = rightPoint.position.x;

        rb = GetComponent<Rigidbody2D>();

        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
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

    //PlayerBattle acomponent; //攻击玩家

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

    void jump(float xSpeed)
    {
        // 施加一个向上的速度
        rb.velocity = new Vector2(3*xSpeed, speedJump);

        Debug.Log("jump");
    }

    void Movement()
    {
        //Debug.Log(rb.velocity.y);
        if (rb.velocity.y == 0)
        {
            isGround = true;  //本来在最高处应当是会出现BUG的，但是真正情况下y值的取法是离散的，而且是float类型
                              //导致其在最高点的时候是取不到0的，取到的概率明显小于千分之一，万一取到变成2连跳，用于彩蛋
        }
        var position = transform.position;
        RaycastHit2D playerFindRight =
            Physics2D.Raycast(position, Vector2.right, (float) 10.0, playerLayer); //右射线判断
        RaycastHit2D playerFindLeft =
            Physics2D.Raycast(position, Vector2.left, (float) 10.0, playerLayer); //左射线判断
        if (playerFindLeft || playerFindRight)
        {
            float faceLeftx = player.position.x - transform.position.x;
            //Debug.Log("find you");
            if (faceLeftx < 0)
            {
                if (isGround&& rb.velocity.y == 0)
                {
                    jump(-speed);
                    isGround = false;
                    Debug.Log("false");
                }
                //rb.velocity = new Vector2(-speed, rb.velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
                faceLeft = true;
            }
            else
            {
                if (isGround)
                {
                    jump(speed);
                    isGround = false;
                }
                //rb.velocity = new Vector2(speed, rb.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
                faceLeft = false;
            }
        }
        else
        {
            if (faceLeft)
            {
                //AudioManeger.closeCheckAudio();
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                if (transform.position.x < xLeft)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    faceLeft = false;
                }
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (transform.position.x > xRight)
                {
                    transform.localScale = new Vector3(1, 1, 1);
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