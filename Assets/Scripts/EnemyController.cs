using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 2f;
    public float actTime = 2f;
    public bool isRight = false;
    public bool chasePlayer = false;
    public float arrange = 6f;

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
    }

    // Update is called once per frame
    void Update()
    {
        //人物与怪物的距离
        var distance = Vector2.Distance(player.transform.position, transform.position);

        if (chasePlayer && distance < arrange)
        {
            //追随
            // rBody.velocity = Vector2.lerp(rBody.velocity, new Vector2(player.transform.position.x - transform.position.x,player.transform.position.y - transform.position.y).normalized * speed,0.1f);
        }else{
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
            //如果玩家正在下落，则怪物被踩死
            if (other.gameObject.transform.position.y > transform.position.y + 1)
            {
                //触发玩家的后跳动作
                other.gameObject.GetComponent<PlayerController>().healthChange(0);
                //自身播放死亡动画
                animator.SetTrigger("dead");
                isDead = true;
                GetComponent<Collider2D>().enabled = false;
                rBody.Sleep();
                Invoke("dead", 1f);
            }else{
                other.gameObject.GetComponent<PlayerController>().healthChange(-1);
            }
        }
    }

    void dead(){
        Destroy(gameObject);
    }
}
