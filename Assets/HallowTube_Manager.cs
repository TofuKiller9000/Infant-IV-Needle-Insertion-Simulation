using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallowTube_Manager : MonoBehaviour
{
    //public GameObject Tip;
    public int LayerMask; //number of layer mask that we want to only collide against for our raycast
    private float distance;
    private bool bIsTouchable; 


    public void AlignToVein(ContactPoint contact, Transform parent)
    {
        RaycastHit hit;
        Quaternion rotation;
        Vector3 position;

        int layerMask = 1 << LayerMask;


        if (Physics.Raycast(contact.point, -contact.normal, out hit, Mathf.Infinity, layerMask))
        {
            rotation = Quaternion.FromToRotation(-Vector3.up, hit.normal);
            position = hit.point;
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            distance = hit.distance;
            //gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, hit.distance - .1f, gameObject.transform.localScale.z);
            print("Hit: " + hit.collider);
            Debug.DrawRay(contact.point, -contact.normal * hit.distance, Color.green, 100, false);
        }
        else
        {
            rotation = Quaternion.FromToRotation(-Vector3.up, contact.normal);
            position = contact.point;
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            Debug.Log("No Hit");
            Debug.DrawRay(contact.point, -contact.normal * hit.distance, Color.yellow, 100, false);
        }
        //Debug.DrawRay(contact.point, -contact.normal, Color.yellow, 100, true);

    }

    public void DisalignToVein()
    {
        gameObject.transform.position = Vector3.zero;
    }

    public void TurnOnResistance()
    {
        //gameObject.tag = "Touchable";
        InitializeResistance.Trigger = true;
    }

    public void TurnOffResistance()
    {
       // gameObject.tag = "Untagged";
        InitializeResistance.Trigger = false;
    }    

}
