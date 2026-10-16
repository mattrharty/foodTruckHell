using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class attached to each bell referencing that bell's counter.
/// </summary>
public class bell : MonoBehaviour
{

    /// <summary>
    /// The counter that the bell is for.
    /// </summary>
    [Tooltip("The counter that the bell is for.")]
    [SerializeField] private CounterSpot counter;

    /// <summary>
    /// Returns the counter that this bell references.
    /// </summary>
    /// <returns>the counter that this bell references.</returns>
    public CounterSpot getCounter()
    {
        return counter;
    }

}
