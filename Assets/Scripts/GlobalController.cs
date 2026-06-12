using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalController : MonoBehaviour
{

    private saveFile save;


    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("global");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        loadFile();
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

    public int getNightNum()
    {
        return save.currentNight;
    }

    public void setNightNum(int n)
    {
        if(save == null)
        {
            save = new saveFile();
        }
        save.currentNight = n;
        saveToFile();
    }

}
