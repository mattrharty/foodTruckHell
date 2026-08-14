using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;

public class flashlightPlayer : MonoBehaviour
{

    [SerializeField] private float totalBattery = 100f;

    [SerializeField] public InputActionReference input;

    [SerializeField] private Sprite[] batStates;
    [SerializeField] private Sprite[] lightStates;
    [SerializeField] private Sprite[] slotStates;

    [SerializeField] private Image batImg;
    [SerializeField] private Image lightImg;
    [SerializeField] private Image slotImg;

    [SerializeField] private Transform flashlightEffect;
    [SerializeField] private Animator flashlightAnim;

    private float batteryLife = 99.99f;
    private bool broke = false;
    private int blinking = 0;

    // Start is called before the first frame update
    void Start()
    {
        flashlightAnim.SetBool("clicking", false);
        input.asset.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.action.IsPressed())
        {
            lightImg.sprite = lightStates[1];
            flashlightAnim.SetBool("clicking", true);
            batteryLife -= Time.deltaTime * (batteryLife / 100f);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layer = LayerMask.GetMask("darkness");
            // Cast the ray and check if it hits anything
            if (Physics.Raycast(ray, out hit, 500, layer))
            {
                Debug.Log(hit.collider);
                flashlightEffect.transform.position = hit.point - new Vector3 (0.0f, 0.6f, 0.0f);
                flashlightEffect.transform.parent.eulerAngles = new Vector3 (0, flashlightEffect.transform.localPosition.x / 0.25f * 20, 0);
            }
        }
        else
        {
            lightImg.sprite = lightStates[0];
            flashlightAnim.SetBool("clicking", false);
        }

        

        batImg.sprite = batStates[Mathf.FloorToInt(batteryLife / 100f * batStates.Length)];
        
        if(blinking > 0)
        {
            if(slotImg.sprite.Equals(slotStates[0]))
                slotImg.sprite = slotStates[1];
            else
                slotImg.sprite = slotStates[0];
            blinking--;
        }
    }

    public void refillBattery()
    {
        batteryLife = 99.99f;
    }

    public void blinkLight()
    {
        blinking = 4;
    }
}
