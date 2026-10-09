using UnityEngine;

public class ShutterIndicators : MonoBehaviour
{

    [SerializeField]
    private GameObject indicatorDaddy;
    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.useGravity)
        {
            indicatorDaddy.SetActive(false);
        } else
        {
            indicatorDaddy.SetActive(true);
            if(rb.mass > 0.0f)
                anim.SetBool("close", false);
            else
                anim.SetBool("close", true);
        }
    }
}
