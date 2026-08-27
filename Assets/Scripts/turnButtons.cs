using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class turnButtons : MonoBehaviour, IPointerEnterHandler
{
    
    [SerializeField] private playerController player;


    public void OnPointerEnter(PointerEventData obj)
    {
        if(gameObject.name.Equals("Turn Left"))
        {
            player.turnHandler(true);
        }
        else if(gameObject.name.Equals("Turn Right"))
        {
            player.turnHandler(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
