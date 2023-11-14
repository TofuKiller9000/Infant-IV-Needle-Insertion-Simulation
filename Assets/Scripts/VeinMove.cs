using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinMove : MonoBehaviour
{

    [Header("Vein Move Properties")]
    public GameObject vein;
    public SkinnedMeshRenderer Arm;
    public bool touched = false;
    public bool inserted = false;
    private bool bulge = false;
    public bool roll = false;
    [Space]

    public float maxRollVal;
    public float minRollVal;
    public Transform rightVeinRollPosition; 
    public Transform leftVeinRollPosition;
    public Transform defaultVeinRollPosition;
    [Space]
    public float rollval = 0;
    public float rollrate;
    public float timeTillReset = 30f; 
    private float timeLeft;

    private float x;
    private float y;
    private float basez;
    private float distance = .0003f;
    private Vector3 previousPosition; 

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = timeTillReset;
        x = transform.localPosition.x;
        y = transform.localPosition.y;
        basez = transform.localPosition.z;
        previousPosition = defaultVeinRollPosition.position;
        var distance =  Vector3.Distance(leftVeinRollPosition.position, rightVeinRollPosition.position);
        print(distance);
    }

    // Update is called once per frame
    void Update()
    {
        //print("Timer: " + timeLeft);
        //VEIN ROLLING MOVEMENT TO BE EDITED
        // transform.localPosition = new Vector3(x, y, transform.localPosition.z);

        if (!inserted && !roll)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                if (Arm.GetBlendShapeWeight(1) > 0 || Arm.GetBlendShapeWeight(2) > 0)
                {
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) - (rollrate * 2) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) - (rollrate * 2) * Time.deltaTime, minRollVal, maxRollVal));
                    vein.transform.position = Vector3.MoveTowards(vein.transform.position, defaultVeinRollPosition.position, 0.01f * Time.deltaTime);
                }
                
                rollval = 0;
            }
            //if (Time.time > resettime + resettimer)
            //{
            //    if (Arm.GetBlendShapeWeight(1) > 0)
            //    {
            //       Arm.SetBlendShapeWeight(1, Arm.GetBlendShapeWeight(1) - (rollrate * 2) * Time.deltaTime);
            //    }
            //    if (Arm.GetBlendShapeWeight(2) > 0)
            //    {
            //        Arm.SetBlendShapeWeight(2, Arm.GetBlendShapeWeight(2) - (rollrate * 2) * Time.deltaTime);
            //    }
            //    rollval = 0;
            //    transform.localPosition = new Vector3(x, y, basez);
            //}
        }

        if (roll && !bulge)
        {
            //print("Roll is true AND we are not bulging");
            if (rollval > 0) //rollval == 100 we are moving right
            {
                var distance = Vector3.Distance(vein.transform.position, rightVeinRollPosition.position);
                //print("distance to rightveinRollposition: " + distance);
                //print("rollval is greater than 0");
                if (Arm.GetBlendShapeWeight(1) < maxRollVal || Arm.GetBlendShapeWeight(2) > minRollVal || distance > 0.001f) //probably also want to set it up that it will continue until the leftposition has been reached //check the distance like what we are doing in the onTriggerEnter
                {
                    previousPosition = vein.transform.position;
                   // print("BlendShapeweight(1) is less than its maxRollVal");
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) + (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) - (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    vein.transform.position = Vector3.MoveTowards(vein.transform.position, rightVeinRollPosition.position, 0.004f * Time.deltaTime);
                    //here we also want to move our vein cube to its new position of the leftPosition
                }
                else
                {
                    //print("At Max Value --> Right BlendShapeWeight: " + Arm.GetBlendShapeWeight(1) + " AND At MinValue --> Left BlendShapeWeight: " + Arm.GetBlendShapeWeight(2));
                }
                //print("rollVal : " + rollval);
                ////Pushed from left blend to now right blend
                //if (Arm.GetBlendShapeWeight(2) > 0)
                //{
                //    print("Push from left blend to now right blend");
                //    Arm.SetBlendShapeWeight(2, Arm.GetBlendShapeWeight(2) - rollrate * Time.deltaTime);
                //    if (Arm.GetBlendShapeWeight(2) < 0)
                //    {
                //        Arm.SetBlendShapeWeight(2, 0);
                //    }
                //}

                ////Blend shape 2 is now < 0
                //else
                //{
                //    print("Blend shape 2 is now less than 0");
                //    if (rollval > Arm.GetBlendShapeWeight(1))
                //    {
                //        print("rollval is larger than the current blendshape val");
                //        //rollval is larger then the current blendshape val
                //        Arm.SetBlendShapeWeight(1, Arm.GetBlendShapeWeight(1) + rollrate * Time.deltaTime);
                //        if (Arm.GetBlendShapeWeight(1) > rollval)
                //        {
                //            Arm.SetBlendShapeWeight(1, rollval);
                //        }
                //        else
                //        {
                //            roll = false;
                //        }
                //    }
                //    else
                //    {
                //        //rollval is smaller then the current blendshape val
                //        print("rollval is smaller than the current blendshape val");
                //        Arm.SetBlendShapeWeight(1, Arm.GetBlendShapeWeight(1) - rollrate * Time.deltaTime);
                //        if (Arm.GetBlendShapeWeight(1) < rollval)
                //        {
                //            Arm.SetBlendShapeWeight(1, rollval);
                //        }
                //        else
                //        {
                //            roll = false;
                //        }
                //    }
                //}
            }
            else if(rollval < 0)
            {
                var distance = Vector3.Distance(vein.transform.position, leftVeinRollPosition.position);
                //print("rollval is greater than 0");;
                //print("rollval is greater than 0");
                if (Arm.GetBlendShapeWeight(2) < maxRollVal || Arm.GetBlendShapeWeight(1) > minRollVal || distance > 0.001)
                {
                    previousPosition = vein.transform.position;
                    // print("BlendShapeweight(1) is less than its maxRollVal");
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) + (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) - (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    vein.transform.position = Vector3.MoveTowards(vein.transform.position, leftVeinRollPosition.position, 0.004f * Time.deltaTime);
                    //here we also want to move our vein cube to its new position of the rightPosition
                }
                else
                {
                    //print("At Max Value --> Right BlendShapeWeight: " + Arm.GetBlendShapeWeight(1) + " AND At MinValue --> Left BlendShapeWeight: " + Arm.GetBlendShapeWeight(2));
                }

            }
            else if(rollval == 0)
            {
                print("Roll val is 0");
                vein.transform.position = previousPosition;
            }


//            else //rollval -100 || 0
//            {
//                print("rollval is less than 0");
//                if (Arm.GetBlendShapeWeight(2) < maxRollVal)
//                {
//                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) + (rollrate * 2) * Time.deltaTime, minRollVal, maxRollVal));
//                }
//                else
//                {
//                    print("Left BlendShapeWeight: " + Arm.GetBlendShapeWeight(2));
//;                }
//                if (Arm.GetBlendShapeWeight(1) > 0)
//                {
//                    Arm.SetBlendShapeWeight(1, Arm.GetBlendShapeWeight(1) - rollrate * Time.deltaTime);
//                    if (Arm.GetBlendShapeWeight(1) < 0)
//                    {
//                        Arm.SetBlendShapeWeight(1, 0);
//                    }
//                    else
//                    {
//                        roll = false;
//                    }
//                }
//                else
//                {
//                    if (Mathf.Abs(rollval) > Arm.GetBlendShapeWeight(2))
//                    {
//                        Debug.Log("Increasing Blend2");
//                        //rollval is larger then the current blendshape val
//                        Arm.SetBlendShapeWeight(2, Arm.GetBlendShapeWeight(2) + rollrate * Time.deltaTime);
//                        if (Arm.GetBlendShapeWeight(2) > Mathf.Abs(rollval))
//                        {
//                            Arm.SetBlendShapeWeight(2, Mathf.Abs(rollval));
//                        }
//                        else
//                        {
//                            roll = false;
//                        }
//                    }
//                    else
//                    {
//                        Debug.Log("Decrease Blend2");
//                        //rollval is smaller then the current blendshape val
//                        Arm.SetBlendShapeWeight(2, Arm.GetBlendShapeWeight(2) - rollrate * Time.deltaTime);
//                        if (Arm.GetBlendShapeWeight(2) < Mathf.Abs(rollval))
//                        {
//                            Arm.SetBlendShapeWeight(2, Mathf.Abs(rollval));
//                        }
//                        else
//                        {
//                            roll = false;
//                        }
//                    }
//                }
//            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {  
        //print(other.name + " : " + other.tag + " has entered into veinmove collider");
        
        if ((other.transform.tag == "Needle" && !bulge))
        {
            //Debug.Log("Collision Entered");
            Vector3 tmp = vein.transform.InverseTransformPoint(other.transform.position);
            if (tmp.z < 0)
            {
                //Vein was touched on the left side so it must roll right
                //Debug.Log("Going right");
                //if (vein.transform.localPosition.z + distance <= -0.0491)
                //{
                //    vein.transform.localPosition = new Vector3(x, y, vein.transform.localPosition.z + distance);
                //}
                var distance = Vector3.Distance(vein.transform.position, rightVeinRollPosition.position);
                if (distance <= 0.001f)
                {
                    //if we are basically are the right vein position, we want to set it to be at the right vein position and not pursue continuing to roll. 
                    vein.transform.position = rightVeinRollPosition.position;
                }
                else
                {
                    //vein.transform.position = new Vector3(vein.transform.position.x - 0.0001f, vein.transform.position.y, vein.transform.position.z);
                    rollval = 100;
                    roll = true;
                    ResetTimer();
                    //resettimer = Time.time;
                    //I get it now, we want to move in increment big enought that it will re-trigger a OnTriggerEnter since we would be leaving the collider; is this necessarily better? 
                }
            }
            else //vein was touched on the right so it must roll left
            {
                
               //Debug.Log("Going left");
                //if (vein.transform.localPosition.z - distance >= -0.0478)
                //{
                //  vein.transform.localPosition = new Vector3(x, y, vein.transform.localPosition.z - distance);
                //}
                var distance = Vector3.Distance(vein.transform.position, leftVeinRollPosition.position);
                if (distance <= 0.001f)
                {
                    vein.transform.position = leftVeinRollPosition.position;
                    //if we are basically are the left vein position, we want to set it to be at the left vein position and not pursue continuing to roll.
                }
                else
                {
                    //vein.transform.position = new Vector3(vein.transform.position.x + 0.0001f, vein.transform.position.y, vein.transform.position.z);
                    roll = true;
                    rollval = -100;
                    ResetTimer();
                    //resettimer = Time.time;
                }
            }
            //-0.0491 //right
            //-0.0478 //left
        }

        else
        {
            //print(other.name + " " + bulge);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Needle" && !bulge)
        {
            roll = false;
            rollval = 0; 

            //start timer here to return blendshape and box to default positions and values
        }
    }

    public void BulgeActivate()
    {
        bulge = true;
    }

    public void BulgeDectivate()
    {
        bulge = false;
    }

    public void VeinEntered()
    {
        inserted = true;
    }

    public void VeinLeft()
    {
        inserted = false;
    }

    public void ResetTimer()
    {
        timeLeft = timeTillReset;
    }
}
