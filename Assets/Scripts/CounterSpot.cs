using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CounterSpot : MonoBehaviour
{

    //Food attributes
    private Burger burger;
    private Soda soda;
    private Fries fries;

    [SerializeField] NightController control;

    public void Start()
    {

    }

    public void ding()
    {
        //Debug.Log("Rang bell for counter " + transform.parent.GetSiblingIndex());
        if(!control.zombiePresent(transform.parent.GetSiblingIndex()))
            return;
        transform.parent.gameObject.GetComponent<Animator>().SetTrigger("orderUp");
        control.orderUp(transform.parent.GetSiblingIndex(), gameObject);
    }

    public int calculateFoodValue()
    {
        int val = 0;

        if(burger != null)
            val += burger.getFoodVal();
        if(fries != null)
            val += fries.getFoodVal();
        if(soda != null)
            val += soda.getFoodVal();

        return val;
    }

    public void clear(Transform target)
    {
        if(transform.GetChild(0).childCount > 0)
            transform.GetChild(0).GetChild(0).parent = transform.root;
        if(transform.GetChild(1).childCount > 0)
            transform.GetChild(1).GetChild(0).parent = transform.root;
        if(transform.GetChild(2).childCount > 0)
            transform.GetChild(2).GetChild(0).parent = transform.root;

        if(burger != null)
            StartCoroutine(burger.sendOut(target));
        /*if(fries != null)
            StartCoroutine(fries.sendOut(target));
        if(fries != null)
            StartCoroutine(soda.sendOut(target));*/
    }

    public bool setBurger(Burger bur)
    {
        if(burger != null)
            return false;
        burger = bur;
        return true;
    }

    public bool setFries(Fries f)
    {
        if(fries != null)
            return false;
        fries = f;
        return true;
    }

    public bool setSoda(Soda s)
    {
        if(soda != null)
            return false;
        soda = s;
        return true;
    }

}
