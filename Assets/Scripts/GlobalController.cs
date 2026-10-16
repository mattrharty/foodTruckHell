using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A controller script that contains 
/// </summary>
public class GlobalController : MonoBehaviour
{

    /// <summary>
    /// The currently loaded save file.
    /// </summary>
    private saveFile save;

    /// <summary>
    /// An array of text assets containing JSON-serialized instances of <c>nightData</c>.
    /// </summary>
    [Tooltip("An array of text assets containing JSON-serialized instances of nightData.")]
    [SerializeField] private TextAsset[] nights;

    /// <summary>
    /// The audio mixer controlling the volume of all sounds and music.
    /// </summary>
    [Tooltip("The audio mixer controlling the volume of all sounds and music.")]
    public AudioMixer mixer;

    /// <summary>
    /// The current mode of turning. Must be either "hover" or "click".
    /// </summary>
    [NonSerialized] public string turnMode = "hover";

    /// <summary>
    /// The settings profile containing set preferences for all settings.
    /// </summary>
    private settingsProfile settings;

    /// <summary>
    /// Runs on class initialization; ensures there is only one instance of this class and loads save and settings.
    /// </summary>
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("global");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        loadFile();
        loadSettings();
        save = new saveFile();
    }

    /// <summary>
    /// Saves the current save file to the user's computer.
    /// </summary>
    public void saveToFile()
    {
        string path = Application.persistentDataPath;
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "burgerSave.json"), JsonUtility.ToJson(save));
    }

    /// <summary>
    /// Loads the save file from the user's computer, if a save file is present.
    /// </summary>
    /// <returns><c>true</c> if there is a directory and save file, otherwise <c>false</c></returns>
    public bool loadFile()
    {
        string path = Application.persistentDataPath;
        if(!Directory.Exists(path) || !File.Exists(Path.Combine(path, "burgerSave.json")))
            return false;
        save = JsonUtility.FromJson<saveFile>(File.ReadAllText(Path.Combine(path, "burgerSave.json")));
        return true;
    }

    /// <summary>
    /// Loads the settings profile from the user's computer; if there is no settings file, one is created.
    /// </summary>
    public void loadSettings()
    {
        string path = Application.persistentDataPath;
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        if(!File.Exists(Path.Combine(path, "settings.json")))
        {
            Debug.Log("not settings file exists");
            settings = new settingsProfile(mixer, this, path);
            File.WriteAllText(Path.Combine(path, "settings.json"), settings.toJSON());
        }
        settings = JsonUtility.FromJson<settingsProfile>(File.ReadAllText(Path.Combine(path, "settings.json")));
        settings.setVars(mixer, this, path);
        settings.refresh();
    }

    /// <summary>
    /// Loads and returns the current settings profile.
    /// </summary>
    /// <returns>The current <c>settings</c></returns>
    public settingsProfile getSettings()
    {
        string path = Application.persistentDataPath;
        loadSettings();
        settings.setVars(mixer, this, path);
        return settings;
    }

    /// <summary>
    /// Deserializes and returns the <c>nightData</c> for the current night.
    /// </summary>
    /// <returns>The <c>nightData</c> for the current night.</returns>
    public nightData getNightData()
    {
        return JsonUtility.FromJson<nightData>(nights[save.currentNight].text);
    }

    /// <summary>
    /// Returns the current night number.
    /// </summary>
    /// <returns><c>currentNight</c> from <c>save</c></returns>
    public int getNightNum()
    {
        return save.currentNight;
    }

    /// <summary>
    /// Increases the current night number and saves it to the save file.
    /// </summary>
    public void incrementNightNum()
    {
        if(save == null)
        {
            save = new saveFile();
        }
        save.currentNight++;
        saveToFile();
    }

    /// <summary>
    /// Sets the current night number to a given number and saves it to the save file.
    /// </summary>
    /// <param name="_nightNum">The number for <c>nightNum</c> to be set to.</param>
    public void setNightNum(int _nightNum)
    {
        if(save == null)
        {
            save = new saveFile();
        }
        save.currentNight = _nightNum;
        saveToFile();
    }

}
