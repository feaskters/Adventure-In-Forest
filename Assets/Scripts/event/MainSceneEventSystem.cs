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
    public GameObject audioManager;
    public Font custonfont;
    void Start()
    {
        //给按钮添加点击事件
        newGame.onClick.AddListener(newgame);
        continueGame.onClick.AddListener(conitnuegame);
        tipNo.onClick.AddListener(() => {tip.GetComponent<Animator>().SetTrigger("TipOut");
            AudioController.instance.buttonPlay();});
        tipYes.onClick.AddListener(() => {SceneManager.LoadScene("1level");
            AudioController.instance.buttonPlay();});

        //判断继续游戏是否有效
        if (PlayerPrefs.GetInt("level") < 1)
        {
            continueGame.interactable = false;
        }else{
            continueGame.interactable = true;
        }
        if (! AudioController.isHaveClone)
        {
            audioManager = Instantiate(audioManager);
            DontDestroyOnLoad(audioManager);
            AudioController.isHaveClone = true;
        }
        //语言本地化
        string language = Application.systemLanguage.ToString();
        Debug.Log(language);
        if (language == "Chinese" || language == "cn" || language == "ChineseSimplified")
        {
            tipYes.GetComponentInChildren<Text>().text = "是";
            tipYes.GetComponentInChildren<Text>().font = custonfont;
            tipNo.GetComponentInChildren<Text>().text = "否";
            tipNo.GetComponentInChildren<Text>().font = custonfont;
            tip.GetComponentInChildren<Text>().text = "过去的数据将被清空，是否确定？";
            tip.GetComponentInChildren<Text>().font = custonfont;
            newGame.GetComponentInChildren<Text>().font = custonfont;
            newGame.GetComponentInChildren<Text>().text = "新游戏";
            continueGame.GetComponentInChildren<Text>().font = custonfont;
            continueGame.GetComponentInChildren<Text>().text = "继续游戏";
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void newgame(){
        AudioController.instance.buttonPlay();
        if (PlayerPrefs.GetInt("level") > 1)
        {
            tip.GetComponent<Animator>().SetTrigger("TipIn");
        }else{
            SceneManager.LoadScene("1level");
        }
    }

    void conitnuegame(){
        AudioController.instance.buttonPlay();
        SceneManager.LoadScene(PlayerPrefs.GetInt("level") + "level");
    }
}
