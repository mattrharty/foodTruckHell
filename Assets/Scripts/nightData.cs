using System;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class nightData
{

    [SerializeField] private string name;
    [SerializeField] private float startDelay;
    [SerializeField] private float batteryLife;
    [SerializeField] private float cookTime;
    [SerializeField] private string weather;
    //Buns, cheese, and patties are always available
    [SerializeField] private string[] stationsAvailable;
    [SerializeField] private ZombieWave[] waves;
    [SerializeField] private int zombAI;

    [SerializeField] private int current;

    public nightData()
    {
        name = "Monday Night";
        startDelay = 12.0f;
        batteryLife = 100.0f;
        cookTime = 1.0f;
        weather = "normal";
        stationsAvailable = new string[] {"lettuce", "pickles", "tomatos", "soda", "frier"};
        waves = new ZombieWave[0];
        zombAI = 1;

        current = 0;
    }

    public ZombieWave getWave(int index)
    {
        if(index < 0 || index >= waves.Length)
            return null;
        return waves[index];
    }

    public ZombieWave getNextWave()
    {
        if(current >= waves.Length)
            return null;
        ZombieWave result = waves[current];
        current++;
        return result;
    }

    public bool hasNextWave()
    {
        if(current >= waves.Length)
            return false;
        return true;
    }

    public string getName()
    {
        return name;
    }

    public float getStartDelay()
    {
        return startDelay;
    }

    public float getBatteryLife()
    {
        return batteryLife;
    }

    public float getCookTime()
    {
        return cookTime;
    }

    public string getWeather()
    {
        return weather;
    }

    public string[] getAvailableStations()
    {
        return stationsAvailable;
    }

    public bool isStationAvailable(string other)
    {
        foreach(string str in stationsAvailable)
            if(str.Equals(other))
                return true;
        return false;
    }

    public int getZombAI()
    {
        return zombAI;
    }

}
