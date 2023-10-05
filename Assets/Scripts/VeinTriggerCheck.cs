using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinTriggerCheck : MonoBehaviour
{
    public bool isNeedleOut = true;
        // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Vein").transform.position != transform.position)
        {
            transform.position = GameObject.Find("Vein").transform.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggerEnter: " + other.gameObject.name);
        if (other.gameObject.name == "needlePoint")
        {
            //Debug.Log("Entered Vein trigger");
            isNeedleOut = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("triggerExit: " + other.gameObject.name);
        if (other.gameObject.name == "needlePoint")
        {
            //Debug.Log("Passed Vein trigger");
            isNeedleOut = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
       // Debug.Log("collisionExit: " + collision.gameObject.name);
        if (collision.gameObject.name == "needlePoint")
        {
            //Debug.Log("Passed Vein collision");
            isNeedleOut = true;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision enter: " + collision.gameObject.name);
        if (collision.gameObject.name == "needlePoint")
        {
            //Debug.Log("Entered Vein collision");
            isNeedleOut = false;
        }
       

    }

}
