using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[Serializable]
public struct NeedleInsertion
{
	public float enterTime;
	public int angleDegrees;
	public int veinEnterCount;
	public int veinExitCount;
	public float veinDepth;
	public float distFromVeinMM;
	public float exitTime;
}

public class SyringeController : MonoBehaviour
{

	[Header("Haptic Settings")]
    public Camera playerCamera;
    public HapticPlugin hapticDevice;
    public HapticSurface skinSurface;
    public GameObject grabber;
    public GameObject blocker;
    public GameObject blockerPos;
    public Transform armTangent;
    public Vector3 nowGrabberPos;
    public Vector3 pastGrabberPos;
    public float rotateSpeed = 1;
    public float elevateSpeed = 0.01f;
    public float velocity = 0; // m/s


    [Header("Skin Settings")]
    public float needleAngle;
    public bool isInsideSkin;
    public bool isInsideVein;
    public RaycastHit lastSkinHit;
    public RaycastHit lastVeinHit;
    NeedleInsertion lastInsertion;
	private int armMask; 


    [Header("Blood Settings")]
    //updated 11/12/2021
    public GameObject bloodSpotPrefab;
    public GameObject lastBloodSpot;
    public bool bloodCoroutineCalled = false;
    public List<GameObject> bloodList;
    float blood = 1;

    [Header("Needle Settings")]
    public GameObject originalNeedle;
    public GameObject fakeNeedle;
    public GameObject needleColor;


    [Header("Vein Settings")]
    public GameObject needlePoint;
    public GameObject veinCopy;
    bool setVeinHaptic = false;
	private int veinMask; 


    [Header("Other")]
    public GameObject dummy;
    public bool done = false;
	int returnStack = 0;
    bool isblocker = false;
    public GameObject mol;




    void Start()
    {
		bloodList = new List<GameObject>();
		if (!playerCamera)
			playerCamera = GetComponentInParent<Camera>();
		if (!armTangent)
			armTangent = GameObject.Find("ArmTangent").transform;
		if (!blocker)
			blocker = GameObject.Find("Blocker");

		lastInsertion = new NeedleInsertion();

		veinMask = LayerMask.NameToLayer("PatientVein");
		armMask = LayerMask.NameToLayer("arm");
	}

	void Update()
	{
        if (hapticDevice.touching)
        {
			if (!isblocker&&hapticDevice.touching.name!= "cylinder")
			{
				isblocker = true;
				blocker.SetActive(true);
				blocker.transform.position = transform.position;
				blocker.transform.rotation = transform.rotation;
				blocker.transform.position = blockerPos.transform.position;

			}
		}
		BloodToNeedle();

		if (Input.GetKeyDown(KeyCode.Return))
        {
            if (returnStack == 0)
            {
				returnStack++;
            }
            else
            {
				if (!done)
				{
					done = true;
					fakeNeedle.transform.position = originalNeedle.transform.position;
					fakeNeedle.transform.rotation = originalNeedle.transform.rotation;
					fakeNeedle.SetActive(true);
					originalNeedle.SetActive(false);
				}
			}
            
        }


		//if (!SessionManager.sessionActive) {
		//	return;
		//}

		

		// Maximize friction when touching, to emulate needle putting pressure on surface
		if (hapticDevice) {
			if (hapticDevice.touchingDepth > 4) {
				skinSurface.hlStaticFriction = 1;
				skinSurface.hlDynamicFriction = 1;
				print("Increasing Static and Dynamic Friction");
			}
			else {
				skinSurface.hlStaticFriction = 0.2f;
				skinSurface.hlDynamicFriction = 0.2f;
			}
		}

		UpdateNeedleAngle();
		UpdateNeedleStatus();
		updateVelocity();

		/*
        if (Input.GetKeyDown(KeyCode.Space))
        {
			bloodCoroutineCalled = false;
			blood = 1;
			eraseBlood();
			needleColor.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", 1);
			done = false;
			originalNeedle.SetActive(true);
			fakeNeedle.SetActive(false);
        }*/
	}

	void updateVelocity()
	{
		pastGrabberPos = nowGrabberPos;
		nowGrabberPos = grabber.transform.position;
		
			float v = Vector3.Distance(nowGrabberPos, pastGrabberPos);
			v /= Time.deltaTime;
			

			//UIManager.UpdateNeedleSpeed(v);
		
		
	}
	void UpdateNeedleAngle()
	{
		needleAngle = 90 - Vector3.Angle(-transform.up, armTangent.up);

		//if (SessionManager.curSession.sessionType == 0) {
		//	UIManager.UpdateNeedleAngle(needleAngle);
		//}
	}

