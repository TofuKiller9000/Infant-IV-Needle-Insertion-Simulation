using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinEnter : MonoBehaviour
{
    public VeinMove VM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform.tag == "Needle"))
        {
            VM.VeinEntered();
            print("Vein entered");
        }
        //print("Other: " +  other.gameObject.name);

    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.transform.tag == "Needle"))
        {
            VM.VeinLeft();
            print("Vein left");
        }
    }

}
