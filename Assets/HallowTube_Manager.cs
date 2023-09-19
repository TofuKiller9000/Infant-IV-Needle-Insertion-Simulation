using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallowTube_Manager : MonoBehaviour
{
    public GameObject Tip;
    public GameObject TestCube;
    public GameObject Stylus; 
    public int LayerMask; //number of layer mask that we want to only collide against for our raycast

    public void AlignToVein(ContactPoint contact, Transform parent)
    {
        RaycastHit hit;
        if (Physics.Raycast(contact.point, -contact.normal, out hit, Mathf.Infinity, LayerMask))
        {
            Debug.Log("Hit");
            Debug.DrawRay(contact.point, -contact.normal * hit.distance, Color.yellow);
        }
        else
        {
            Debug.Log("No Hit");
        }
        Quaternion rotation = Quaternion.FromToRotation(-Vector3.up, contact.normal);
        Debug.DrawRay(contact.point, contact.normal, Color.red, 100, true);
        Vector3 position = contact.point;
        //gameObject.transform.parent = null;
        //gameObject.transform.SetParent(parent, true);
        //gameObject.transform.position = position;
        //gameObject.transform.rotation = rotation;

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        //GameObject newOb = Instantiate(TestCube, position, rotation);
        //print("Rotation:" + rotation + "  position: " + position);

        //newOb.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }

}
