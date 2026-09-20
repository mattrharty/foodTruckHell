using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalController : MonoBehaviour
{

    private saveFile save;

    [SerializeField] private TextAsset[] nights;


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
