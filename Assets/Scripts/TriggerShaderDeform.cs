using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShaderDeform : MonoBehaviour
{
    public GameObject ParticleSystem;

    private void OnTriggerEnter(Collider other)
    {


        //print(other.tag);
        if (other.tag == "Particle" || other.tag == "Sphere")
        {
            
            ParticleSystem.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //print(other.name);
        if (other.tag == "Particle" || other.tag == "Sphere")
        {
            ParticleSystem.SetActive(false);
        }
    }
}