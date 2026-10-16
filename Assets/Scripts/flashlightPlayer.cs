using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor;
using System.Reflection;

/// <summary>
/// A class with one instance in the scene which controls the flashlight mechanic.
/// </summary>
public class flashlightPlayer : MonoBehaviour
{

    /// <summary>
    /// The total, or maximum, amount of battery power.
    /// </summary>
    [Tooltip("The total, or maximum, amount of battery power.")]
    [Range(20, 150)]
    [SerializeField] private float totalBattery = 100f;

    [Space]
    /// <summary>
    /// A reference to the InputAction for using the flashlight; By default, right-click.
    /// </summary>
    [Tooltip("A reference to the InputAction for using the flashlight; By default, right-click.")]
    [SerializeField] private InputActionReference input;

    [Space]
    /// <summary>
    /// A reference to the playerController instance in the scene.
    /// </summary>
    [Tooltip("A reference to the playerController instance in the scene.")]
    [SerializeField] private playerController player;

    [Space][Header("Sprite Arrays for Flashlight UI")]
    /// <summary>
    /// A Sprite Array containing sprites of the battery UI for each increment of battery life remaining in increasing order.
    /// </summary>
    [Tooltip("A Sprite Array containing sprites of the battery UI for each increment of battery life remaining in increasing order.")]
    [SerializeField] private Sprite[] batStates;

    /// <summary>
    /// A Sprite Array containing the sprites of the outer flashlight UI depicting its status.
    /// 0 = off
    /// 1 = on
    /// 2 = broken
    /// </summary>
    [Tooltip("A Sprite Array containing the sprites of the outer flashlight UI depicting its status.\n0 = off\n1 = on\n2 = broken")]
    [SerializeField] private Sprite[] lightStates;

    /// <summary>
    /// A Sprite Array containing the sprite of the battery slot UI.
    /// 0 = normal
    /// 1 = red-tint
    /// </summary>
    [Tooltip("A Sprite Array containing the sprite of the battery slot UI.\n 0 = normal\n 1 = red-tint")]
    [SerializeField] private Sprite[] slotStates;

    [Space][Header("References to Flashlight UI")]
    /// <summary>
    /// A reference to the Image component depicting the battery life UI.
    /// </summary>\
    [Tooltip("A reference to the Image component depicting the battery life UI.")]
    [SerializeField] private Image batImg;

    /// <summary>
    /// A reference to the Image component depicting the outer flashlight shell UI.
    /// </summary>
    [Tooltip("A reference to the Image component depicting the outer flashlight shell UI.")]
    [SerializeField] private Image lightImg;

    /// <summary>
    /// A reference to the Image component depicting the battery slot UI.
    /// </summary>
    [Tooltip("A reference to the Image component depicting the battery slot UI.")]
    [SerializeField] private Image slotImg;

    [Space][Header("Reference to Actual Flashlight Effect Elements")]
    /// <summary>
    /// The Transform component of the flashlight effect that masks the darkness.
    /// </summary>
    [Tooltip("Transform component of the flashlight effect that masks the darkness.")]
    [SerializeField] private Transform flashlightEffect;

    /// <summary>
    /// The Animator component of the flashlight effect, containing flicker on and off animations.
    /// </summary>
    [Tooltip("The Animator component of the flashlight effect, containing flicker on and off animations.")]
    [SerializeField] private Animator flashlightAnim;

    /// <summary>
    /// The Transform component of the gameObject containing a Light component to be pointed at the flashlight effect.
    /// </summary>
    [Tooltip("The Transform component of the gameObject containing a Light component to be pointed at the flashlight effect.")]
    [SerializeField] private Transform spotLight;

    /// <summary>
    /// The Transform component of the flashlight effect Image overlaying the screen.
    /// </summary>
    [Tooltip("The Transform component of the flashlight effect Image overlaying the screen.")]
    [SerializeField] private Transform canvasLight;

    /// <summary>
    /// The remaining battery life out of 100, starts at 99.99f.
    /// </summary>
    private float batteryLife = 99.99f;

    /// <summary>
    /// A boolean to track whether the flashlight is broken.
    /// </summary>
    private bool broke = false;

    /// <summary>
    /// Increment variable to keep track of when the battery slot should be blinking.
    /// </summary>
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
        canvasLight.position = Input.mousePosition;

        if(player.getLoc().Equals("window")){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layer = LayerMask.GetMask("darkness");
            // Cast the ray and check if it hits anything
            if (Physics.Raycast(ray, out hit, 500, layer))
            {
                flashlightEffect.position = hit.point - new Vector3 (0.0f, 0.6f, 0.0f);
                flashlightEffect.parent.eulerAngles = new Vector3 (0, Mathf.Clamp(flashlightEffect.localPosition.x / 0.25f * 30, -20.0f, 20.0f), 0);

                float depth = Mathf.Abs(flashlightEffect.position.z - spotLight.position.z);
                spotLight.eulerAngles = new Vector3 (ToDeg(Mathf.Atan(flashlightEffect.position.y / depth)) * -1.0f + 22.5f, ToDeg(Mathf.Atan(flashlightEffect.position.x / depth)), 0);
            }
        }


        if (input.action.IsPressed())
        {
            lightImg.sprite = lightStates[1];
            flashlightAnim.SetBool("clicking", true);
            batteryLife -= Time.deltaTime;
            spotLight.gameObject.GetComponent<Light>().intensity = 6.0f;

            canvasLight.gameObject.SetActive(true);
        }
        else
        {
            lightImg.sprite = lightStates[0];
            flashlightAnim.SetBool("clicking", false);
            spotLight.gameObject.GetComponent<Light>().intensity = 0.0f;

            canvasLight.gameObject.SetActive(false);
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

    /// <summary>
    /// Sets <c>batteryLife</c> to 99.99.
    /// </summary>
    public void RefillBattery()
    {
        batteryLife = 99.99f;
    }

    /// <summary>
    /// Takes a value in radians and returns its conversion to the equivalent in degrees.
    /// </summary>
    /// <param name="rad">A value in radians.</param>
    /// <returns><c>rad</c> converted to degrees.</returns>
    private float ToDeg(float rad)
    {
        return rad * (180 / Mathf.PI);
    }

    public void BlinkLight()
    {
        blinking = 4;
    }
}
