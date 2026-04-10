using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    //[System.NonSerialized]
    public bool Landing = false;

    //[System.NonSerialized]
    public bool HUDActive = true;

    //[System.NonSerialized]
    public bool HorizontalLineActive = false;

    //[System.NonSerialized]
    public bool SettingActive = false;

    //[System.NonSerialized]
    public bool CameraSwitch = true; // true:FPS false:TPS

    //[System.NonSerialized]
    public bool SettingChanged = false;

    //[System.NonSerialized]
    public bool MousePitchControl = false;

    //[System.NonSerialized]
    public float MouseSensitivity = 1.000f; // Magnitude of Gust [m/s]

    //[System.NonSerialized]
    public float GustMag = 0.000f; // Magnitude of Gust [m/s]

    //[System.NonSerialized]
    public float GustDirection = 0.000f; // Direction of Gust [deg]: -180~180

    //[System.NonSerialized]
    public float Airspeed_TO = 5.000f; // Airspeed at take-off [m/s]

    //[System.NonSerialized]
    public float alpha_TO = 0.000f; // Angle of attack at take-off [deg]

    //[System.NonSerialized]
    public string AircraftName = "ARG-2";

    //[System.NonSerialized]
    public string FlightMode = "BirdmanRally";

    //[System.NonSerialized]
    public GameObject Aircraft = null;

    //[System.NonSerialized]
    public Vector3 PlatformPosition = new Vector3(0f, 10.5f, 0f);

    public enum Status
    {
        PreFlight,
        InFlight,
        Landing
    }

    public Status status = Status.PreFlight;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private bool pressing = false;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.isPressed && !pressing)
        {
            pressing = true;
            Debug.Log("Space key was pressed.");
            OnPressSpaceKey();
        }
        else if (!Keyboard.current.spaceKey.isPressed && pressing)
        {
            pressing = false;
        }
    }

    public void OnPressSpaceKey()
    {
        if (status == Status.PreFlight)
        {
            status = Status.InFlight;
        }
        else
        {
            status = Status.PreFlight;
        }
    }
}
