using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Tooltip("水平移动加速度")] [Min(0)]
    private float speedHorizontal = 0;

    [SerializeField] [Tooltip("跳跃向上加速度")] [Min(0)]
    private float speedJump = 0;

    [SerializeField] [Tooltip("最大跳跃次数限制")] [Min(0)]
    private int maxJumpCount = 0;

    [SerializeField] private Rigidbody2D rb = null;

    [SerializeField] [Tooltip("检测地面触碰点")] private Transform groundCheckPoint = null;

    [SerializeField] private LayerMask groundLayer = 0;

    [SerializeField] private Animator anim = null;

    // 判定当前是否在地面, 判断是否按下跳跃键
    private bool onGround, jumpPressed;
    private bool isSquat;
    private bool isTalk;

    // 还能跳跃的次数
    private int jumpCount;
    private static readonly int animSquatHash = Animator.StringToHash("isSquat");
    private static readonly int animSpeedHorizontalHash = Animator.StringToHash("speedHorizontal");
    private static readonly int animSpeedVerticalHash = Animator.StringToHash("speedVertical");

    //起跳位置
    public float x, y;
    public Vector3 takeOffPositon;

    public bool IsSquat
    {
        get => isSquat;
        set
        {
            if (isSquat == value) return;
            isSquat = value;
            if(isSquat) 
                OnSquat?.Invoke(gameObject);
        }
    }

    public delegate void OnSquatAction(GameObject player);

    public event OnSquatAction OnSquat;

    void Start()
    {
        jumpCount = 0;
        onGround = false;
        jumpPressed = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void UpdateAnimator()
    {
        anim.SetBool(animSquatHash, IsSquat);
        anim.SetFloat(animSpeedHorizontalHash, Math.Abs(rb.velocity.x));
        anim.SetFloat(animSpeedVerticalHash, rb.velocity.y);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        if (IsSquat != (Input.GetAxisRaw("Vertical") < 0))
        {
            IsSquat = !IsSquat;
        }
        UpdateAnimator();
    }

    private bool CheckOnGround()
    {
        return Physics2D.OverlapCircle(groundCheckPoint.position, 0.1f, groundLayer);
    }

    private void FixedUpdate()
    {
        onGround = CheckOnGround();
        Jump();
        Move();
        ActiveAudio();
    }

    private void Move()
    {
        var horizontalMove = Input.GetAxisRaw("Horizontal");
        // 施加一个横向的速度
        if (Math.Abs(horizontalMove) > 0.001f)
        {
            // 获取坐标重新给与方向
            rb.velocity = new Vector2(horizontalMove * speedHorizontal, rb.velocity.y);
            Vector3 k = this.transform.localScale;
            k.x = Math.Abs(k.x) * horizontalMove;
            this.transform.localScale = k;
        }
    }

    private void Jump()
    {
        if (onGround)
        {
            x = rb.transform.position.x;
            y = rb.transform.position.y+2;

            takeOffPositon = new Vector3(x, y, 0);
            jumpCount = maxJumpCount;
        }
        if (jumpPressed && jumpCount > 0)
        {
            jumpPressed = false;
            jumpCount--;
            onGround = false;
            // 施加一个向上的速度
            rb.velocity = new Vector2(rb.velocity.x, speedJump);
            AudioManeger.playerJumpAudio();
        }
        jumpPressed = false;
    }

   /* private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == 11)
        {
            x = rb.transform.position.x;
            y = rb.transform.position.y;
            
            takeOffPositon = new Vector3(x, y, 0);
            Debug.Log(takeOffPositon);
        }
    }*/

    private void ActiveAudio()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AudioManeger.ActiveAudio();
        }
    }
}