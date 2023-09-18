using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeResistance : MonoBehaviour
{

    public bool Trigger; 

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Touchable";
        Trigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Trigger)
        {
            gameObject.tag = "Touchable";
        }
        else
        {
            gameObject.tag = "Untagged";
        }

    }
}
