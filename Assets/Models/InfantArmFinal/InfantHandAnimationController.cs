using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantHandAnimationController : MonoBehaviour
{

    public bool open;
    public Animator controller;
    public Transform HandPoint; 
    private GameObject cube; 

    private float time;
    private float lerpValue;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float inverse = 1; 

    void Update()
    {
        if(open)
        {
            distance = Vector3.Distance(HandPoint.transform.position, cube.transform.position);
            distance = Mathf.Clamp01(distance * 10);
            inverse = 1 - distance;
            controller.SetFloat("Blend", inverse);
        }
        else
        {
            controller.SetFloat("Blend", 0);
        }

        //if(open)
        //{
        //    lerpValue = Mathf.Lerp(0, 1, time);
        //    controller.SetFloat("Blend", lerpValue);
        //    time += 0.1f * Time.deltaTime;
        //    time = Mathf.Clamp01(time);
        //}
        //else
        //{
        //    lerpValue = Mathf.Lerp(0, 1, time);

        //    controller.SetFloat("Blend", lerpValue);

        //    time -= 0.1f * Time.deltaTime;
        //    time = Mathf.Clamp01(time);
        //}
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Gee") )
        {
            open = true;
            cube = other.gameObject;
            //print("Cube entered");
        }
        //else
        //{
        //    print(other.transform.name);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Gee")
        {
            open = false;
            cube = null;
            distance = 0;
            inverse = 1; 
            //print("Cube Exited");
        }
    }
}
