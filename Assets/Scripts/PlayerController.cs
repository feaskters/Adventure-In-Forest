using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class PlayerController : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 3f;
    public FloatingJoystick floatingJoystick;
    public Button jumpButton;
    public Image healthImage;//生命值UI
    public float invincibleTime = 1.5f;
    public SceneController sc;
    public Button muteButton;//静音按钮
    // public Sprite muteImage;
    // public Sprite unmuteImage;

    bool isRight;//角色朝向
    float invincibleTimer;//无敌倒计时
    bool isInvincible;//是否无敌
    int maxHealth = 3;//最大生命
    int currenthealth;//当前生命
    Rigidbody2D rBody;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        isInvincible = false;
        invincibleTimer = invincibleTime;
        currenthealth = maxHealth;
        rBody = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // jumpButton.onClick.AddListener(jump);
        //跳跃按钮添加点击事件
        EventTrigger trigger = jumpButton.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
        //静音按钮添加点击事件
        muteButton.onClick.AddListener(mute);
    }

    // Update is called once per frame
    void Update()
    {
        //设置音量图标
        // if (PlayerPrefs.GetInt("mute") == 0)
        // {
        //     muteButton.GetComponent<Image>().sprite = unmuteImage;
        // }else{
        //     muteButton.GetComponent<Image>().sprite = muteImage;
        // }
        //muteButton.GetComponent<Image>().fillAmount = PlayerPrefs.GetInt("mute");
        muteButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("mute") == 0 ? (Sprite)Resources.Load("unmute",typeof(Sprite) ): (Sprite)Resources.Load("mute",typeof(Sprite));
        float moveX = floatingJoystick.Horizontal;
        float moveY = floatingJoystick.Vertical;
        isRight = moveX > 0 ? true : false; 
        //角色左右移动
        animator.SetFloat("moveX",moveX);
        animator.SetFloat("moveY",rBody.velocity.y);
        var velo = rBody.velocity;
        velo.x = moveX * speed;
        rBody.velocity = velo;
        //脚步声
        
        //判断是否无敌
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
                invincibleTimer = invincibleTime;
            }
        }   
    }

    void jump(){
        if (rBody.velocity.y < 0.001 && rBody.velocity.y > -0.001)
        {
            rBody.AddForce(Vector2.up * jumpForce);
            AudioController.instance.jumpPlay();
        }
    }

    public void healthChange(int health){
        
        //受到伤害且不是无敌状态
        if (health < 0 && !isInvincible)
        {
            //播放音效
            AudioController.instance.damagePlay();
            //状态改为无敌
            isInvincible = true;
            currenthealth = Mathf.Clamp(currenthealth + health, 0 , 3);
            //修改血量图片
            healthImage.fillAmount = 0.117f * currenthealth + 0.25f;
            hurt();
        }
        //踩死怪物虽然不掉血，但是触发跳跃
        if (health == 0)
        {
            rBody.velocity = Vector2.zero;
            rBody.AddForce(new Vector2(isRight ? -0.5f : 0.5f, 0.7f) * jumpForce);
        }
        //吃到萝卜回血
        if (health > 0)
        {
            currenthealth = Mathf.Clamp(currenthealth + health, 0 , 3);
            //修改血量图片
            healthImage.fillAmount = 0.117f * currenthealth + 0.25f;
        }
    }

    //受到伤害动画并且被弹开
    void hurt(){
        rBody.velocity = Vector2.zero;
        rBody.AddForce(new Vector2(isRight ? -0.5f : 0.5f, 0.7f) * jumpForce);
        animator.SetTrigger("hurt");
    }
    //结束伤害动画,如果生命值为0则重新开始本关
    void hurtEnd(){
        animator.SetTrigger("unhurt");
        if (currenthealth == 0)
        {
            Destroy(gameObject);
            sc.reloadScene();
        }
    }
    
    void mute(){
        //设置静音
        PlayerPrefs.SetInt("mute", PlayerPrefs.GetInt("mute") == 0 ? 1 : 0);
    }

    public void OnPointerDownDelegate(PointerEventData eventData){
        jump();
    }
    
}
