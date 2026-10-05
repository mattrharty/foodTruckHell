using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{

    [SerializeField] private Transform holdLoc;
    private GameObject heldObj = null;
    private bool canInteract = false;

    [SerializeField] private InputActionReference left;
    [SerializeField] private InputActionReference right;
    [SerializeField] private InputActionReference flashlight;
    [SerializeField] private InputActionReference pause;

    [SerializeField] private GameObject[] turnHUD;
    [SerializeField] private PauseMenu pauseMenu;

    private string pos;

    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        pos = "window";

        anim.Play("backWindow", 0, 1.0f);

        pause.action.Enable();
        pause.action.performed += pauseGame;
    }

    public void OnDisable()
    {
        pause.action.performed -= pauseGame;
        pause.action.Disable();
    }

    public void pauseGame(InputAction.CallbackContext context)
    {
        canInteract = !pauseMenu.pauseGame();
    }

    public void pauseGame()
    {
        canInteract = !pauseMenu.pauseGame();
    }

    public IEnumerator turnLeft()
    {
        canInteract = false;
        if(pos.Equals("window"))
            pos = "back";
        else if(pos.Equals("back"))
            pos = "grill";
        else if(pos.Equals("grill"))
            pos = "window";
        anim.SetTrigger("left");

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        canInteract = true;
    }

    public IEnumerator turnRight()
    {
        canInteract = false;
        if(pos.Equals("window"))
            pos = "grill";
        else if(pos.Equals("back"))
            pos = "window";
        else if(pos.Equals("grill"))
            pos = "back";
        anim.SetTrigger("right");
        
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        canInteract = true;
    }

    public void turnHandler(bool left)
    {
        if(!canInteract)
            return;

        if (left)
            StartCoroutine(turnLeft());
        else
            StartCoroutine(turnRight());
    }

    // Update is called once per frame
    void Update()
    {

        //Fixes position
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Window"))
            pos = "window";
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Grill"))
            pos = "grill";
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Back"))
            pos = "back";

        // Checks canInteract
        if (!canInteract)
        {
            left.action.Disable();
            right.action.Disable();
            flashlight.action.Disable();
            foreach(GameObject obj in turnHUD)
                obj.SetActive(false);
            return;
        } else
        {
            left.action.Enable();
            right.action.Enable();
            flashlight.action.Enable();
            foreach(GameObject obj in turnHUD)
                obj.SetActive(true);
        }

        // Checks for left and right input
        if (left.action.IsPressed())
        {
            StartCoroutine(turnLeft());
        }
        if (right.action.IsPressed())
        {
            StartCoroutine(turnRight());
        }

        

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
            else if(hit.collider.gameObject.GetComponent<clickableObj>() != null)
            {
                hit.collider.gameObject.GetComponent<clickableObj>().run();
            }
            else if(hit.collider.gameObject.name.Equals("trash")){
                if(heldObj == null)
                    return;
                GameObject.DestroyImmediate(heldObj.gameObject);
                heldObj = null;
            }
            else if(hit.collider.gameObject.tag.Equals("grillSpot")){
                int i = hit.collider.gameObject.transform.GetSiblingIndex();
                if(heldObj == null)
                    return;
                if(hit.collider.gameObject.transform.parent.parent.gameObject.GetComponent<Grill>().fillSlot(heldObj, i))
                    placeObj(hit.collider.gameObject.transform.GetChild(0));
            }
            else if(hit.collider.gameObject.GetComponent<Ingred>() != null)
            {
                if(!hit.collider.gameObject.GetComponent<Ingred>().canGrab())
                    return;
                if(hit.collider.transform.parent.parent.tag.Equals("grillSpot"))
                    hit.collider.transform.parent.parent.parent.parent.gameObject.GetComponent<Grill>().emptySlot(hit.collider.gameObject);
                grabObj(hit.collider.gameObject);
            } else if (hit.collider.gameObject.GetComponent<CounterSpot>() != null)
            {
                if(heldObj == null)
                    return;
                if(heldObj.GetComponent<Burger>() != null && hit.collider.transform.GetChild(0).childCount == 0){
                    Transform daddy = hit.collider.transform.GetChild(0);
                    daddy.localPosition = new Vector3 (daddy.localPosition.x, heldObj.GetComponent<SpriteRenderer>().size.y / 4, daddy.localPosition.z);
                    hit.collider.gameObject.GetComponent<CounterSpot>().setBurger(heldObj.GetComponent<Burger>());
                    placeObj(hit.collider.transform.GetChild(0));
                }
                else if(heldObj.GetComponent<Fries>() != null && hit.collider.transform.GetChild(1).childCount == 0){
                    hit.collider.gameObject.GetComponent<CounterSpot>().setFries(heldObj.GetComponent<Fries>());
                    placeObj(hit.collider.transform.GetChild(1));
                }
                else if(heldObj.GetComponent<Soda>() != null && hit.collider.transform.GetChild(2).childCount == 0){
                    hit.collider.gameObject.GetComponent<CounterSpot>().setSoda(heldObj.GetComponent<Soda>());
                    placeObj(hit.collider.transform.GetChild(2));
                }
            } else if(hit.collider.gameObject.GetComponent<Burger>() != null)
            {
                Burger bur = hit.collider.gameObject.GetComponent<Burger>();
                if(heldObj != null && heldObj.GetComponent<Burger>() != null && bur.getStatus() && !bur.hasIngred(IngredType.bun))
                {
                    heldObj.GetComponent<Burger>().setStatus(true);
                    placeObj(bur.transform);
                    bur.transform.GetChild(0).parent = bur.transform.parent;

                    Destroy(bur.gameObject);
                    return;
                }
                if(heldObj != null && bur.getStatus()){
                    if(bur.addIngred(heldObj))
                        placeObj(bur.transform, bur.getHeight());
                    return;
                }
                if(heldObj == null && bur.hasIngred(IngredType.bun)){
                    GameObject newBur = new GameObject();
                    newBur.AddComponent<Burger>();
                    newBur.AddComponent<BoxCollider>();
                    newBur.GetComponent<BoxCollider>().size = new Vector3 (0.5f, 0.1f, 0.5f);
                    newBur.AddComponent<SpriteRenderer>();

                    newBur.GetComponent<SpriteRenderer>().sprite = bur.GetComponent<SpriteRenderer>().sprite;
                    newBur.GetComponent<SpriteRenderer>().drawMode = bur.GetComponent<SpriteRenderer>().drawMode;
                    newBur.GetComponent<SpriteRenderer>().sortingLayerName = "Foil";

                    newBur.name = "Burger";
                    newBur.transform.parent = bur.transform.parent;
                    newBur.transform.localEulerAngles = new Vector3();
                    newBur.transform.localPosition = new Vector3();
                    newBur.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    grabObj(bur.gameObject);
                    bur.setStatus(false);
                }
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

    public string getLoc()
    {
        return pos;
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
