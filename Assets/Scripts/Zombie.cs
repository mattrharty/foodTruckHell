using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{

    private int hunger;
    private int speed;
    private NightController control;

    public void Start()
    {
        hunger = 100;
    }

    public void setHunger(int _hunger)
    {
        hunger = _hunger;
    }

    public bool eatFood(int foodVal)
    {
        hunger -= foodVal;
        return hunger <= 0;
    }

    public void setControl(NightController _control)
    {
        control = _control;
    }

    public void Update()
    {
        if(transform.position.z < 6){
            control.die();
            Destroy(gameObject);
        }
    }

}
