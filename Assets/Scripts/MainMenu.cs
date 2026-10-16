using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// A class that controls the main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{

    /// <summary>
    /// A reference to the "continue" button.
    /// </summary>
    [Tooltip("A reference to the \"continue\" button.")]
    [SerializeField] Button cont;

    [Space]
    /// <summary>
    /// The controller of the settings menu.
    /// </summary>
    [Tooltip("The controller of the settings menu.")]
    [SerializeField] SettingsMenu settingsMenu;

    /// <summary>
    /// A reference to the global controller.
    /// </summary>
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

    /// <summary>
    /// Sets the night number to 0 and loads the game; runs when "New Game" button is pressed.
    /// </summary>
    public void newGame()
    {
        global.setNightNum(0);
        play();
    }
    
    /// <summary>
    /// Loads the night scene.
    /// </summary>
    public void play(){
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Opens the settings menu and hides the main menu.
    /// </summary>
    public void settings()
    {
        settingsMenu.gameObject.SetActive(true);
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    /// <summary>
    /// Closes the settings menu and shows the main menu.
    /// </summary>
    public void closeSettings()
    {
        settingsMenu.settings.saveToJSON(Application.persistentDataPath);
        settingsMenu.gameObject.SetActive(false);
        for(int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Closes the game.
    /// </summary>
    public void quit()
    {
        Application.Quit();
    }
}
