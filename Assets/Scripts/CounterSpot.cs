using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CounterSpot : MonoBehaviour
{

    //Food attributes
    private List<Ingred> ingreds;
    private Soda soda;
    private Fries fries;
    private float incTotal;
    private float inc;

    [SerializeField] NightController control;

    public void Start()
    {
        ingreds = new List<Ingred>();
        inc = 0.8f;
        incTotal = -0.3f;
    }

    public void ding()
    {
        if(!control.zombiePresent(transform.GetSiblingIndex()))
            return;
        gameObject.GetComponent<Animator>().SetTrigger("orderUp");
        control.orderUp(transform.GetSiblingIndex(), transform.GetChild(0).gameObject);
    }
    
    public float addFood(GameObject obj)
    {
        if(obj.GetComponent<Fries>() != null)
        {
            if(fries != null)
                return -1;
            fries = obj.GetComponent<Fries>();
        }
        else if(obj.GetComponent<Soda>() != null)
        {
            if(soda != null)
                return -1;
            soda = obj.GetComponent<Soda>();
        }
        else if(obj.GetComponent<Ingred>() != null)
        {
            if(obj.GetComponent<Ingred>().getName() == IngredType.burnt_patty)
            {
                return -1;
            }
            if(obj.GetComponent<Ingred>().getName() != IngredType.bun)
            {
                if(!containsIngred(IngredType.bun))
                    return -1;
            } else if(containsIngred(IngredType.bun))
                return -1;
            obj.GetComponent<Ingred>().plate(ingreds.Count);
            ingreds.Add(obj.GetComponent<Ingred>());
            //deparentFood();
            incTotal += inc;
            //sDebug.Log(incTotal);
            obj.transform.position = new Vector3(0, incTotal, 0);
            inc = 0.4f;
            if((int)obj.GetComponent<Ingred>().getName() <= 3)
                inc = 0.8f;
        }
        else return -1;
        return incTotal;
    }

    public void deparentFood()
    {
        if(ingreds.Count <= 0)
            return;
        for(int i = transform.GetChild(0).childCount - 1; i >= 0; i--)
            transform.GetChild(0).GetChild(0).parent = transform;
    }

    public int calculateFoodValue()
    {
        int val = 0;

        //Add up val based on ingrediants
        foreach(Ingred ing in ingreds){
            if((int)ing.getName() > 3)
                val += 5;
            if(ing.getName() == IngredType.cooked_patty)
                val += 80;
            if(ing.getName() == IngredType.bun)
                val += 15;
        }

        return val;
    }

    public bool containsIngred(IngredType type)
    {
        foreach(Ingred ing in ingreds)
            if(ing.getName() == type)
                return true;
        return false;
    }

    public void clear()
    {
        ingreds = new List<Ingred>();
        incTotal = -0.3f;
    }

}
