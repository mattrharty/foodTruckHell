using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class referencing the method(s) to be executed upon be clicked.
/// </summary>
public class clickableObj : MonoBehaviour
{

    /// <summary>
    /// The event to be executed when this object is clicked.
    /// </summary>
    [Tooltip("The event to be executed when this object is clicked.")]
    [SerializeField] private UnityEvent onClick;

    /// <summary>
    /// Invokes the onClick event.
    /// </summary>
    public void run()
    {
        onClick.Invoke();
    }
}
