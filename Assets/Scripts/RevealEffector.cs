using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RevealEffector : MonoBehaviour
{

    public Light revealLight;
    public Material rend;


    void Update()
    {
        rend.SetVector("_position", revealLight.transform.position);
        rend.SetVector("_direction", -revealLight.transform.forward);
        rend.SetFloat("_angle", revealLight.range);
    }

}