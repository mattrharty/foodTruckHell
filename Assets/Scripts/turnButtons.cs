using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class turnButtons : MonoBehaviour
{
    
    [SerializeField] private playerController player;
    [SerializeField] private InputActionReference click;
    private bool overHUD = false;

    // Update is called once per frame
    void Update()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        overHUD = false;
        foreach(RaycastResult r in results)
            if(r.gameObject.name.Equals(gameObject.name))
                overHUD = true;

        if((!click.action.IsPressed() && GameObject.FindGameObjectWithTag("global").GetComponent<GlobalController>().turnMode.Equals("click")) || !overHUD)
            return;
        if(gameObject.name.Equals("Turn Left"))
        {
            player.turnHandler(true);
        }
        else if(gameObject.name.Equals("Turn Right"))
        {
            player.turnHandler(false);
        }
    }
}
