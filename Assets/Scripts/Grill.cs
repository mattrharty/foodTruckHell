using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour
{

    private GameObject[] slots;
    [SerializeField] private float cookSpeed;

    // Start is called before the first frame update
    void Start()
    {
        slots = new GameObject[transform.childCount];
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

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject obj in slots)
            if(obj != null)
                obj.GetComponent<Ingred>().cook(cookSpeed * Time.deltaTime);
    }
}
