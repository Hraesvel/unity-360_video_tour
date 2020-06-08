using UnityEngine;
using UnityEngine.XR;

public class DeviceDetect : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).IsValid ||
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).IsValid)
            Debug.Log("found a hand");
    }
}