using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burger : MonoBehaviour
{
    
    private List<Ingred> ingreds;
    private bool canEdit;

    public Burger()
    {
        ingreds = new List<Ingred>();
        canEdit = true;
    }

    public List<Ingred> getIngreds()
    {
        return ingreds;
    }

    public bool addIngred(GameObject obj)
    {
        if(obj.GetComponent<Ingred>() == null)
            return false;
        Ingred ing = obj.GetComponent<Ingred>();
        if (isValid(ing.getName()))
        {
            ingreds.Add(ing);
            return true;
        }
        else
            return false;
    }

    private bool isValid(IngredType type)
    {
        List<IngredType> blacklist = new List<IngredType> {IngredType.burnt_patty, IngredType.patty};
        List<IngredType> whitelist = new List<IngredType>();
        if(hasIngred(IngredType.bun))
            blacklist.Add(IngredType.bun);
        else
            whitelist.Add(IngredType.bun);
        if(whitelist.Count > 0)
            return whitelist.Contains(type);
        return !blacklist.Contains(type);
    }

    public void Update()
    {
        if (!canEdit)
        {
            //change burger to foiled burger
        }

        gameObject.GetComponent<BoxCollider>().size = new Vector3(1.0f, 1.0f + getHeight() * 2, 0.2f);
    }

    public bool hasIngred(IngredType type)
    {
        foreach(Ingred i in ingreds)
            if(i.getName().Equals(type))
                return true;
        return false;
    }

    public bool getStatus()
    {
        return canEdit;
    }

    public void setStatus(bool status)
    {
        canEdit = status;
    }

    public float getHeight()
    {
        float h = 0.0f;
        if(ingreds.Count <= 1)
            return 0.0f;
        for(int i = 0; i < ingreds.Count - 1; i++)
        {
            Ingred ing = ingreds[i];
            if(ing.getName() == IngredType.bun)
                h += 0.175f;
            else if(ing.getName() == IngredType.cooked_patty)
                h += 0.15f;
            else
                h += 0.1f;
        }
        return h;  
    }

}
