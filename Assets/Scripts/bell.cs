using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bell : MonoBehaviour
{

    [SerializeField] private CounterSpot counter;

    public CounterSpot getCounter()
    {
        return counter;
    }

}
