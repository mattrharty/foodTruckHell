using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingred : MonoBehaviour
{

    [SerializeField] private IngredType name;
    public Sprite[] states;
    private float cookTime;
    private bool grabbable;

    public void Start()
    {
        cookTime = 30;
        grabbable = true;
    }

    public Ingred(IngredType _name)
    {
        name = _name;
    }

    public void setName(IngredType _name)
    {
        name = _name;
    }

    public IngredType getName()
    {
        return name;
    }

    public bool canGrab()
    {
        return grabbable;
    }

    public void plate(int index)
    {
        grabbable = false;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = index;
        if(name == IngredType.bun)
            gameObject.GetComponent<SpriteRenderer>().sprite = states[1];
    }

    public void cook(float timePassed)
    {
        cookTime = cookTime - timePassed;
        if(cookTime < 0 && (int)getName() <= 2){
            setName(IngredType.burnt_patty);
            gameObject.GetComponent<SpriteRenderer>().sprite = states[2];
        } else if(cookTime <= 15 && (int)getName() <= 2){
            setName(IngredType.cooked_patty);
            gameObject.GetComponent<SpriteRenderer>().sprite = states[1];
        } else
        {
            setName(IngredType.patty);
            gameObject.GetComponent<SpriteRenderer>().sprite = states[0];
        }
    }

    public void sendOut()
    {
        gameObject.GetComponent<Animator>().SetTrigger("sendOut");
    }
}
