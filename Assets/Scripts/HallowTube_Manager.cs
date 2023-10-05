using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallowTube_Manager : MonoBehaviour
{
    //public GameObject Tip;
    public int LayerMask; //number of layer mask that we want to only collide against for our raycast
    private float distance;
    private bool bIsTouchable;
    public HapticPlugin hapticDevice;

    public HapticSurface hapticSurface;

    private void Update()
    {
        //Debug.Log("Touch Depth: " + hapticDevice.touchingDepth);

        if(hapticDevice.touchingDepth > 10)
        {
            hapticSurface.hlPopThrough = 0.233f;
        }
        else
        {
            hapticSurface.hlPopThrough = 0f;
        }
    }


    public void AlignToVein(ContactPoint contact, Transform parent)
    {
        RaycastHit hit;
        Quaternion rotation;
        Vector3 position;

        int layerMask = 1 << LayerMask;


        if (Physics.Raycast(contact.point, -contact.normal, out hit, Mathf.Infinity, layerMask))
        {
            rotation = Quaternion.FromToRotation(-Vector3.up, hit.normal);//parent.rotation.eulerAngles
            position = hit.point;
            gameObject.transform.position = parent.transform.position;
            gameObject.transform.rotation = parent.transform.rotation;
            distance = hit.distance;
            float angle = Vector3.Angle(hit.normal, hit.point);

        }
        else
        {
            rotation = Quaternion.FromToRotation(-Vector3.up, contact.normal);
            position = contact.point;
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
        }

    }

    public void DisalignToVein()
    {
        //gameObject.transform.position = Vector3.zero;
    }

    public void TurnOnResistance(Transform parent)
    {
        gameObject.transform.SetParent(parent, false);
        print("Parented");
        gameObject.transform.localPosition = new Vector3 (0, 0, 0);
        //TurnOffResistance();
        //gameObject.tag = "Touchable";
        //InitializeResistance.Trigger = true;
    }

    public void TurnOffResistance()
    {
        gameObject.transform.SetParent(null, true);
       //gameObject.tag = "Untagged";
        //InitializeResistance.Trigger = false;
    }    

}