	public void UpdateNeedleStatus()
	{
		// Cast ray from top of needle to near the tip, and try to detect skin/vein
		//Vector3 startPos = transform.position + transform.up * 0.05f;\
		Vector3 startPos = needlePoint.transform.position; 
		RaycastHit[] hits = Physics.RaycastAll(startPos, -transform.up, 0.049f);
		Vector3 testV = -transform.up;
		Debug.DrawRay(startPos, testV, Color.red,0.049f);
		testV.x *= 0.045f;
		testV.y *= 0.045f;
		testV.z *= 0.045f;
		
		bool wasInsideSkin = isInsideSkin;
		bool wasInsideVein = isInsideVein;
		isInsideSkin = false;
		isInsideVein = false;

		bool veinCheck = false;
		bool skinCheck = false;
		//this logic has to be changed
		foreach (RaycastHit hit in hits) {
			if (hit.transform.gameObject.layer == veinMask) 
			{
				//print(hit.transform.gameObject.name);
				Debug.DrawLine(startPos, hit.point, Color.green);
				veinCheck = true;
				isInsideVein = true;
				print("Is Inside vein");
				if (isInsideSkin)
					break;
			}
			else if (hit.transform.gameObject.layer == armMask/* || hit.transform.gameObject.name == mol.GetComponent<methodOfLimit>().refName */) {
                //print(hit.transform.gameObject.name);
                Debug.DrawLine(startPos, hit.point, Color.cyan);
				skinCheck = true;

				lastSkinHit = hit;
				Debug.DrawLine(hit.point, new Vector3(hit.point.x + hit.normal.x, hit.point.y + hit.normal.y, hit.point.z + hit.normal.z ), Color.blue);
			}
			else
			{
				print("Name: " + hit.transform.gameObject.layer);
			}
		}

		//Debug.Log("wS: " + wasInsideSkin.ToString() + ", iS: " + isInsideSkin.ToString()+", wV: " + wasInsideVein.ToString() + ", iV: " + isInsideVein.ToString());
        if (veinCheck)
        {
			isInsideVein = true;
        }
		else if (skinCheck)
        {
			isInsideSkin = true;
        }
		if (isInsideSkin && !wasInsideSkin&&!isInsideVein) {
			OnEnterSkin();
			//Debug.Log("EnteredSkin!");
		}
		
	
		if (isInsideVein && !wasInsideVein)
		{
			OnEnterVein();
			//Debug.Log("EnteredVein!");
		}

		if (wasInsideSkin && !isInsideSkin &&!isInsideVein) {
			OnExitSkin();
		}
		if(wasInsideVein&& veinCopy.GetComponent<VeinTriggerCheck>().isNeedleOut)
        {
			OnPassVein();
        }
		if (wasInsideVein && !isInsideVein) {
			OnExitVein();
		}

  //      if (!wasInsideVein && !wasInsideSkin && !isInsideVein && !isInsideSkin)
  //      {
		//	bloodCoroutineCalled = false;
		//	blood = 1;
		//	eraseBlood();
		//	needleColor.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", 1);
		//	done = false;
		//	originalNeedle.SetActive(true);
		//	fakeNeedle.SetActive(false);
		//}
	}

	void OnEnterSkin()
	{
		//if (SessionManager.curSession.sessionType == 0) {
		//	UIManager.UpdateNeedleStatus("Inside Skin", Color.Lerp(Color.yellow, Color.red, 0.5f));
		//}
		//lastInsertion.enterTime = SessionManager.curSession.timeElapsed;
		//lastInsertion.angleDegrees = Mathf.RoundToInt(needleAngle);

		if (blocker)
		{
			blocker.SetActive(true);
			blocker.transform.position = transform.position;
			blocker.transform.rotation = transform.rotation;
			blocker.transform.position = blockerPos.transform.position;

		}

		//mol modify:  uncommand below

		if (bloodSpotPrefab) {
			eraseBlood();
			//bloodList.Add(Instantiate(bloodSpotPrefab, lastSkinHit.point, Quaternion.Euler(lastSkinHit.normal)));
			//bloodList.Add(Instantiate(bloodSpotPrefab, lastSkinHit.point, Quaternion.FromToRotation(bloodSpotPrefab.transform.up, lastSkinHit.normal)));
			//Debug.Log("normal:" + lastSkinHit.normal);
			//Debug.Log("rotation: " + Quaternion.FromToRotation(bloodSpotPrefab.transform.up, lastSkinHit.normal));
			//lastBloodSpot = bloodList[bloodList.Count - 1];

			//lastBloodSpot = Instantiate(bloodSpotPrefab, lastSkinHit.point, Quaternion.LookRotation(lastSkinHit.normal));
			//lastBloodSpot.transform.position = lastBloodSpot.transform.position + lastBloodSpot.transform.up * 0.001f;
			//bloodList.Add(lastBloodSpot);
		}
        
			blocker.GetComponentInChildren<cylinderHaptic>().setSkinHapticParameter();
		
	}
	//need to fix for the test
	
