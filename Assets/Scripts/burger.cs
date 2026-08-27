using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burger : MonoBehaviour
{

    private List<Ingred> ings;

    public List<Ingred> getIngs()
    {
        return ings;
    }

    public void setIngs(List<Ingred> _ings)
    {
        ings = _ings;
    }

    public bool addIng(Ingred newIng)
    {
        return false;
    }

}
