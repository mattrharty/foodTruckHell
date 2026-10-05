using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class GlobalController : MonoBehaviour
{

    private saveFile save;

    [SerializeField] private TextAsset[] nights;
    public AudioMixer mixer;

    public string turnMode = "hover";

    private settingsProfile settings;


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

    public void saveToFile()
    {
        string path = Application.persistentDataPath;
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "burgerSave.json"), JsonUtility.ToJson(save));
    }

    public bool loadFile()
    {
        string path = Application.persistentDataPath;
        if(!Directory.Exists(path) || !File.Exists(Path.Combine(path, "burgerSave.json")))
            return false;
        save = JsonUtility.FromJson<saveFile>(File.ReadAllText(Path.Combine(path, "burgerSave.json")));
        return true;
    }

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

    public settingsProfile getSettings()
    {
        string path = Application.persistentDataPath;
        loadSettings();
        settings.setVars(mixer, this, path);
        return settings;
    }

    public nightData getNightData()
    {
        return JsonUtility.FromJson<nightData>(nights[save.currentNight].text);
    }

    public int getNightNum()
    {
        return save.currentNight;
    }

    public void incrementNightNum()
    {
        if(save == null)
        {
            save = new saveFile();
        }
        save.currentNight++;
        saveToFile();
    }

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
