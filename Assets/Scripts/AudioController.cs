using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance {get;private set;}
    public AudioClip damage;
    public AudioClip collect;
    public AudioClip buttonClick;
    public AudioClip enemydead;
    public AudioClip step;
    public AudioClip jump;
    public AudioClip jumpEnd;
    public AudioSource theAs;
    public static bool isHaveClone = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void collectPlay(){
        theAs.PlayOneShot(collect);
    }

    public void damagePlay(){
        theAs.PlayOneShot(damage);
    }

    public void stepPlay(){
        
    }

    public void enemydeadPlay(){
        theAs.PlayOneShot(enemydead);
    }

    public void buttonPlay(){
        theAs.PlayOneShot(buttonClick);
    }

    public void jumpPlay(){
        theAs.PlayOneShot(jump);
    }

    public void jumpEndPlay(){
        theAs.PlayOneShot(jumpEnd);
    }
}
