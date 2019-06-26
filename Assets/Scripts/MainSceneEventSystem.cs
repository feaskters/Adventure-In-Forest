using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainSceneEventSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Button newGame;
    public Button continueGame;
    public GameObject tip;
    public Button tipYes;
    public Button tipNo;
    void Start()
    {
        //给按钮添加点击事件
        newGame.onClick.AddListener(newgame);
        continueGame.onClick.AddListener(conitnuegame);
        tipNo.onClick.AddListener(() => {tip.GetComponent<Animator>().SetTrigger("TipOut");});
        tipYes.onClick.AddListener(() => {SceneManager.LoadScene("1level");});

        //判断继续游戏是否有效
        if (PlayerPrefs.GetInt("level") < 1)
        {
            continueGame.interactable = false;
        }else{
            continueGame.interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void newgame(){
        if (PlayerPrefs.GetInt("level") > 1)
        {
            tip.GetComponent<Animator>().SetTrigger("TipIn");
        }else{
            SceneManager.LoadScene("1level");
        }
    }

    void conitnuegame(){
        SceneManager.LoadScene(PlayerPrefs.GetInt("level") + "level");
    }
}
