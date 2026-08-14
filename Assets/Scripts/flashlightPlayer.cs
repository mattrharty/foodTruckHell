using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class flashlightPlayer : MonoBehaviour
{

    [SerializeField] private Sprite[] batStates;
    [SerializeField] private Sprite[] lightStates;
    [SerializeField] private Sprite[] slotStates;

    [SerializeField] private Image batImg;
    [SerializeField] private Image lightImg;
    [SerializeField] private Image slotImg;

    private float batteryLife = 99.99f;
    private bool broke = false;
    private int blinkerTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        batImg.sprite = batStates[Mathf.FloorToInt(batteryLife / 100f * batStates.Length)];
        
        if(blinkerTimer != 0)
        {
            if(blinkerTimer % 2 == 0)
                slotImg.sprite = slotStates[0];
            else
                slotImg.sprite = slotStates[1];
        } else
            slotImg.sprite = slotStates[0];
    }

    public void refillBattery()
    {
        batteryLife = 99.99f;
    }

    public void blinkLight()
    {
        blinkerTimer = 10;
    }
}
