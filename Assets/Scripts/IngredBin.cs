using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredBin : MonoBehaviour
{

    [SerializeField] private GameObject ingredPrefab;

    public GameObject getIngred()
    {
        return Instantiate(ingredPrefab);
    }

}
