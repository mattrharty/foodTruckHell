using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            ing.plate(ingreds.Count + 100);
            return true;
        }
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
            foreach(SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().size = new Vector2 (0.90f, 0.66f + getHeight());
        } else
        {
            foreach(SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        gameObject.GetComponent<BoxCollider>().size = new Vector3(0.7f, 1.0f + getHeight() * 2, 0.2f);
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
                h += 0.16f;
            else if(ing.getName() == IngredType.cooked_patty)
                h += 0.15f;
            else
                h += 0.1f;
        }
        return h;  
    }

    public int getFoodVal()
    {
        int val = 5;
        foreach(Ingred ing in ingreds)
        {
            if(ing.getName() == IngredType.cooked_patty)
                val += 30;
            else
                val += 5;
        }
        return val;
    }

    public IEnumerator sendOut(Transform target)
    {
        float yOffset = 1.67f;
        transform.parent = transform.root.root;
        
        float totalDistance = Mathf.Abs(transform.position.z - target.position.z);
        
        while(Mathf.Abs(transform.position.z - target.position.z) > 0.001f)
        {
            float step = 24.0f * Time.deltaTime;
            Vector3 newPos = Vector3.MoveTowards(transform.position, target.position, step);
            float newY = Mathf.Sin(Mathf.Abs(transform.position.z - target.position.z) / totalDistance * Mathf.PI) * 6 + yOffset;
            transform.position = new Vector3 (newPos.x, newY, newPos.z);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}
