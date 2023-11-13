using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject rightTeleportation;

    public InputActionProperty rightActivate; 


    // Update is called once per frame
    void Update()
    {
        //this very simple script will simply check if the trigger button is greater than 1, meaning the teleportation action is requested
        rightTeleportation.SetActive(rightActivate.action.ReadValue<float>() >= .9f);
    }
}
