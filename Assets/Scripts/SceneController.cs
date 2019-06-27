using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Button back;
    public GameObject userData;
    public Animator transition;

    string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(backToMain);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reloadScene(){
        sceneName = userData.name + "level";
        transition.SetTrigger("out");
        Invoke("animationEnded", 1f);
    }

    void backToMain(){
        AudioController.instance.buttonPlay();
        sceneName = "MainScene";
        transition.SetTrigger("out");
        Invoke("animationEnded", 1f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            sceneName = (int.Parse(userData.name)+ 1) + "level";
            transition.SetTrigger("out");
            Invoke("animationEnded", 1f);
        }
    }

    void animationEnded(){
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception)
        {
            SceneManager.LoadScene("MainScene");
            throw;
        }
    }
}
