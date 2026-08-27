using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grill : MonoBehaviour
{

    private GameObject[] slots;
    [SerializeField] private float cookSpeed;

    [SerializeField] private SpriteRenderer[] indicators;
    [SerializeField] private Sprite[] indicatorStates;
    [SerializeField] private bool[] dialState;
    [SerializeField] private SpriteRenderer[] burners;
    [SerializeField] private Sprite[] burnerStates;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[transform.GetChild(0).childCount];
        dialState = new bool[3] {false, false, false};
    }

    public bool fillSlot(GameObject patty, int slotIndex)
    {
        if(slots[slotIndex] != null || (int)patty.GetComponent<Ingred>().getName() >= 3)
            return false;
        slots[slotIndex] = patty;
        return true;
    }

    public void emptySlot(GameObject patty)
    {
        for(int i = 0; i < slots.Length; i++)
            if(slots[i] != null && slots[i].Equals(patty))
                slots[i] = null;
    }

    public void turnDial(GameObject dial)
    {
        int index = int.Parse(dial.name.Substring(5)) - 1;
        Debug.Log(index);
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
            if(slots[i] != null && dialState[i / 2])
                slots[i].GetComponent<Ingred>().cook(cookSpeed * Time.deltaTime);

        for(int i = 0; i < burners.Length; i++)
                if(dialState[i / 3])
                    burners[i].sprite = burnerStates[Random.Range(1, burnerStates.Length)];
                else
                    burners[i].sprite = burnerStates[0];
    }
}
