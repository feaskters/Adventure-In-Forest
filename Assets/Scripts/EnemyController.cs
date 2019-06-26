using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float speed = 2f;
    public float actTime = 2f;
    public bool isRight = false;

    Animator animator;
    Rigidbody2D rBody;
    float actTimer;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rBody = GetComponent<Rigidbody2D>();
        actTimer = actTime;
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().healthChange(-1);
        }    
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().healthChange(-1);
        }
    }
}
