using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagDirection : MonoBehaviour
{

    public HapticEffect CubeEffect; 

    // Update is called once per frame
    void Update()
    {
        CubeEffect.Direction =  transform.up; 
    }
}
