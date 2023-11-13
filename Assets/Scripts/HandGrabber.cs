using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabber : MonoBehaviour
{

    public bool touched = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void contact() { touched = true; }

    public void leavetouch() { touched = false; }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == 6)
    //    {
    //        Debug.Log("Grabbed hand");
    //        touched = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == 6)
    //    {
    //        Debug.Log("Stopped grabbing hand");
    //        touched = false;
    //    }
    //}
}
