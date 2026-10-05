using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

public class settingsProfile
{

    // Master Volume
    public float _masterVolume;
    public float masterVolume
    {
        get => _masterVolume;
        set { _masterVolume = value; refresh(); }
    }

    // Sound Volume
    public float _soundVolume;
    public float soundVolume
    {
        get => _soundVolume;
        set { _soundVolume = value; refresh(); }
    }

    // Music Volume
    public float _musicVolume;
    public float musicVolume
    {
        get => _musicVolume;
        set { _musicVolume = value; refresh(); }
    }

    // Turn Mode
    public string _turnMode;
    public string turnMode
    {
        get => _turnMode;
        set { _turnMode = value; refresh(); }
    }

    // Display
    public string _display;
    public string display
    {
        get => _display;
        set { _display = value; refresh(); }
    }

    // Resolution
    public int[] _resolution;
    public int[] resolution
    {
        get => _resolution;
        set { _resolution = value; refresh(); }
    }

    private string path;
    private AudioMixer mixer;
    [NonSerialized]
    public GlobalController global;

    public settingsProfile()
    {
        masterVolume = 0.4f;
        soundVolume = 0.4f;
        musicVolume = 0.4f;
        turnMode = "hover";
        display = "fullscreen";
        resolution = new int[] {Screen.currentResolution.width, Screen.currentResolution.height};       
    }

    public settingsProfile(AudioMixer mixer, GlobalController global, string path)
    {
        this.mixer = mixer;
        this.global = global;
        this.path = path;   

        masterVolume = 0.4f;
        soundVolume = 0.4f;
        musicVolume = 0.4f;
        turnMode = "hover";
        display = "fullscreen";
        resolution = new int[] {Screen.currentResolution.width, Screen.currentResolution.height};     
    }

    public void setVars(AudioMixer mixer, GlobalController global, string path)
    {
        this.mixer = mixer;
        this.global = global;
        this.path = path;    
    }

    public string toJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public void saveToJSON(string path)
    {
        if(path == null || global == null)
            return;
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "settings.json"), JsonUtility.ToJson(this));
    }

    public FullScreenMode getScreenMode()
    {
        if(display.Equals("fullscreen"))
            return FullScreenMode.ExclusiveFullScreen;
        if(display.Equals("borderless"))
            return FullScreenMode.FullScreenWindow;
        return FullScreenMode.Windowed;
    }

    public void refresh()
    {
        saveToJSON(path);

        if(global == null)
            return;

        // Master Volume
        float db = Mathf.Log10(masterVolume) * 20;
        global.mixer.SetFloat("masterVol", db);

        // SoundFX Volume
        db = Mathf.Log10(soundVolume) * 20;
        global.mixer.SetFloat("soundVol", db);

        // Music Volume
        db = Mathf.Log10(musicVolume) * 20;
        global.mixer.SetFloat("musicVol", db);

        // Turn Mode
        global.turnMode = turnMode;

        // Display Mode / Resolution
        Screen.SetResolution(resolution[0], resolution[1], getScreenMode());

    }

    public int getIndexTurnMode(string str)
    {
        return str.Equals("hover") ? 0 : 1;
    }

    public int getIndexDisplay(string str)
    {
        List<string> results = new List<string> {"fullscreen", "borderless", "windowed"};
        return results.IndexOf(str);
    }

    public string getResolution()
    {
        return string.Format("{0} x {1}", resolution[0], resolution[1]);
    }

}
