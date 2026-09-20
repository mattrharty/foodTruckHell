using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ZombieWave
{

    [SerializeField] private float minDelay;
    [SerializeField] private float percentDeadToSpawn;
    [SerializeField] private string[] zombies;

    [SerializeField] private int current = 0;

    public ZombieWave()
    {
        minDelay = 3.0f;
        percentDeadToSpawn = 0.50f;
        zombies = new string[0];
    }

    public float getMinDelay()
    {
        return minDelay;
    }

    public float getPercentDeadToSpawn()
    {
        return percentDeadToSpawn;
    }

    public bool isReadyToSpawn(float timeSinceLastWave, float percentDead)
    {
        return timeSinceLastWave > minDelay && percentDead >= percentDeadToSpawn;
    }

    public string[] getZombies()
    {
        return zombies;
    }

    public string getNextZombie()
    {
        if(current >= zombies.Length)
            return null;
        string zom = zombies[current];
        current++;
        return zom;
    }

    public bool hasNextZombie()
    {
        if(current >= zombies.Length)
            return false;
        return true;
    }

    public bool isFirstZombie()
    {
        return current == 0;
    }

}
