using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button cont;
    private GlobalController global;

    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>();
        if(global.loadFile()){
            cont.interactable = true;
            cont.transform.GetChild(1).GetComponent<TMP_Text>().text = global.getNightNum() + "";
        }
        else{
            cont.interactable = false;
            cont.transform.GetChild(1).GetComponent<TMP_Text>().text = " ";
        }
    }

    public void newGame()
    {
        global.setNightNum(1);
        play();
    }
    
    public void play(){
        SceneManager.LoadScene(1);
    }
    
    public void quit()
    {
        Application.Quit();
    }
}
