using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingFlatController : MonoBehaviour
{
    public bool isVerticle;
    public float speed;
    public float time;

    float timer;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        timer = time;
    }

    // Update is called once per frame
    void Update()
    {
        //方向控制
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }else{
            direction *= -1;
            timer = time;
        }
        //上下移动
        if (isVerticle)
        {
            transform.Translate(Vector3.up * direction * speed * Time.deltaTime);
        }else{
            transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            other.transform.Translate(Vector3.right * speed * Time.deltaTime * direction);
        }
    }
}
