using UnityEngine;

public class Boss1AI : EnemyBattle
{
    private Rigidbody2D rb;
    public Transform LeftPoint, RightPoint, player;
    public LayerMask playerLayer;

    private Animator anim;
    private float xLeft, xRight;
    public float speed,speedz;
    private bool faceLeft = true;
    public int attackCloseInterval ; 
    public int attackFarInterval;  //初始攻击频率
    public int timeInterval; //攻击时间间隔
    [SerializeField] private GameObject bossFireBullet, bossFireBigBullet;

    private static readonly int animSpeedHash = Animator.StringToHash("speed");
    private bool smallFireFirst = true;
    // Start is called before the first frame update
    void Start()
    {
        xLeft = LeftPoint.position.x;
        xRight = RightPoint.position.x;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();


        speedz = speed;
        Destroy(LeftPoint.gameObject);
        Destroy(RightPoint.gameObject);
    }

    private void UpdateAnimator()
    {
        float speedX = rb.velocity.x;
        if (speedX < 0)
            speedX = -speedX;
        //Debug.Log(speedX);
        anim.SetFloat(animSpeedHash, speedX);
    }

    private bool RayHitPlayer(float dis)  //距离判断
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
        if (RayHitPlayer(40.0f))
        {
            //Debug.Log("CLOSE" + attackCloseInterval);
            //Debug.Log("far" + attackFarInterval);

            //attackFarInterval++;
            //if (attackFarInterval >= timeInterval)
            //{
            //    attackFarInterval = FarFire(attackFarInterval);
            //}
            if (!(attackCloseInterval < 10 || attackCloseInterval >= 85))
            {
                Track();  //追击
            }
           
            if (RayHitPlayer(25.0f))  //小火球
            {
                //rb.velocity = new Vector2(0, rb.velocity.y);
                speed = 2;  
                attackFarInterval++;
                if (attackFarInterval > (timeInterval)/2)
                    attackFarInterval = FarFire(attackFarInterval);
            }
            else if (RayHitPlayer(35.0f))  //大火球
            {
                speed = speedz;
                attackCloseInterval++;
                if (attackCloseInterval > timeInterval)
                {
                    attackCloseInterval = CloseFire(attackCloseInterval);
                }
                if (attackCloseInterval < 10 || attackCloseInterval >= 75)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
        else
        {
            Movement();
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

    float CalAngle(Transform ga)
    {
        Vector3 direction = ga.position - transform.position;
        //Debug.Log("ga:" + direction);
        //Debug.Log("before"+direction);
        direction.y += 3.4f;
        //Debug.Log("tran:"+direction);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle;
    }

    private int FarFire(int attackInterval1)
    {
        attackInterval1 = -1;
        Instantiate(bossFireBullet, transform.Find("firstPoint").position,
             Quaternion.AngleAxis(CalAngle(transform.Find("firstPoint")), Vector3.forward));  //第一枚火球

        Instantiate(bossFireBullet, transform.Find("secondPoint").position,
             Quaternion.AngleAxis(CalAngle(transform.Find("secondPoint")), Vector3.forward));  //第二枚火球

        Instantiate(bossFireBullet, transform.Find("thirdPoint").position,
            Quaternion.AngleAxis(CalAngle(transform.Find("thirdPoint")), Vector3.forward));  //第三枚火球

        Instantiate(bossFireBullet, transform.Find("forthPoint").position,
            Quaternion.AngleAxis(CalAngle(transform.Find("forthPoint")), Vector3.forward));  //第四枚火球

        Instantiate(bossFireBullet, transform.Find("fifthPoint").position,
            Quaternion.AngleAxis(CalAngle(transform.Find("fifthPoint")), Vector3.forward));  //第五枚火球

        Instantiate(bossFireBullet, transform.Find("sixthPoint").position,
            Quaternion.AngleAxis(CalAngle(transform.Find("sixthPoint")), Vector3.forward));  //第六枚火球
                                                                                             //Debug.Log(transform.localPosition);

        //Instantiate(bossFireBullet, transform.Find("thirdPoint").position, Quaternion.identity);
        //GameObject bullet = GameObject.Instantiate(ghostFire, rb.position,Quaternion.identity);
        attackInterval1++;
        return attackInterval1;
    }

    private int CloseFire(int attackBig1)
    {
        if (attackBig1 >= timeInterval)
        {
            attackBig1 = -1;
            Instantiate(bossFireBigBullet, transform.Find("bigPoint").position, Quaternion.identity);
            //GameObject bullet = GameObject.Instantiate(ghostFire, rb.position,Quaternion.identity);
        }
        attackBig1++;
        return attackBig1;
    }

    public override bool Death(bool force = false)
    {
        GameManager.Victory();
        //SceneManager.LoadScene("FakeEnding");//just for test
        //FindObjectOfType<SceneFader>().FadeTo("FakeEnding");
        return base.Death(force);
    }
}