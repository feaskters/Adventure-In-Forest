using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slugBossController : MonoBehaviour
{
    
    public float speed = 2f;
    public float actTime = 2f;
    public bool isRight = false;
    public bool chasePlayer = true;
    public float arrange = 30f;
    public float maxHealth = 50f;
    public float invincibleTime = 0.5f;//受伤后无敌时间
    public GameObject bossUI;
    public float attackTime = 5f;//攻击时间间隔
    public float jumpForce = 300f;//跳跃力度
    public GameObject spikegate;//打完boss后门打开
    float attackTimer;//攻击计时器
    bool isJumpAttackMode;//跳跃攻击模式，在背上也会受伤
    bool isCrashAttackMode;//冲刺攻击模式，取消追随角色
    float crashAttackTimer = 3f;
    float jumpAttackTimer = 0.1f;//跳跃攻击模式倒计时
    float invincibleTimer;
    float currentHealth;
    Color32 nowColor;
    public Image healthImage;

    bool isDead;
    Animator animator;
    Rigidbody2D rBody;
    float actTimer;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        actTimer = actTime;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
        invincibleTimer = invincibleTime;
        attackTimer = attackTime;
        //播放战斗音乐
        AudioController.instance.fightPlay();
        //初始化颜色
        nowColor = new Color32(255,255,255,255);
    }

    // Update is called once per frame
    void Update()
    {
        //人物与怪物的距离
        var distance = Vector2.Distance(player.transform.position, transform.position);
        //血量图片控制
        if(isDead) bossUI.SetActive(false);
        GetComponent<SpriteRenderer>().color = nowColor;
        healthImage.fillAmount = currentHealth / maxHealth;
        //无敌时间控制
        if (invincibleTimer < invincibleTime)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                invincibleTimer = invincibleTime;
            }
        }
        //跳跃攻击模式控制
        if (isJumpAttackMode)
        {
            GetComponent<SpriteRenderer>().color = new Color32(105,255,0,255);
            jumpAttackTimer -= Time.deltaTime;
            if (jumpAttackTimer <= 0)
            {
                jumpAttackTimer = 0.1f;
                isJumpAttackMode = false;
            }
        }
        //冲刺攻击模式控制
        if (isCrashAttackMode)
        {
            crashAttackTimer -= Time.deltaTime;
            if (crashAttackTimer <= 0)
            {
                crashAttackTimer = 3f;
                isCrashAttackMode = false;
            }
        }

        if (chasePlayer && distance < arrange)
        {
            //显示bossUI
            if (!isDead)
            {
                bossUI.SetActive(true);
            }
            //追随
            // rBody.velocity = Vector2.lerp(rBody.velocity, new Vector2(player.transform.position.x - transform.position.x,player.transform.position.y - transform.position.y).normalized * speed,0.1f);
            var velo = rBody.velocity;
            velo.x = rBody.transform.position.x - player.transform.position.x > 0 ? -speed * 2 : speed * 2;
            //如果处于冲刺攻击模式则不追随
            if(isCrashAttackMode) 
            {
                velo.x = rBody.velocity.x;
            }
            rBody.velocity = velo;
            animator.SetFloat("moveX", velo.x - player.transform.position.x < 0 ? 1 : 0);
            //攻击时间倒计时
            if (attackTimer < attackTime)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    attackTimer = attackTime;
                }
            }

            //攻击模式，如果角色在boss上方则采取跳跃攻击，否则采取冲刺攻击
            if (player.transform.position.y > transform.position.y && attackTime == attackTimer)
            {
                isJumpAttackMode = true;
                attackTimer -= Time.deltaTime;
                //跳跃
                // var velojump = new Vector2();
                // velojump.x = rBody.velocity.x;
                // velojump.y = jumpForce;
                // rBody.velocity = velojump;
                rBody.AddForce(Vector2.up * jumpForce);
            }else if (player.transform.position.y <= transform.position.y && attackTime == attackTimer)
            {//冲刺攻击
                isCrashAttackMode = true;
                attackTimer -= Time.deltaTime;
                var velo3 = rBody.velocity;
                velo3.x = rBody.transform.position.x - player.transform.position.x > 0 ? -jumpForce / 1.5f: jumpForce / 1.5f;
                velo3.y = 0;
                rBody.AddForce(velo3);
            }
        }else{
            //关闭bossUI
            bossUI.SetActive(false);
            actTimer -= Time.deltaTime;
            var position = rBody.position;
            if (isRight)
            {
                position.x += speed * Time.deltaTime;
                animator.SetFloat("moveX", 1);
            }else{
                position.x -= speed * Time.deltaTime;
                animator.SetFloat("moveX", 0);
            }
            rBody.MovePosition(position);
            if (actTimer <= 0f)
            {
                isRight = !isRight;
                actTimer = actTime;
            }
        }
        
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && !isDead)
        {
            //如果玩家正在下落且boss不处于跳跃攻击模式，则boss血减一
            if (other.gameObject.transform.position.y > transform.position.y && !isJumpAttackMode)
            {
                if (invincibleTimer == invincibleTime)
                {
                    invincibleTimer -= 0.01f;
                    //触发玩家的后跳动作
                    other.gameObject.GetComponent<PlayerController>().healthChange(0);
                    AudioController.instance.enemydeadPlay();
                    currentHealth -= 1;

                    //如果血量低于15进入第二阶段
                    if(currentHealth == 15){
                        nowColor = new Color32(253,21,21,255);
                        attackTime = 2f;
                        attackTimer = 2f;
                    }

                    if (currentHealth == 0)
                    {
                        //自身播放死亡动画
                        animator.SetTrigger("dead");
                        //重置重力和速度
                        rBody.gravityScale = 0;
                        rBody.velocity = Vector2.zero;
                        isDead = true;
                        GetComponent<Collider2D>().enabled = false;
                        rBody.Sleep();
                        //开启刺门
                        Destroy(spikegate);
                        //结束战斗音乐
                        AudioController.instance.fightEnd();
                        AudioController.instance.fightSuccessPlay();
                        Invoke("dead", 1f);
                    } 
                }
                
                
            }else{
                 other.gameObject.GetComponent<PlayerController>().healthChange(-1);
                
            }
        }
    }

    void dead(){
        Destroy(gameObject);
    }
}
