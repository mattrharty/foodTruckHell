using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class clickableObj : MonoBehaviour
{

    [SerializeField] private UnityEvent onClick;

    // Start is called before the first frame update
    public void run()
    {
        onClick.Invoke();
    }
}
