using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeSpot : MonoBehaviour
{

    [SerializeField] private spotType type;

    private bool hasSoda = false;
    private bool hasFries = false;
    private burger bur;

    public bool addFood(GameObject obj)
    {
        Ingred ing = null;
        if(obj.GetComponent<Ingred>() != null)
            ing = obj.GetComponent<Ingred>();
        else
            return false;
        IngredType name = ing.getName();
        List<IngredType> blacklist = new List<IngredType> {IngredType.patty, IngredType.burnt_patty, IngredType.unfried_fries, IngredType.burnt_fries};
        if(type == spotType.assembly)
            blacklist.AddRange(new List<IngredType> {IngredType.soda, IngredType.fries});
        return false;
    }

}

public enum spotType
{
    assembly,
    counter
}