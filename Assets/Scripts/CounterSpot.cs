using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A class attached to each counter object managing the food placed upon the counter.
/// </summary>
public class CounterSpot : MonoBehaviour
{

    /// <summary>
    /// The burger placed on the counter.
    /// </summary>
    private Burger burger;
    /// <summary>
    /// The soda placed on the counter.
    /// </summary>
    private Soda soda;
    /// <summary>
    /// The fries placed on the counter.
    /// </summary>
    private Fries fries;

    /// <summary>
    /// Reference to the night controller.
    /// </summary>
    [SerializeField] NightController control;

    /// <summary>
    /// Sends food at zombies and triggers counter <code>"orderUp"</code> animation.
    /// </summary>
    public void ding()
    {
        if (!control.zombiePresent(transform.parent.GetSiblingIndex()))
            return;
        transform.parent.gameObject.GetComponent<Animator>().SetTrigger("orderUp");
        control.orderUp(transform.parent.GetSiblingIndex(), gameObject);
    }

    /// <summary>
    /// Calculates and returns the food value of all food on the counter.
    /// </summary>
    /// <returns>the food value of all food on the counter.</returns>
    public int calculateFoodValue()
    {
        int val = 0;

        if (burger != null)
            val += burger.getFoodVal();
        if (fries != null)
            val += fries.getFoodVal();
        if (soda != null)
            val += soda.getFoodVal();

        return val;
    }

    /// <summary>
    /// Clears all the food from the counter and sends it out at the zombies.
    /// </summary>
    /// <param name="target">The Transform component of the target zombie.</param>
    public void clear(Transform target)
    {
        if (transform.GetChild(0).childCount > 0)
            transform.GetChild(0).GetChild(0).parent = transform.root;
        if (transform.GetChild(1).childCount > 0)
            transform.GetChild(1).GetChild(0).parent = transform.root;
        if (transform.GetChild(2).childCount > 0)
            transform.GetChild(2).GetChild(0).parent = transform.root;

        if (burger != null)
            StartCoroutine(burger.sendOut(target));
        /*if(fries != null)
            StartCoroutine(fries.sendOut(target));
        if(fries != null)
            StartCoroutine(soda.sendOut(target));*/
    }

    /// <summary>
    /// Adds the given burger to the counter if there is not one already.
    /// </summary>
    /// <param name="bur">The burger to be added to the counter.</param>
    /// <returns>False if the counter was previously had a burger, otherwise true.</returns>
    public bool setBurger(Burger bur)
    {
        if (burger != null)
            return false;
        burger = bur;
        return true;
    }

    /// <summary>
    /// Adds the given fries to the counter if there is not one already.
    /// </summary>
    /// <param name="f">The fries to be added to the counter.</param>
    /// <returns>False if the counter was previously had a fries, otherwise true.</returns>
    public bool setFries(Fries f)
    {
        if (fries != null)
            return false;
        fries = f;
        return true;
    }

    /// <summary>
    /// Adds the given soda to the counter if there is not one already.
    /// </summary>
    /// <param name="s">The soda to be added to the counter.</param>
    /// <returns>False if the counter was previously had a soda, otherwise true.</returns>
    public bool setSoda(Soda s)
    {
        if (soda != null)
            return false;
        soda = s;
        return true;
    }

}
