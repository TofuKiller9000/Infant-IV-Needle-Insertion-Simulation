using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestScript : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        print(other.name + " has entered the " + transform.name);
    }

}
