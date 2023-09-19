using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallowTube_Manager : MonoBehaviour
{
    public GameObject Tip;
    public GameObject TestCube;
    public GameObject Stylus; 
    public int LayerMask; //number of layer mask that we want to only collide against for our raycast
    private float distance;



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
            distance = Vector3.Distance(hit.normal, hit.point);
           // Debug.Log("Distance: " + distance);
           // Debug.Log("Hit distance: " + hit.distance); Hit Distance is more consistent
            //Debug.Log("Hit");
            Debug.DrawRay(contact.point, -contact.normal * hit.distance, Color.green, 100, false);
        }
        else
        {
            Debug.Log("No Hit");
            Debug.DrawRay(contact.point, -contact.normal * hit.distance, Color.yellow, 100, false);
        }
        //rotation = Quaternion.FromToRotation(-Vector3.up, contact.normal);
        Debug.DrawRay(contact.point, -contact.normal, Color.yellow, 100, true);
        // Vector3 position = contact.point;
        //gameObject.transform.parent = null;
        //gameObject.transform.SetParent(parent, true);
        //gameObject.transform.position = position;
        //gameObject.transform.rotation = rotation;

        //gameObject.transform.position = position;
        //gameObject.transform.rotation = rotation;
        //GameObject newOb = Instantiate(TestCube, position, rotation);
        //print("Rotation:" + rotation + "  position: " + position);

        //newOb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }

}
