using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class depicting a burger containing a list of ingredients.
/// </summary>
public class Burger : MonoBehaviour
{

    /// <summary>
    /// A list of ingredients in the burger.
    /// </summary>
    private List<Ingred> ingreds;
    /// <summary>
    /// A boolean that stores whether or not the ingredients of the burger can be edited.
    /// </summary>
    private bool canEdit;

    /// <summary>
    /// A contructor for creating a burger with no ingredients.
    /// </summary>
    public Burger()
    {
        ingreds = new List<Ingred>();
        canEdit = true;
    }

    /// <summary>
    /// Returns the list of the ingredients.
    /// </summary>
    /// <returns>The list of the ingredients.</returns>
    public List<Ingred> getIngreds()
    {
        return ingreds;
    }

    /// <summary>
    /// Adds a given ingredient to be burger if it is valid.
    /// </summary>
    /// <param name="obj">The ingredient to be added.</param>
    /// <returns>True if the given ingredient is valid, otherwise false.</returns>
    public bool addIngred(GameObject obj)
    {
        if (obj.GetComponent<Ingred>() == null)
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

    /// <summary>
    /// Checks to see if the given ingredient type is valid.
    /// </summary>
    /// <param name="type">The type of ingredient to be checked.</param>
    /// <returns>True if the given ingredient type is valid, otherwise false.</returns>
    private bool isValid(IngredType type)
    {
        List<IngredType> blacklist = new List<IngredType> { IngredType.burnt_patty, IngredType.patty };
        List<IngredType> whitelist = new List<IngredType>();
        if (hasIngred(IngredType.bun))
            blacklist.Add(IngredType.bun);
        else
            whitelist.Add(IngredType.bun);
        if (whitelist.Count > 0)
            return whitelist.Contains(type);
        return !blacklist.Contains(type);
    }

    /// <summary>
    /// Runs every frame to update its appearance based on the status of the burger.
    /// </summary>
    public void Update()
    {
        if (!canEdit)
        {
            foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<SpriteRenderer>().size = new Vector2(0.90f, 0.66f + getHeight());
        }
        else
        {
            foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
                sr.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        gameObject.GetComponent<BoxCollider>().size = new Vector3(0.7f, 1.0f + getHeight() * 2, 0.2f);
    }

    /// <summary>
    /// Checks if the burger has a certain ingredient type.
    /// </summary>
    /// <param name="type">The ingredient type to be checked.</param>
    /// <returns>True if the burger contains the given ingredient, otherwise false.</returns>
    public bool hasIngred(IngredType type)
    {
        foreach (Ingred i in ingreds)
            if (i.getName().Equals(type))
                return true;
        return false;
    }

    /// <summary>
    /// Returns the status of the burger.
    /// </summary>
    /// <returns>True if the burger can be edited, otherwise false.</returns>
    public bool getStatus()
    {
        return canEdit;
    }

    /// <summary>
    /// Sets the status to the given boolean.
    /// </summary>
    /// <param name="status">A boolean that determines if the burger can be edited.</param>
    public void setStatus(bool status)
    {
        canEdit = status;
    }

    /// <summary>
    /// Calculates and returns the height of the burger as used for collision and foil rendering.
    /// </summary>
    /// <returns>The calculated height of the burger.</returns>
    public float getHeight()
    {
        float h = 0.0f;
        if (ingreds.Count <= 1)
            return 0.0f;
        for (int i = 0; i < ingreds.Count - 1; i++)
        {
            Ingred ing = ingreds[i];
            if (ing.getName() == IngredType.bun)
                h += 0.16f;
            else if (ing.getName() == IngredType.cooked_patty)
                h += 0.15f;
            else
                h += 0.1f;
        }
        return h;
    }

    /// <summary>
    /// Calculates and returns the food value of the burger based on the ingredients.
    /// </summary>
    /// <returns>The food value of the burger.</returns>
    public int getFoodVal()
    {
        int val = 5;
        foreach (Ingred ing in ingreds)
        {
            if (ing.getName() == IngredType.cooked_patty)
                val += 30;
            else
                val += 5;
        }
        return val;
    }

}
