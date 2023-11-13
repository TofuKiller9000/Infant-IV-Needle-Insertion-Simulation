using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SGCore;
using SG;
using UnityEngine.InputSystem.XR.Haptics;
using SGCore.Haptics;

public class Heartbeat : MonoBehaviour
{

    SG_TrackedHand hand;
    ThumperWaveForm waveform;
    SG_HandDetector handDetector;

    public int pulse = 25;

    public float pulsetime = .7f;
    public float pulsetimer = 1000000000;

    bool index = false;
    bool thumb = false;
    
    SG_BuzzCmd beat = null;

    private int[] fingers = new int[5] { 0, 0, 0, 0, 0 };

    //public Material correctMat;
    //public GameObject Light;

    //bool on = false;
    //private float mattime = 1;
    //private float mattimer = 10000000;
    //public Material basemat;

    // Start is called before the first frame update
    void Start()
    {
        handDetector = GetComponentInChildren<SG_HandDetector>();
        
    }

    // Update is called once per frame
    void Update()
    {

        try{ hand = handDetector.FullyDetectedHands()[0]; }
        catch { }
        if (index)
        {
            if(Time.time > pulsetimer + pulsetime)
            {
                pulsetimer = Time.time;
                SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Index, pulse, .2f, 0);
                hand.SendCmd(tmp);
            }
        }
        if (thumb)
        {
            if (Time.time > pulsetimer + pulsetime)
            {
                pulsetimer = Time.time;
                SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Thumb, pulse, .2f, 0);
                hand.SendCmd(tmp);
            }
        }


        //if (on)
        //{
        //    if (Time.time > mattimer + mattime)
        //    {
        //        Light.GetComponent<Renderer>().material = basemat;
        //        on = false;
        //    }
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Collision entered");
        if(other.tag == "Index")
        {
            index = true;
            pulsetimer = 0;
            fingers[1] = pulse;
            beat = new SG_BuzzCmd(fingers);
            SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Index, pulse, .2f, 0);
            try { hand.SendCmd(tmp); }
            catch { }
        }
        if (other.tag == "thumb")
        {
            thumb = true;
            pulsetimer = 0;
            fingers[0] = pulse;
            beat = new SG_BuzzCmd(fingers);
            SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Thumb, pulse, .2f, 0);
            try { hand.SendCmd(tmp); }
            catch { }
        }
        if (other.tag == "Poker")
        {
            //on = true; mattimer = Time.time;
            //Light.GetComponent<Renderer>().material = correctMat;
            float x = Random.Range(-0.14f, -0.033f);
            float z = Random.Range(0.08f, 0.358f);
            Vector3 pos = new Vector3(x, -0.1058f, z);
            float rot = Random.Range(-15f, 15f);
            Debug.Log("Rotation: " + rot);
            transform.localPosition = pos;
            transform.eulerAngles = new Vector3(0, rot, 0);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collision exit");
        if(other.tag == "Index")
        {
            index = false;
            fingers[1] = 0;
            beat = new SG_BuzzCmd(fingers);
            SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Index, pulse, .2f, 0);
            //try { hand.SendCmd(tmp); }
           // catch { }

        }
        if(other.tag == "thumb")
        {
            thumb = false;
            fingers[0] = 0;
            beat = new SG_BuzzCmd(fingers);
            SG_TimedBuzzCmd tmp = new SG_TimedBuzzCmd(Finger.Thumb, pulse, .2f, 0);
        }
    }
}
