using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class versionTxt : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TMP_Text>().text = Application.productName + " v" + Application.version;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
