using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI : EnemyBattle
{
    private Rigidbody2D rb;
    public Transform LeftPoint, RightPoint, player;
    public LayerMask playerLayer;

    private Animator anim;
    private float xLeft, xRight, speedZ;
    public float speed;
    private bool faceLeft = true;
    public int attackCloseInterval = 0, attackFarInterval = 0; //初始攻击频率
    public int timeInterval = 90; //攻击时间间隔,模拟时间
    [SerializeField] private GameObject bossFireFarBullet, bossFireCloseBullet;

    private static readonly int animSpeedHash = Animator.StringToHash("speed");
    private bool stopUpdate = false;

    // Start is called before the first frame update
    void Start()
    {
        xLeft = LeftPoint.position.x;
        xRight = RightPoint.position.x;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        Destroy(LeftPoint.gameObject);
        Destroy(RightPoint.gameObject);

        speedZ = speed;
    }

    private void UpdateAnimator()
    {
        float speedX = rb.velocity.x;
        if (speedX < 0)
            speedX = -speedX;
        anim.SetFloat(animSpeedHash, speedX);
    }

    private bool RayHitPlayer(float dis) //距离判断
    {
        var position = transform.position;
        RaycastHit2D playerFindRight1 = Physics2D.Raycast(position, Vector2.right, dis, playerLayer); //右射线判断
        RaycastHit2D playerFindLeft1 = Physics2D.Raycast(position, Vector2.left, dis, playerLayer); //左射线判断
        position.y -= 3; //放大射线
        RaycastHit2D playerFindRight2 = Physics2D.Raycast(position, Vector2.right, dis, playerLayer);
        RaycastHit2D playerFindLeft2 = Physics2D.Raycast(position, Vector2.left, dis, playerLayer);
        return (playerFindLeft1 || playerFindRight1 || playerFindLeft2 || playerFindRight2);
    }

    // Update is called once per frame
    void Update()
    {
        if (RayHitPlayer(30.0f))
        {
            //Debug.Log("CLOSE" + attackCloseInterval);
            //Debug.Log("far" + attackFarInterval);

            //attackFarInterval++;
            //if (attackFarInterval >= timeInterval)
            //{
            //    attackFarInterval = FarFire(attackFarInterval);
            //}
            if (!(attackCloseInterval > timeInterval || attackFarInterval > timeInterval))
                Track(); //追击
            if (RayHitPlayer(10.0f)) //近端施法
            {
                attackCloseInterval++;
                if (attackCloseInterval > timeInterval)
                    attackCloseInterval = CloseFire(attackCloseInterval);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (RayHitPlayer(20.0f)) //远端施法
            {
                speed = 3;
                attackFarInterval++;
                if (attackFarInterval > timeInterval)
                    attackFarInterval = FarFire(attackFarInterval);
                if (attackFarInterval < 10 || attackFarInterval >= 75)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            speed = speedZ;
        }
        else
        {
            Movement(); //巡逻
        }
        UpdateAnimator();
    }


    void Track()
    {
        float FaceLeftx = player.position.x - transform.position.x;
        if (FaceLeftx < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Movement()
    {
        if (faceLeft)
        {
            //巡视bgm
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

    private void LandBall()
    {
        Vector3 pos = player.transform.position;
        pos.y -= 20;
        Vector3 direction = player.transform.position - pos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pos.y = -6.5f;
        StartCoroutine(Utils.WaitAndDo(1.0f, () =>
        {
            Instantiate(bossFireFarBullet, pos, Quaternion.AngleAxis(angle, Vector3.forward));
            Debug.Log("BALL");
        }));
    }

    private int FarFire(int attackInterval)
    {
        //stopUpdate = true;
        //Debug.Log("FFFFFFFFFFFFFFFFFFFFFFFFF");
        if (attackInterval == timeInterval + 90)
        {
            Vector3 pos = player.transform.position;
            pos.y -= 20;
            Vector3 direction = player.transform.position - pos;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pos.y = -6.5f;

            Instantiate(bossFireFarBullet, pos, Quaternion.AngleAxis(angle, Vector3.forward));
            pos.x -= 2;
            Instantiate(bossFireFarBullet, pos, Quaternion.AngleAxis(angle, Vector3.forward));
            pos.x += 4;
            Instantiate(bossFireFarBullet, pos, Quaternion.AngleAxis(angle, Vector3.forward));
            attackInterval = 0;
            stopUpdate = false;
        }
        if (attackInterval == (timeInterval + 50))
        {
            Debug.Log("FIRE2");
            LandBall();
        }
        if (attackInterval == timeInterval + 10)
        {
            Debug.Log("FIRE1");
            LandBall();
        }
        return attackInterval;
    }

    private int CloseFire(int attackInterval1)
    {
        Vector3 pos = player.transform.position;
        pos.y += 20;

        Vector3 direction = player.transform.position - pos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //pos.y += 40;
        pos.y += 20;
        Instantiate(bossFireCloseBullet, pos,
            Quaternion.AngleAxis(angle, Vector3.forward));
        pos.y += 70;
        Instantiate(bossFireCloseBullet, pos,
            Quaternion.AngleAxis(angle, Vector3.forward));
        pos.y += 70;
        Instantiate(bossFireCloseBullet, pos,
            Quaternion.AngleAxis(angle, Vector3.forward));
        attackInterval1 = -1;
        attackInterval1++;
        return attackInterval1;
    }

    public override bool Death(bool force = false)
    {
        GameManager.FinalVictory();
        //SceneManager.LoadScene("FakeEnding");//just for test
        //FindObjectOfType<SceneFader>().FadeTo("FakeEnding");
        return base.Death(force);
    }
}