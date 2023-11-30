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
        print("Other: " +  other.gameObject.name);
        VM.VeinEntered();
        print("Vein entered");
    }

    private void OnTriggerExit(Collider other)
    {
        print("Other: " + other.gameObject.name);
        VM.VeinLeft();
        print("Vein left");
    }

}
