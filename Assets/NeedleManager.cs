using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleManager : MonoBehaviour
{

    private bool buttonStatus = false;          //!< Is the button currently pressed?
    public int buttonID = 0;		//!< index of the button assigned to grabbing.  Defaults to the first button
    private HapticPlugin hapticDevice = null;   //!< Reference to the GameObject representing the Haptic Device
    private Animator needle; 


    private void Start()
    {
        if (hapticDevice == null)
        {
            hapticDevice = (HapticPlugin)FindObjectOfType(typeof(HapticPlugin));
        }
        if (hapticDevice == null)
        {
            Debug.LogError("Missing Required Haptic Device");
        }
        needle = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        bool newButtonStatus = hapticDevice.GetComponent<HapticPlugin>().Buttons[buttonID] == 1;
        bool oldButtonStatus = buttonStatus;
        buttonStatus = newButtonStatus;

        if(oldButtonStatus == false && newButtonStatus == true)
        {
            print("Engage Safety Cage");
            needle.SetBool("ButtonPressed", true);
        }
        if(oldButtonStatus == true && newButtonStatus == false)
        {
            
        }
    }
}

