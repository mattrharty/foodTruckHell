using UnityEngine;

/// <summary>
/// A class attached to the grill object that handles all logic surrounding the grill and cooking burgers.
/// </summary>
public class Grill : MonoBehaviour
{

    /// <summary>
    /// An array of Game Objects that will contain any burgers placed on the grill. Length is equal to the amount of slots on the grill.
    /// </summary>
    private GameObject[] slots;

    /// <summary>
    /// The speed at which the grill cooks burgers.
    /// </summary>
    [Tooltip("The speed at which the grill cooks burgers.")]
    [Range(0.5f, 5.0f)]
    [SerializeField] private float cookSpeed;

    [Space][Header("Sprite Changes for Front Panel")]
    /// <summary>
    /// An array of Sprite Renderers attached to the indicator lights on the grill's front panel.
    /// </summary>
    [Tooltip("An array of Sprite Renderers attached to the indicator lights on the grill's front panel.")]
    [SerializeField] private SpriteRenderer[] indicators;
    /// <summary>
    /// An array of Sprites for the indicator lights.
    /// 0 = off
    /// 1 = on
    /// </summary>
    [Tooltip("An array of Sprites for the indicator lights.\n0 = off\n1 = on")]
    [SerializeField] private Sprite[] indicatorStates;

    [Space]
    /// <summary>
    /// An array of Sprite Renderers attached to the burners.
    /// </summary>
    [Tooltip("An array of Sprite Renderers attached to the burners.")]
    [SerializeField] private SpriteRenderer[] burners;
    /// <summary>
    /// An array of Sprites for the burners.
    /// 0 = off
    /// 1-4 = on
    /// </summary>
    [Tooltip("An array of Sprites for the burners.\n0 = off\n1-4 = on")]
    [SerializeField] private Sprite[] burnerStates;

    /// <summary>
    /// The state of each dial on the grill, each controlling a set of two slots; <c>true</c> if "on", otherwise <c>false</c>.
    /// </summary>
    private bool[] dialState;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[transform.GetChild(0).childCount];
        dialState = new bool[3] {false, false, false};
        foreach(ParticleSystem ps in transform.GetChild(0).GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.EmissionModule emit = ps.emission;
            emit.enabled = false;
        }
    }

    /// <summary>
    /// Tries to fill a slot of given index with a given patty, if the slot is empty; returns whether it was successful.
    /// </summary>
    /// <param name="patty">The patty to be placed on the grill</param>
    /// <param name="slotIndex">The index of the slot to be filled.</param>
    /// <returns><c>false</c> if the slot is already full or <c>patty</c> is not a patty, otherwise <c>false</c>.</returns>
    public bool fillSlot(GameObject patty, int slotIndex)
    {
        if(slots[slotIndex] != null || (int)patty.GetComponent<Ingred>().getName() >= 3)
            return false;
        slots[slotIndex] = patty;
        return true;
    }

    /// <summary>
    /// Empties the slot in which a given patty is, if it is on the grill.
    /// </summary>
    /// <param name="patty">The patty to be removed from the grill.</param>
    public void emptySlot(GameObject patty)
    {
        for(int i = 0; i < slots.Length; i++)
            if(patty.Equals(slots[i])){
                slots[i] = null;
            }
        
    }

    /// <summary>
    /// Toggles a given dial and updates the status of the grill accordingly.
    /// </summary>
    /// <param name="dial">The dial to be toggled.</param>
    public void turnDial(GameObject dial)
    {
        int index = int.Parse(dial.name.Substring(5)) - 1;
        if(!dialState[index]){
            dial.transform.eulerAngles = new Vector3 (-5.6f, 0.0f, -90.0f);
            indicators[index].sprite = indicatorStates[1];
            dialState[index] = true;
        }
        else
        {
            dial.transform.eulerAngles = new Vector3 (-5.6f, 0.0f, 0.0f);
            indicators[index].sprite = indicatorStates[0];
            dialState[index] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < slots.Length; i++)
            if(slots[i] != null && dialState[i / 2]){
                slots[i].GetComponent<Ingred>().cook(cookSpeed * Time.deltaTime);
                ParticleSystem.EmissionModule emit = transform.GetChild(0).GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
                emit.enabled = true;
            } else{
                ParticleSystem.EmissionModule emit = transform.GetChild(0).GetChild(i).GetComponentInChildren<ParticleSystem>().emission;
                emit.enabled = false;
            }

        for(int i = 0; i < burners.Length; i++)
                if(dialState[i / 3])
                    burners[i].sprite = burnerStates[Random.Range(1, burnerStates.Length)];
                else
                    burners[i].sprite = burnerStates[0];
    }
}
