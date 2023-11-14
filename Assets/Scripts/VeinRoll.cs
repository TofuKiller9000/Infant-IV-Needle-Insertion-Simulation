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
    public TextMeshProUGUI _debugginText;
    public TextMeshProUGUI _leftVein, _rightVein; 
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
        if(_veinMove == null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        _debugginText.text = "Timer: " + timeLeft.ToString();
        //_leftVein.text = Mathf.Floor(Arm.GetBlendShapeWeight(2)).ToString();
        //_rightVein.text = Mathf.Floor(Arm.GetBlendShapeWeight(1)).ToString();

        //if(temp ==maxBlendShapeVal)
        //{
        //    _debugginText.color = Color.green;
        //}
        //else
        //{
        //    _debugginText.color = Color.red;
        //}
        //Vein Puff
        if (contact)
        {
            if (Arm.GetBlendShapeWeight(0) <= maxBlendShapeVal)
            {
                Arm.SetBlendShapeWeight(0,Mathf.Clamp(Arm.GetBlendShapeWeight(0) + (rollrate * 2) * Time.deltaTime, minBlendShapeVal, maxBlendShapeVal));
            }
            if(Mathf.Floor(Arm.GetBlendShapeWeight(0)) == maxBlendShapeVal)
            {
                
                //print("At max Blendshape value: " + Mathf.Floor(Arm.GetBlendShapeWeight(0)));
               // _veinMove.BulgeActivate();
            }
            else
            {
                //print(Arm.GetBlendShapeWeight(0));
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
                }
                if (Mathf.Floor(Arm.GetBlendShapeWeight(0)) == minBlendShapeVal)
                {
                    _debugginText.text = "At Min Blendshape";
                    //_veinMove.BulgeDectivate();
                }
            }
            //if (Time.time > resettime + resettimer)
            //{
            //    if (Arm.GetBlendShapeWeight(0) > minBlendShapeVal)
            //    {
            //        Arm.SetBlendShapeWeight(0, Arm.GetBlendShapeWeight(0) - (rollrate * 2) * Time.deltaTime);
            //    }
            //   if (Mathf.Floor(Arm.GetBlendShapeWeight(0)) == minBlendShapeVal)
            //    {
            //        _debugginText.text = "At Min Blendshape";
            //        //_veinMove.BulgeDectivate();
            //    }
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.layer + " " + other.gameObject.name + contact);
        if (other.gameObject.layer == 22 || other.gameObject.name.Contains("Left"))
        {
            contact = true;
            ResetTimer();
            print("Collision Entered for Vein Roll");
        }
        else
        {
            //print(other.gameObject.layer + " " + contact);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == gloveMask || other.gameObject.name.Contains("Left"))
        {
            contact = false;
            ResetTimer();
        }
        else
        {
            //print(other.gameObject.name);
        }
    }
    public void ResetTimer()
    {
        timeLeft = timeTillReset;
    }
}
