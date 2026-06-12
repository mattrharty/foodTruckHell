using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    [SerializeField] private Transform holdLoc;
    private GameObject heldObj = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast the ray and check if it hits anything
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0)) 
        {
            
            if(hit.collider.gameObject.GetComponent<IngredBin>() != null){
                if(heldObj == null)
                    grabObj(hit.collider.gameObject.GetComponent<IngredBin>().getIngred());
            }
            else if(hit.collider.gameObject.name.Equals("trash")){
                if(heldObj == null)
                    return;
                GameObject.DestroyImmediate(heldObj.gameObject);
                heldObj = null;
            }
            else if(hit.collider.gameObject.tag.Equals("grillSpot")){
                //Debug.Log(hit.collider.gameObject.transform.parent.gameObject.GetComponent<Grill>());
                int i = hit.collider.gameObject.transform.GetSiblingIndex();
                if(heldObj == null)
                    return;
                if(hit.collider.gameObject.transform.parent.gameObject.GetComponent<Grill>().fillSlot(heldObj, i))
                    placeObj(hit.collider.gameObject.transform.GetChild(0));
            }
            else if(hit.collider.gameObject.GetComponent<Ingred>() != null)
            {
                Debug.Log(hit.collider.gameObject.GetComponent<Ingred>().canGrab());
                if(!hit.collider.gameObject.GetComponent<Ingred>().canGrab())
                    return;
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.transform.parent.parent.tag.Equals("grillSpot"))
                    hit.collider.transform.parent.parent.parent.gameObject.GetComponent<Grill>().emptySlot(hit.collider.gameObject);
                grabObj(hit.collider.gameObject);
            } else if (hit.collider.gameObject.GetComponent<CounterSpot>() != null)
            {
                if(heldObj == null)
                    return;
                float n = hit.collider.gameObject.GetComponent<CounterSpot>().addFood(heldObj);
                if(n > -0.5f)
                    placeObj(hit.collider.transform.GetChild(0), n);
            }   else if (hit.collider.gameObject.GetComponent<bell>() != null)
            {
                hit.collider.gameObject.GetComponent<bell>().getCounter().ding();
            }
        }
    }

    public bool grabObj(GameObject obj)
    {
        if(heldObj == null)
        {
            heldObj = obj;
            heldObj.transform.parent = holdLoc;
            heldObj.transform.position = holdLoc.position;
            heldObj.transform.localRotation = new Quaternion();
            return true;
        }
        return false;
    }

    public bool placeObj(Transform endLoc)
    {
        if(heldObj == null)
            return false;
        Vector3 temp = heldObj.transform.localPosition;
        heldObj.transform.parent = endLoc;
        heldObj.transform.localPosition = new Vector3(0, temp.y, 0);
        heldObj.transform.localRotation = new Quaternion();
        heldObj = null;
        return true;
    }

    public bool placeObj(Transform endLoc, float offsetY)
    {
        if(heldObj == null)
            return false;
        heldObj.transform.parent = endLoc;
        heldObj.transform.localPosition = new Vector3(0, offsetY, 0);
        heldObj.transform.localRotation = new Quaternion();
        heldObj = null;
        return true;
    }

    public IngredType getObjType()
    {
        if(heldObj == null)
            return 0;
        return heldObj.GetComponent<Ingred>().getName();
    }

}
