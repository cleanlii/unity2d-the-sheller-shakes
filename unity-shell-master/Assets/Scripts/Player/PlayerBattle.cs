using System;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IBattle
{
    public float maxHp = 100.0f;
    public float currentHp = 10.0f;
    public float attackValue = 50.0f;
    public float attackDelta = 1.0f, fireDelta = 0.5f;
    public Transform attackPoint, attackBackPoint;
    public Vector2 attackSize = new Vector2(1, 2);
    public LayerMask enemyLayer;
    public Animator anim;

    public GameObject shellCreate;
    public PlayerSpriteManager manager;
    public LayerMask shellLayer;

    public GameObject deathVFXPrefab;

    public GameObject fireObject;
    public SpriteRenderer fireRender;

    public int trapsLayer;

    [SerializeField] private float nextAttackTime = 0.0f, nextFireTime = 0.0f;

    private bool attackPress = false, activePress = false, firePress = false;
    private bool attackEvent = false, activeEvent = false;
    private static readonly int animBattleHash = Animator.StringToHash("isBattle");
    private static readonly int animActiveHash = Animator.StringToHash("isActive");
    private bool isDead = false;
    public GameObject deadShell = null;
    private void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
        currentHp = maxHp;
        isDead = false;
        deadShell = null;
    }

    public void Update()
    {
        attackPress = Input.GetButtonDown("Fire1");
        firePress = Input.GetButtonDown("Active1");
        activeEvent = activePress = Input.GetButtonDown("Active1");
        attackEvent = activeEvent = false;
        Attack();
        Fire();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (attackEvent)
        {
            anim.SetTrigger(animBattleHash);
        }
        if (activeEvent)
        {
            anim.SetTrigger(animActiveHash);
        }
    }

    public void Attack()
    {
        if (!attackPress || Time.realtimeSinceStartup < nextAttackTime) return;
        attackEvent = true;
        nextAttackTime = Time.realtimeSinceStartup + attackDelta;
        AudioManeger.playerSlashAudio();
        Collider2D[] lists = Physics2D.OverlapCapsuleAll(attackPoint.position, attackSize, CapsuleDirection2D.Vertical,
            0.0f, enemyLayer);
        foreach (var enemy in lists)
        {
            if (enemy.gameObject.TryGetComponent<EnemyBattle>(out var enemyBattle))
            {
                enemyBattle.Damage(attackValue);
                enemyBattle.AttackBack(attackBackPoint.position);
            }
        }
    }

    public void Fire()
    {
        if (!firePress || Time.realtimeSinceStartup < nextFireTime) return;
        nextFireTime = Time.realtimeSinceStartup + fireDelta;
        AudioManeger.playerSlashAudio();
        // Quaternion.FromToRotation()
        // 生成火球
        var rotation = fireObject.transform.rotation;
        rotation.z *= gameObject.transform.localScale.x > 0.0 ? 1.0f : -1.0f;
        var fire = Instantiate(fireObject, attackPoint.transform.position, rotation);
        // 设置大小和方向
        fire.transform.localScale = gameObject.transform.localScale / 4;
        // 设置飞行速度
        var flyAttack = fire.GetComponent<FlyAttack>();
        flyAttack.moveSpeed.x *= gameObject.transform.localScale.x > 0.0 ? 1.0f : -1.0f;
        // 设置SpriteRender
        fire.GetComponent<SpriteRenderer>().sprite = fireRender.sprite;
    }

    public float Recovery(float health)
    {
        health = Math.Min(maxHp - currentHp, health);
        currentHp += health;
        return health;
    }

    public float Damage(float damage)
    {
        if (currentHp <= damage && currentHp != 0.0f)
        {
            damage = currentHp;
            currentHp = 0.0f;
            Death();
            return damage;
        }
        if (TryGetComponent<WhiteController>(out var white))
        {
            white.setWhite(0.15f);
        }
        AudioManeger.playerHurtAudio();
        currentHp -= damage;
        return damage;
    }

    public bool Death(bool force = false)
    {
        if (isDead) return false;
        isDead = true;
        Instantiate(deathVFXPrefab, transform.position, transform.rotation);
        ShellConnect();
        AudioManeger.playerDeathAudio();
        //GameManager.Defeat();
        return true;
    }

    public float MaxHealth()
    {
        return maxHp;
    }

    public float CurrentHealth()
    {
        return currentHp;
    }

    public void ChangeAttack(float value, float time = 0)
    {
        attackValue += value;
        if (time > 0)
        {
            StartCoroutine(Utils.WaitAndDo(time, () => { attackValue -= value; }));
        }
    }

    public void AttackBack(Vector3 position, float times = 1)
    {
    }

    public void ShellConnect()
    {

        if(FindObjectOfType<Lost>().isLost)
        {
            var shell = GameObject.Instantiate(shellCreate);
            shell.transform.position = GetComponent<PlayerMovement>().takeOffPositon;
            deadShell = shell;
        }
        else
        {
             var shell = GameObject.Instantiate(shellCreate, transform.position, transform.rotation);
            shell.transform.position = gameObject.transform.position;
            deadShell = shell;
        }
        GameObject.Find("Canvas").GetComponent<SendUI>().setActive();
    }

    /*public void CreateShell(string words,string sceneName,string name,GameObject shell)
    {
        // 接口调用方法
        Rootobject shells = new Rootobject();
        shells = Front.HttpGet("BUG4", 1);
        //从BUG4场景中随机获取2个壳,测试的时候建议不用MainLevel的场景
        StartCoroutine(Front.IEPostCreateShell("测试!", "BUG4", "prefabShell1",shellCreate));
        //"BUG4"：BUG4，prefabShell1是gameobject的名字，"测试！"即为壳里面的内容，测试的时候建议不用MainLevel的场景
        //Debug.Log(shells.data[0].text);
        //shells的内部具体类可以看脚本Front：649-692行
    }*/
}