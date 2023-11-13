using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandController : MonoBehaviour
{
	public bool showController = false;
	public InputDevice controllerCharacteristics;
	public List<GameObject> controllerPrefabs;

	public GameObject handModelPrefab;

	private InputDevice targetDevice;
	private InputDeviceCharacteristics controller; 
	private GameObject spawnedController;
	private GameObject spawnedHandModel;
	//private Animator handAnimator;

	// Start is called before the first frame update
	void Start()
	{
		StartCoroutine(InitializeDevices());
		//TryInitialize();
	}
    private void Awake()
    {
		controller = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Left);
		//InputDevices.GetDevicesWithCharacteristics(controller, devices);
	}

    void TryInitialize()
	{
		List<InputDevice> devices = new List<InputDevice>();

		InputDevices.GetDevicesWithCharacteristics(controller, devices);

		if (devices.Count > 0)
		{
			targetDevice = devices[0];
			GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
			

			if (prefab)
			{
				spawnedController = Instantiate(prefab, transform);
			}
			else
			{
				Debug.LogError("Error");
				spawnedController = Instantiate(controllerPrefabs[0], transform);
			}

			spawnedHandModel = Instantiate(handModelPrefab, transform);
			print("Hand Model Spawned in");
			//handAnimator = spawnedHandModel.GetComponent<Animator>(); //initializing handAnimator
		}
        else
        {
			//print("Unable to find devices");
        }
		
	}
	void UpdateHandAnimation()
	{
		/*
		//script to control hand animations using our blend tree
		
		if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
		{
			//Setting the Float value of our grip animation to our trigger value
			handAnimator.SetFloat("Trigger", triggerValue);
		}
		else
		{
			//if we have no trigger value, then it is set to 0, and our trigger animation is not valid
			handAnimator.SetFloat("Trigger", 0);
		}

		//same thing but with our grip animation
		if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
		{
			handAnimator.SetFloat("Grip", gripValue);
		}
		else
		{
			handAnimator.SetFloat("Grip", 0);
		}
		*/
	}

	// Update is called once per frame
	void Update()
	{
		
		if(showController)
        {
			spawnedHandModel.SetActive(false);
			spawnedController.SetActive(true);
        }

        else
        {
            spawnedHandModel.SetActive(true);
			spawnedController.SetActive(false);
			//UpdateHandAnimation(); //we want to run the UpdateHandAnimation Function whenever the controllers are not being shown. 
        }
		
	}

	private void OnDeviceConnect(InputDevice device)
	{
		TryInitialize();
	}

	private void OnDeviceDisconnect(InputDevice device)
	{
		TryInitialize();
	}

	private void OnEnable()
	{
		InputDevices.deviceConnected += OnDeviceConnect;
		InputDevices.deviceDisconnected += OnDeviceDisconnect;
	}

	private void OnDisable()
	{
		InputDevices.deviceConnected -= OnDeviceDisconnect;
		InputDevices.deviceDisconnected -= OnDeviceDisconnect;
	}

	IEnumerator InitializeDevices()
    {
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		List<InputDevice> devices = new List<InputDevice>();
		InputDevices.GetDevicesWithCharacteristics(controller, devices);
		while(devices.Count == 0)
        {
			yield return wait;
			InputDevices.GetDevicesWithCharacteristics(controller, devices);
		}

		targetDevice = devices[0];
		GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);


		if (prefab)
		{
			spawnedController = Instantiate(prefab, transform);
		}
		else
		{
			Debug.LogError("Error");
			spawnedController = Instantiate(controllerPrefabs[0], transform);
		}

		spawnedHandModel = Instantiate(handModelPrefab, transform);
		print("Hand Model Spawned in");
		//handAnimator = spawnedHandModel.GetComponent<Animator>(); //initializing handAnimator
	}
}