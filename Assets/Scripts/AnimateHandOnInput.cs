using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class AnimateHandOnInput : MonoBehaviour
{

    public InputActionProperty pinchAnimation;
    public InputActionProperty gripAnimation; 
    public Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimation.action.ReadValue<float>();
        float gripValue = gripAnimation.action.ReadValue<float>();//local value
       /* if (triggerValue == 1)
        {
            print($"Trigger Value: {triggerValue}");
        }*/
        /*if (gripValue == 1 || gripValue == 0)
        {
            print($"Grip Value Value: {triggerValue}");
        }*/

        handAnimator.SetFloat("Trigger", triggerValue);
        handAnimator.SetFloat("Grip", gripValue);


    }
}
