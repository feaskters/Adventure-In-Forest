using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeeBossController : MonoBehaviour
{
    public float speed = 2f;
    public GameObject bee;//召唤物小蜜蜂
    public float actTime = 2f;
    public bool isRight = false;
    public bool chasePlayer = true;
    public float arrange = 30f;
    public float maxHealth = 50f;
    public float invincibleTime = 0.5f;//受伤后无敌时间
    public GameObject bossUI;
    public float attackTime = 5f;//攻击时间间隔
    public float Force = 300f;//冲刺力度
    public GameObject spikegate;//打完boss后门打开
    public int beeCount = 3;//召唤蜜蜂的数量
    float attackTimer;//攻击计时器
    float invincibleTimer;
    float currentHealth;
    Color32 nowColor;
    public Image healthImage;

    bool isCrashAttackMode = false;
    float crashAttackTimer;//冲刺攻击计时器
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
        crashAttackTimer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //人物与怪物的距离
        var distance = Vector2.Distance(player.transform.position, transform.position);
        var direction_x = transform.position.x - player.transform.position.x + (transform.position.x > 15 ? 10 : -10) > 0 ? -1 : 1;
        var direction_y = transform.position.y - 10 > 0 ? -1 : 1;
        GetComponent<SpriteRenderer>().color = nowColor;
        //血量图片控制
        if(isDead) bossUI.SetActive(false);
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
        // //跳跃攻击模式控制
        // if (isJumpAttackMode)
        // {
        //     GetComponent<SpriteRenderer>().color = new Color32(105,255,0,255);
        //     jumpAttackTimer -= Time.deltaTime;
        //     if (jumpAttackTimer <= 0)
        //     {
        //         jumpAttackTimer = 0.1f;
        //         isJumpAttackMode = false;
        //         GetComponent<SpriteRenderer>().color = nowColor;
        //     }
        // }
        // //冲刺攻击模式控制
        if (isCrashAttackMode)
        {
            crashAttackTimer -= Time.deltaTime;
            if (crashAttackTimer <= 0)
            {
                crashAttackTimer = 3f;
                isCrashAttackMode = false;
                GetComponent<Collider2D>().isTrigger = true;
                Invoke("recover",1);
            }
        }
        if (!isCrashAttackMode)
            {
                // var pos = rBody.position;
                // pos.x = player.transform.position.x + 10;
                // pos.y = 10;
                rBody.velocity = Vector2.zero;
                rBody.transform.Translate(new Vector3(direction_x * speed * Time.deltaTime ,direction_y * speed * Time.deltaTime,0));
            }

        if (chasePlayer && distance < arrange)
        {
            //显示bossUI
            if (!isDead)
            {
                bossUI.SetActive(true);
            }
            //追随于角色上方
            // rBody.velocity = Vector2.lerp(rBody.velocity, new Vector2(player.transform.position.x - transform.position.x,player.transform.position.y - transform.position.y).normalized * speed,0.1f);
            
            //如果处于冲刺攻击模式则不追随
            // if(isCrashAttackMode) 
            // {
            //     pos.x = rBody.velocity.x;
            // }
            // rBody.MovePosition(pos * Time.deltaTime * speed);
            
            animator.SetFloat("moveX", rBody.position.x - player.transform.position.x > 0 ? 0 : 1);
            //攻击时间倒计时
            if (attackTimer < attackTime)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    attackTimer = attackTime;
                }
            }

            //随机攻击模式0=>召唤,>1=>冲刺
            int attackMode = Random.Range(0,3);
            if (attackMode == 0 && attackTime == attackTimer)
            {
                attackTimer -= Time.deltaTime;
                //召唤三只小蜜蜂
                for (int i = 0; i < beeCount; i++) 
                {
                    var pos = new Vector3(Random.Range(-5,5) + transform.position.x ,Random.Range(3,5) + transform.position.y,0);    
                    Instantiate(bee, pos, Quaternion.identity);
                }
                }

             if (attackMode >= 1 && attackTime == attackTimer)
            {
                attackTimer -= Time.deltaTime;
                isCrashAttackMode = true;
                //朝着角色的方向
                var dir = player.GetComponent<Rigidbody2D>().position - rBody.position;
                // rBody.AddForce(dir * Force * 10);
                rBody.velocity = dir * 2;
            }

            // //攻击模式，如果角色在boss上方则采取跳跃攻击，否则采取冲刺攻击
            // if (player.transform.position.y > transform.position.y && attackTime == attackTimer)
            // {
            //     attackTimer -= Time.deltaTime;
            //     //跳跃
            //     // var velojump = new Vector2();
            //     // velojump.x = rBody.velocity.x;
            //     // velojump.y = jumpForce;
            //     // rBody.velocity = velojump;
            //     rBody.AddForce(Vector2.up * jumpForce);
            // }else if (player.transform.position.y <= transform.position.y && attackTime == attackTimer)
            // {//冲刺攻击
            //     attackTimer -= Time.deltaTime;
            //     var velo3 = rBody.velocity;
            //     velo3.x = rBody.transform.position.x - player.transform.position.x > 0 ? -jumpForce / 1.5f: jumpForce / 1.5f;
            //     velo3.y = 0;
            //     rBody.AddForce(velo3);
            // }
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

    //恢复为可碰撞
    void recover(){
        GetComponent<Collider2D>().isTrigger = false;
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && !isDead)
        {
            //如果玩家正在下落且boss不处于跳跃攻击模式，则boss血减一
            if (other.gameObject.transform.position.y > transform.position.y)
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
                        attackTime = 3f;
                        attackTimer = 3f;
                        beeCount = 5;
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
