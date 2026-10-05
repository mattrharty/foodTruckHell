using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Button cont;
    [SerializeField] SettingsMenu settingsMenu;
    private GlobalController global;

    // Start is called before the first frame update
    void Start()
    {
        global = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>();
        settingsMenu.settings = global.getSettings();
        closeSettings();

        if(global.loadFile()){
            cont.interactable = true;
            cont.transform.GetChild(1).GetComponent<TMP_Text>().text = global.getNightNum() + 1 + "";
        }
        else{
            cont.interactable = false;
            cont.transform.GetChild(1).GetComponent<TMP_Text>().text = " ";
        }
    }

    public void newGame()
    {
        global.setNightNum(0);
        play();
    }
    
    public void play(){
        SceneManager.LoadScene(1);
    }

    public void settings()
    {
        settingsMenu.gameObject.SetActive(true);
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
            t.gameObject.SetActive(false);
    }

    public void closeSettings()
    {
        settingsMenu.settings.saveToJSON(Application.persistentDataPath);
        settingsMenu.gameObject.SetActive(false);
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
            t.gameObject.SetActive(true);
    }
    
    public void quit()
    {
        Application.Quit();
    }
}
