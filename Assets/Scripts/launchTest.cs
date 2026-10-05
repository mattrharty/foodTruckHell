using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class launchTest : MonoBehaviour
{

    public Transform obj;
    public Transform target;


    public void goCube()
    {
        StartCoroutine(launch());
    }

    public IEnumerator launch()
    {
        Debug.Log("Started launching cube");
        obj.parent = obj.root;
        float yOffset = obj.position.y;
        float totalDistance = Mathf.Abs(obj.position.z - target.position.z);
        
        Debug.Log("Distance between obj and target: " + Mathf.Abs(obj.position.z - target.position.z));
        while(Mathf.Abs(obj.position.z - target.position.z) > 0.001f)
        {
            Debug.Log("Distance between obj and target: " + Mathf.Abs(obj.position.z - target.position.z));
            float step = 1.0f * Time.deltaTime;
            Vector3 newPos = Vector3.MoveTowards(obj.position, target.position, step);
            float newY = Mathf.Sin(Mathf.Abs(obj.position.z - target.position.z) / totalDistance * Mathf.PI) + yOffset;
            obj.position = new Vector3 (newPos.x, newY, newPos.z);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Finished moving obj");
    }
}
