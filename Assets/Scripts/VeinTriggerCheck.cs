using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class VeinTriggerCheck : MonoBehaviour
{
    public bool isNeedleOut = true;
    public TextMeshProUGUI InVeinText;
    public VeinMove _veinMove;
    public GameObject mainvein; 
        // Start is called before the first frame update
    void Start()
    {
        InVeinText.text = "Not In Vein";
        InVeinText.color = Color.red;

    }

    void Update()
    {
        //if (mainvein.transform.position != transform.position)
        //{
        //    transform.position = mainvein.transform.position;
        //}

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Needle" && _veinMove.bulge == true) //&& _veinMove.bulge == true
        {
            InVeinText.text = "In Vein";
            InVeinText.color = Color.green;
            print("Needle " + " has entered");
            isNeedleOut = false;
        }
        else
        {
            print(other.gameObject.name + " has entered");
        }
        ////Debug.Log("triggerEnter: " + other.gameObject.name);
        //if (other.gameObject.transform.tag == "Needle" && _veinMove.bulge == true)
        //{
        //    InVeinText.text = "In Vein";
        //    InVeinText.color = Color.green;
        //    //Debug.Log("Entered Vein trigger");
        //    isNeedleOut = false;
        //}
        //else
        //{
        //    print(other.gameObject.name + " has entered");
        //}
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.transform.tag == "Needle")
        {
            InVeinText.text = "Not In Vein";
            InVeinText.color = Color.red;
            print("Needle " + " has exited");
            isNeedleOut = true;
        }

        else
        {
            print(other.gameObject.name + " has exited");
        }
        ////Debug.Log("triggerExit: " + other.gameObject.name);
        //if (other.gameObject.transform.tag == "Needle")
        //{
        //    InVeinText.text = "Not In Vein";
        //    InVeinText.color = Color.red;
        //    //Debug.Log("Passed Vein trigger");
        //    isNeedleOut = true;
        //}
        //else
        //{
        //    print(other.gameObject.name + " has entered");
        //}
    }
    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.transform.tag == "Needle")
        {
            InVeinText.text = "Not In Vein";
            InVeinText.color = Color.red;
            //Debug.Log("Passed Vein collision");
            isNeedleOut = true;
        }

        else
        {
            print(collision.gameObject.name + " has exited");
        }
        //// Debug.Log("collisionExit: " + collision.gameObject.name);
        // if (collision.gameObject.transform.tag == "Needle")
        // {
        //     InVeinText.text = "Not In Vein";
        //     InVeinText.color = Color.red;
        //     //Debug.Log("Passed Vein collision");
        //     isNeedleOut = true;
        // }
        // else
        // {
        //     print(collision.gameObject.name + " has entered");
        // }

    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.transform.tag == "Needle" && _veinMove.bulge == true)
        {
            InVeinText.text = "In Vein";
            InVeinText.color = Color.green;
            //Debug.Log("Entered Vein collision");
            isNeedleOut = false;
        }
        else
        {
            print(collision.gameObject.name + " has entered");
        }
        ////Debug.Log("collision enter: " + collision.gameObject.name);
        //if (collision.gameObject.transform.tag == "Needle")
        //{
        //    InVeinText.text = "In Vein";
        //    InVeinText.color = Color.green;
        //    //Debug.Log("Entered Vein collision");
        //    isNeedleOut = false;
        //}

        //else
        //{
        //    print(collision.gameObject.name + " has entered");
        //}
    }

}
