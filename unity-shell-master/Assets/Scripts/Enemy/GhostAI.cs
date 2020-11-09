using UnityEngine;

public class GhostAI : EnemyBattle
{
    private Rigidbody2D rb;
    public Transform leftPoint, rightPoint, player;
    public GameObject shellPrefab;
    public LayerMask playerLayer;
    private float xLeft, xRight;
    public float speed;
    private bool faceLeft = false, alive = true;
    [SerializeField] private GameObject ghostFire;
    public int attackInterval = 100;  //初始攻击频率
    public int timeInterval = 90; //攻击时间间隔

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
            var position = transform.position;
            RaycastHit2D playerFindRight = Physics2D.Raycast(position, Vector2.right, (float)30.0, playerLayer); //右射线判断
            RaycastHit2D playerFindLeft = Physics2D.Raycast(position, Vector2.left, (float)30.0, playerLayer); //左射线判断
            if (playerFindLeft || playerFindRight)
            {
                if (attackInterval < timeInterval)
                {
                    Track();  // 追踪玩家
                    attackInterval++;
                }
                if (attackInterval >= timeInterval)  //施法停顿
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);  
                    attackInterval = Fire(attackInterval);
                }
            }
            else
            {
                Movement();
            }
        }
        else
        {
            alive = false;
            Death();
        }
    }

    PlayerBattle acomponent; //攻击玩家

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

    void Track()
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

    void Movement()
    {
        if (faceLeft)
        {
            //AudioManager.closeCheckAudio();
            rb.velocity = new Vector2(-speed, rb.velocity.y);
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

    private void CreateShell()
    {
        GameObject.Instantiate(shellPrefab, transform.position, transform.rotation);
    }

    public override bool Death(bool force = false)
    {
        CreateShell();
        return base.Death(force);
    }

    private int Fire(int attackInterval)
    {
        if(attackInterval >= timeInterval+20)
        {
            attackInterval = -1;
            Instantiate(ghostFire, rb.position, Quaternion.identity);
            //GameObject bullet = GameObject.Instantiate(ghostFire, rb.position,Quaternion.identity);
        }
        attackInterval++;
        return attackInterval;
    }
}