using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    private bool gamePaused = true;
    private GlobalController global;
    private SettingsMenu settingsMenu;
    private playerController player;

    public void Awake()
    {
        global = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>();
        settingsMenu = GetComponentInChildren<SettingsMenu>();
        settingsMenu.settings = global.getSettings();

        closeSettings();
        resume();
    }

    public bool pauseGame()
    {
        this.player = player;
        if (settingsMenu.gameObject.activeSelf)
        {
            closeSettings();
            return false;
        }
        gamePaused = !gamePaused;
        if(gamePaused){
            Time.timeScale = 0.0f;
            transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            Time.timeScale = 1.0f;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        return gamePaused;
    }

    public void resume()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().pauseGame();
    }

    public void settings()
    {
        settingsMenu.gameObject.SetActive(true);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
    }

    public void closeSettings()
    {
        settingsMenu.settings.saveToJSON(Application.persistentDataPath);
        settingsMenu.gameObject.SetActive(false);
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
