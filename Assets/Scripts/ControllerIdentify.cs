using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerIdentify : MonoBehaviour
{
    InputDeviceCharacteristics rightCtrl;
    InputDeviceCharacteristics leftCtrl;
    private void Awake()
    {

        rightCtrl = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right);
        leftCtrl = (InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left);

    }
    // Start is called before the first frame update
    void Start()
    {

        InitializeControllers();

    }

    private void InitializeControllers()
    {
        List<InputDevice> testDevices = new List<InputDevice>();

        //InputDeviceCharacteristics RightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        InputDevices.GetDevicesWithCharacteristics(rightCtrl, testDevices);

        InputDevices.GetDevicesWithCharacteristics(leftCtrl, testDevices);

        foreach (var item in testDevices)
        {
            print($"Name: {item.name}, Description: {item.characteristics}");
        }
        if (testDevices.Count == 0)
        {
            print("Unable to find devices in Controller Identify");
        }
    }

    private void OnDeviceConnect(InputDevice device)
    {
        InitializeControllers();
    }

    private void OnDeviceDisconnect(InputDevice device)
    {
        InitializeControllers();
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

}
