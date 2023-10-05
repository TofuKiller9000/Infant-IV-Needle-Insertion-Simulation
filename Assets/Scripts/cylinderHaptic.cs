using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinderHaptic : MonoBehaviour
{
    public float skinStiffness = 0.91f;
    public float skinDamping = 0.9f;
    public float skinStaticFriction = 0.88f;
    public float skinDynamicFriction = 0.9f;

    public float veinStiffness = 0.91f;
    public float veinDamping = 0.9f;
    public float veinStaticFriction = 0.88f;
    public float veinDynamicFriction = 0.9f;



    // Start is called before the first frame update
    void Start()
    {
        setSkinHapticParameter();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setSkinHapticParameter()
    {
        gameObject.GetComponent<HapticSurface>().hlStiffness = skinStiffness;
        gameObject.GetComponent<HapticSurface>().hlDamping = skinDamping;
        gameObject.GetComponent<HapticSurface>().hlStaticFriction = skinStaticFriction;
        gameObject.GetComponent<HapticSurface>().hlDynamicFriction = skinDynamicFriction;
    }

    public void setVeinHapticParameter()
    {
        gameObject.GetComponent<HapticSurface>().hlStiffness = veinStiffness;
        gameObject.GetComponent<HapticSurface>().hlDamping = veinDamping;
        gameObject.GetComponent<HapticSurface>().hlStaticFriction = veinStaticFriction;
        gameObject.GetComponent<HapticSurface>().hlDynamicFriction = veinDynamicFriction;
    }


}
