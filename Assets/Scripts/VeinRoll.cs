using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class VeinRoll : MonoBehaviour
{

    [Header("Vein Roll Settings")]
    public SkinnedMeshRenderer Arm;
    public HandGrabber HG;
    public VeinMove _veinMove;
    public TextMeshProUGUI veinBulging; 
    public float maxBlendShapeVal, minBlendShapeVal;
    public float rollrate;
    public LayerMask gloveMask;
    public float timeTillReset = 5;
    private float timeLeft;


    #region Vein Roll Properties
    private float rollval;
    private float distance;
    private float x;
    private float y;
    private float xcomp;
    private float ycomp;
    private float basez;
    private bool contact = false;
    private bool needle = true;
    private bool roll;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeTillReset;
        x = transform.localPosition.x;
        y = transform.localPosition.y;
        xcomp = transform.position.x;
        ycomp = transform.position.y;
        basez = transform.localPosition.z;
        veinBulging.text = "Vein Bulging : False ";
        veinBulging.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (contact)
        {
            if (Arm.GetBlendShapeWeight(0) <= maxBlendShapeVal)
            {
                Arm.SetBlendShapeWeight(0,Mathf.Clamp(Arm.GetBlendShapeWeight(0) + (rollrate * 2) * Time.deltaTime, minBlendShapeVal, maxBlendShapeVal));
                veinBulging.text = "Vein Bulging : True ";
                veinBulging.color = Color.green;
            }
        }

        if (!contact && HG.touched == false)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                if (Arm.GetBlendShapeWeight(0) > minBlendShapeVal)
                {
                    Arm.SetBlendShapeWeight(0, Arm.GetBlendShapeWeight(0) - (rollrate * 2) * Time.deltaTime);
                    veinBulging.text = "Vein Bulging : False ";
                    veinBulging.color = Color.red;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.gameObject.layer + " " + other.gameObject.name + contact);
        if ((other.gameObject.layer == 22 || other.gameObject.name.Contains("Left")) && other.transform.tag != "Index")
        {
            contact = true;
            ResetTimer();
            print("Collision Entered for Vein Roll");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 22 || other.gameObject.name.Contains("Left") && other.transform.tag != "Index")
        {
            contact = false;
        }
    }
    public void ResetTimer()
    {
        timeLeft = timeTillReset;
    }
}
