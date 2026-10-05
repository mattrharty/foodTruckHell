using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField]
    private Selectable[] options;

    [SerializeField]
    private TMP_Dropdown res;

    public settingsProfile settings;

    public virtual void back()
    {
        transform.parent.parent.GetComponent<PauseMenu>().closeSettings();
    }

    void OnEnable()
    {
        foreach(Selectable s in options)
        {
            if(s is Slider slide)
                slide.onValueChanged.AddListener((float f) => {setSettings(slide.gameObject.name, f);});
            if(s is TMP_Dropdown drop)
                drop.onValueChanged.AddListener((int i) => {setSettings(drop.gameObject.name, i);});
        }

        List<String> strs = new List<String>();
        Resolution[] r = Screen.resolutions;
        for(int i = 0; i < r.Length; i++)
            if (i == 0 || !(r[i].width == r[i - 1].width && r[i].height == r[i - 1].height))
                strs.Add(string.Format("{0} x {1}", r[i].width, r[i].height));
        res.ClearOptions();
        res.AddOptions(strs);

        if(settings == null)
            return;
        //Updates menu items;

        GlobalController global = GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>();
        string path = Application.persistentDataPath;
        settings.setVars(global.mixer, global, path);

        foreach(Selectable s in options)
        {
            if (s.gameObject.name.Equals("masterVolume"))
                ((Slider)s).value = settings.masterVolume;                
            if (s.gameObject.name.Equals("soundVolume"))
                ((Slider)s).value = settings.soundVolume;
            if (s.gameObject.name.Equals("musicVolume"))
                ((Slider)s).value = settings.musicVolume;
            if (s.gameObject.name.Equals("turnMode"))
                ((TMP_Dropdown)s).value = settings.getIndexTurnMode(settings.turnMode);
            if (s.gameObject.name.Equals("display"))
                ((TMP_Dropdown)s).value = settings.getIndexDisplay(settings.display);
            if (s.gameObject.name.Equals("resolution"))
                ((TMP_Dropdown)s).value = ((TMP_Dropdown)s).options.FindIndex(option => option.text == settings.getResolution());
        }
    }

    private void setSettings(string option, float f)
    {
        if(option.Equals("masterVolume"))
            settings.masterVolume = f;
        if(option.Equals("soundVolume"))
            settings.soundVolume = f;
        if(option.Equals("musicVolume"))
            settings.musicVolume = f;
    }

    private void setSettings(string option, int i)
    {
        if(option.Equals("turnMode"))
            settings.turnMode = i == 0 ? "hover" : "click";
        if(option.Equals("display"))
            settings.display = i == 0 ? "fullscreen" : i == 1 ? "borderless" : "windowed";
        if(option.Equals("resolution"))
            settings.resolution = new int[] {Screen.resolutions[i].width, Screen.resolutions[i].height};
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
