using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VeinMove : MonoBehaviour
{

    [Header("Vein Move Properties")]
    public GameObject vein;
    public SkinnedMeshRenderer Arm;
    public TextMeshProUGUI veinRollingText;
    public TextMeshProUGUI veinStabilizedText;
    public bool touched = false;
    public bool inserted = false;
    public bool bulge = false;
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


    void Start()
    {
        timeLeft = timeTillReset;
        x = transform.localPosition.x;
        y = transform.localPosition.y;
        basez = transform.localPosition.z;
        previousPosition = defaultVeinRollPosition.position;
        var distance =  Vector3.Distance(leftVeinRollPosition.position, rightVeinRollPosition.position);
        print(distance);
        veinStabilizedText.text = "Vein Stabilized: False";
        veinStabilizedText.color = Color.red;
        veinRollingText.text = "Vein Rolling: False";
        veinRollingText.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {

        if (!inserted && !roll)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                if (Arm.GetBlendShapeWeight(1) > 0 || Arm.GetBlendShapeWeight(2) > 0)
                {
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) - (rollrate * 2) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) - (rollrate * 2) * Time.deltaTime, minRollVal, maxRollVal));
                    transform.position = Vector3.MoveTowards(transform.position, defaultVeinRollPosition.position, 0.01f * Time.deltaTime);
                }
                
                rollval = 0;
            }
        }

        if (roll && !bulge)
        {

            if (rollval > 0) //rollval == 100 we are moving right
            {
                var distance = Vector3.Distance(transform.position, rightVeinRollPosition.position);
                if (Arm.GetBlendShapeWeight(1) < maxRollVal || Arm.GetBlendShapeWeight(2) > minRollVal || distance > 0.001f) //probably also want to set it up that it will continue until the leftposition has been reached //check the distance like what we are doing in the onTriggerEnter
                {
                    previousPosition = transform.position;
                   // print("BlendShapeweight(1) is less than its maxRollVal");
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) + (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) - (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                   transform.position = Vector3.MoveTowards(transform.position, rightVeinRollPosition.position, 0.004f * Time.deltaTime);
                }
            }
            else if(rollval < 0)
            {
                var distance = Vector3.Distance(transform.position, leftVeinRollPosition.position);
                if (Arm.GetBlendShapeWeight(2) < maxRollVal || Arm.GetBlendShapeWeight(1) > minRollVal || distance > 0.001)
                {
                    previousPosition = transform.position;
                    // print("BlendShapeweight(1) is less than its maxRollVal");
                    Arm.SetBlendShapeWeight(2, Mathf.Clamp(Arm.GetBlendShapeWeight(2) + (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    Arm.SetBlendShapeWeight(1, Mathf.Clamp(Arm.GetBlendShapeWeight(1) - (rollrate) * Time.deltaTime, minRollVal, maxRollVal));
                    transform.position = Vector3.MoveTowards(transform.position, leftVeinRollPosition.position, 0.004f * Time.deltaTime);
                }

            }
            else if(rollval == 0)
            {
                print("Roll val is 0");
                transform.position = previousPosition;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {  

        if ((other.transform.tag == "Needle" || other.transform.tag == "Index") && !bulge)
        {
            Vector3 tmp = transform.InverseTransformPoint(other.transform.position);
            if (tmp.z < 0)
            {
                var distance = Vector3.Distance(transform.position, rightVeinRollPosition.position);
                if (distance <= 0.001f)
                {
                    //if we are basically are the right vein position, we want to set it to be at the right vein position and not pursue continuing to roll. 
                    transform.position = rightVeinRollPosition.position;
                }
                else
                {
                    rollval = 100;
                    vein.gameObject.GetComponent<Collider>().enabled = false;
                    roll = true;
                    veinRollingText.text = "Vein Rolling: True"; 
                    veinRollingText.color = Color.green;
                    ResetTimer();

                }
            }
            else //vein was touched on the right so it must roll left
            {
               
                var distance = Vector3.Distance(transform.position, leftVeinRollPosition.position);
                if (distance <= 0.001f)
                {
                    transform.position = leftVeinRollPosition.position;
                    //if we are basically are the left vein position, we want to set it to be at the left vein position and not pursue continuing to roll.
                }
                else
                {
                    roll = true;
                    veinRollingText.text = "Vein Rolling: True";
                    veinRollingText.color = Color.green;
                    veinStabilizedText.text = "Vein Stabilized: False";
                    veinStabilizedText.color = Color.red;
                    rollval = -100;
                    vein.gameObject.GetComponent<Collider>().enabled = false;
                    ResetTimer();
                }
            }
        }

        else if (other.transform.tag == "Needle" && bulge)
        {
            veinRollingText.text = "Vein Rolling: False";
            veinRollingText.color = Color.red;
            veinStabilizedText.text = "Vein Stabilized: True";
            veinStabilizedText.color = Color.green; 
            roll = false;
            rollval = 0;
            vein.gameObject.GetComponent<Collider>().enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if((other.transform.tag == "Needle" || other.transform.tag == "Index"))
        {
            roll = false;
            veinRollingText.text = "Vein Rolling: False";
            veinRollingText.color = Color.red;
            veinStabilizedText.text = "Vein Stabilized: False";
            veinStabilizedText.color = Color.red;
            rollval = 0;
            vein.gameObject.GetComponent<Collider>().enabled = true;
        }

    }

    public void BulgeActivate()
    {
        bulge = true;
        veinStabilizedText.text = "Vein Stabilized: True";
        veinStabilizedText.color = Color.green;
    }

    public void BulgeDectivate()
    {
        bulge = false;
        veinStabilizedText.text = "Vein Stabilized: False";
        veinStabilizedText.color = Color.red;
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
