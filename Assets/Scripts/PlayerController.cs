using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 3f;
    public FloatingJoystick floatingJoystick;
    public Button jumpButton;
    public Image healthImage;


    int maxHealth = 3;
    int currenthealth;
    Rigidbody2D rBody;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currenthealth = maxHealth;
        rBody = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpButton.onClick.AddListener(jump);
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = floatingJoystick.Horizontal;
        float moveY = floatingJoystick.Vertical;
        //角色左右移动
        animator.SetFloat("moveX",moveX);
        animator.SetFloat("moveY",rBody.velocity.y);
        var velo = rBody.velocity;
        velo.x = moveX * speed;
        rBody.velocity = velo;
        
    }

    void jump(){
        if (rBody.velocity.y < 0.001 && rBody.velocity.y > -0.001)
        {
            rBody.AddForce(Vector2.up * jumpForce);
        }
    }

    public void healthChange(int health){
        currenthealth = Mathf.Clamp(currenthealth + health, 0 , 3);
        //修改血量图片
        healthImage.fillAmount = 0.117f * currenthealth + 0.25f;
        if (health < 0)
        {
            hurt();
        }
    }

    //受到伤害动画
    void hurt(){
        rBody.AddForce(Vector2.left * jumpForce);
        animator.SetTrigger("hurt");
    }
    //结束伤害动画
    void hurtEnd(){
        animator.SetTrigger("unhurt");
    }
}