	void OnEnterVein()
	{
		//if (SessionManager.curSession.sessionType == 0) {
		//	UIManager.UpdateNeedleStatus("Inside Vein", Color.magenta);
		//}
		//needleColor.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", 0);

		//mol: uncommand below
		
		bloodCoroutineCalled = true;
		//if (lastInsertion.enterTime == 0) {
		//	lastInsertion.enterTime = SessionManager.curSession.timeElapsed;
		//	if (bloodSpotPrefab) {
		//		//bloodList.Add(Instantiate(bloodSpotPrefab, lastSkinHit.point, Quaternion.Euler(lastSkinHit.normal)));
		//		//lastBloodSpot = bloodList[bloodList.Count - 1];

		//		//lastBloodSpot = Instantiate(bloodSpotPrefab, lastSkinHit.point, Quaternion.Euler(lastSkinHit.normal));
		//		//lastBloodSpot.transform.position = lastBloodSpot.transform.position + lastBloodSpot.transform.up * 0.001f;
		//		//bloodList.Add(lastBloodSpot);
		//	}

		//}
		// Set angle/depth if first time entering vein
		if (lastInsertion.veinEnterCount == 0) {
			lastInsertion.angleDegrees = Mathf.RoundToInt(needleAngle);
			lastInsertion.veinDepth = lastVeinHit.distance - lastSkinHit.distance;
			if (lastInsertion.veinDepth < 0)
				lastInsertion.veinDepth = 0;
		}
		lastInsertion.veinEnterCount++;

		// If already created blood spot, make it larger to indicate hitting the vein
		if (lastBloodSpot) {
			lastBloodSpot.transform.localScale = bloodSpotPrefab.transform.localScale * 3;
		}

		


		if (!setVeinHaptic)
        {
			setVeinHaptic = true;
			blocker.GetComponentInChildren<cylinderHaptic>().setVeinHapticParameter();
		}
		
	}

	void OnExitSkin()
	{
		//if (SessionManager.curSession.sessionType == 0) {
		//	UIManager.UpdateNeedleStatus("Outside Skin", Color.white);
		//}

		//lastInsertion.exitTime = SessionManager.curSession.timeElapsed;

		// Only add insertion if it was not immediately after the last (i.e. to mitigate shaky hands)
		//if (lastInsertion.exitTime - lastInsertion.enterTime > 0.5f) {
		//	SessionManager.AddSyringeInsertion(lastInsertion);
		//}
		lastInsertion = new NeedleInsertion(); // Reset last insertion, since syringe has exited

		// Disable blocker so needle isn't blocked
		if (blocker) {
			blocker.SetActive(false);
			isblocker = false;
		}

		//if (SessionManager.curSession.sessionType == 1 && SessionManager.curSession.needleInsertions.Count == SessionManager.testModeMaxTries) {
		//	UIManager.UpdateMiscFeedback("Completed Insertion. Ending Session...");
		//	StartCoroutine(EndTest());
		//}
		
	}

	void OnExitVein()
	{
		//if (SessionManager.curSession.sessionType == 0) {
		//	UIManager.UpdateNeedleStatus("Inside Skin", Color.Lerp(Color.yellow, Color.red, 0.5f));
		//}

		lastInsertion.veinExitCount++;
        if (setVeinHaptic)
        {
			setVeinHaptic = false;
			blocker.GetComponentInChildren<cylinderHaptic>().setSkinHapticParameter();
		}
		//mol modification

		if (blocker)
		{
			blocker.SetActive(false);
			isblocker = false;
		}

	}

	void OnPassVein()
	{
		//if (SessionManager.curSession.sessionType == 0)
		//{
		//	UIManager.UpdateNeedleStatus("Passed Vein", Color.Lerp(Color.red, Color.blue, 0.5f));
		//	dummy.GetComponent<cylinderHaptic>().setSkinHapticParameter();
		//	//mol modification : you have to uncommend code above
		//	//dummy.GetComponent<cylinderHaptic>().setVeinHapticParameter();
		//}
	}


	void checkNeedlePassed()
    {
		
    }
	IEnumerator EndTest()
	{
		yield return new WaitForSeconds(3.0f);

		//SessionManager.EndSession();
		SceneManager.LoadScene(2); // Load SessionEnd scene
	}

	void BloodToNeedle()
    {
		//Debug.Log("bloodCoroutineCalled1");
		if (bloodCoroutineCalled)
		{
			
			//Debug.Log("bloodCoroutineCalled1");
			blood -= Time.deltaTime*0.5f;
			if(blood <= 0)
            {
				blood = 0;
				//needleColor.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", blood);
				bloodCoroutineCalled = false;
            }
            else
            {
				//needleColor.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", blood);
			}
			
			
		}
		else
		{
			
		}

    }
	void eraseBlood()
    {
		foreach(GameObject blood in bloodList)
        {
			GameObject.Destroy(blood);
        }
		bloodList.Clear();
    }
}