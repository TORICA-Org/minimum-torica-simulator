using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject aircraft;

    private GameObject sideCameraObj;
    private GameObject backCameraObj;

    private Vector3 offset = new(10, 3, 10);

    private void Start()
    {
        aircraft = GameManager.instance.Aircraft;

        sideCameraObj = GameObject.Find("SideCamera");
        backCameraObj = GameObject.Find("BackCamera");
    }

    private void Update()
    {
        sideCameraObj.transform.position = aircraft.transform.position + offset;
        sideCameraObj.transform.LookAt(aircraft.transform);
    }
}