using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AerodynamicCalculator : MonoBehaviour
{
    // public

    [SerializeField] private InputActionProperty _actionProp;

    // ===== InputSystemの有効化 =====
    private void OnEnable()
    {
        _actionProp.action.Enable();
    }

    // ===== InputSystemの無効化 =====
    private void OnDisable()
    {
        _actionProp.action.Disable();
    }

    public enum Status
    {
        PreFlight, // Take-off preparation
        InFlight, // Flight
        Landing // Landing
    }
    public Status status = Status.PreFlight; // Flight status

    public float Airspeed_TO = 5.0f; // Airspeed at take-off [m/s]

    public float alpha_TO = 0.0f; // Angle of attack at take-off [deg]

    //public bool MousePitchControl = true; // Mouse pitch control: True, False

    //public float MouseSensitivity = 1.0f; // Mouse sensitivity

    public float GustMag = 0.0f; // Gust magnitude at 10m [m/s]

    public float GustDirection = 0.0f; // Gust direction at 10m [deg]

    public Vector3 PlatformPosition = Vector3.zero; // Position of the platform [m]

    public bool pressingSpaceKey = false; // Space key pressing status

    [System.NonSerialized]
    public float Airspeed = 0.000f; // Airspeed [m/s]

    [System.NonSerialized]
    public float alpha = 0.000f; // Angle of attack [deg]

    [System.NonSerialized]
    public float beta = 0.000f; // Side slip angle [deg]

    [System.NonSerialized]
    public float de = 0.000f; // Elevator angle [deg]

    [System.NonSerialized]
    public float dr = 0.000f; // Rudder angle [deg]

    [System.NonSerialized]
    public float dh = 0.000f; // Movement of c.g. [-]

    [System.NonSerialized]
    public float LocalGustMag = 0.000f; // Magnitude of local gust [m/s]

    [System.NonSerialized]
    public float LocalGustDirection = 0.000f; // Magnitude of local gust [m/s]

    [System.NonSerialized]
    public float nz = 0.000f; // Load factor [-]

    // Phisics
    static private float rho = 1.164f;

    static private float hE0 = 10.500f; // Altitude at Take-off [m]

    // At Cruise without Ground Effect
    static private float Airspeed0; // Magnitude of ground speed [m/s]

    static private float alpha0; // Angle of attack [deg]
    static private float CDp0; // Parasitic drag [-]
    static private float Cmw0; // Pitching momentum [-]
    static private float CLMAX; // Lift Coefficient [-]
    static private float CL0 = 0.000f; // Lift Coefficient [-]
    static private float CLw0 = 0.000f; // Lift Coefficient [-]
    static private float CLt0 = 0.000f; // Tail Coefficient [-]
    static private float epsilon0 = 0.000f; // Downwash

    // Aircraft
    private static bool Downwash; // Conventional Tail: True, T-Tail: False

    static private float CL = 0.000f; // Lift Coefficient [-]
    static private float CD = 0.000f; // Drag Coefficient [-]
    static private float Cx = 0.000f; // X Force Coefficient [-]
    static private float Cy = 0.000f; // Y Force Coefficient [-]
    static private float Cz = 0.000f; // Z Force Coefficient [-]
    static private float Cl = 0.000f; // Rolling momentum [-]
    static private float Cm = 0.000f; // Pitching momentum [-]
    static private float Cn = 0.000f; // Yawing momentum [-]
    static private float dh0 = 0.000f; // Initial Mouse Position

    // Wing
    static private float Sw; // Wing area of wing [m^2]

    static private float bw; // Wing span [m]
    static private float cMAC; // Mean aerodynamic chord [m]
    static public float aw; // Wing Lift Slope [1/deg]
    static private float hw; // Length between Wing a.c. and c.g. [-]
    static private float AR; // Aspect Ratio [-]
    static private float ew; // Wing efficiency [-]
    static private float CLw = 0.000f; // Lift Coefficient [-]

    // Tail
    static private float St; // Wing area of tail [m^2]

    static private float at; // Tail Lift Slope [1/deg]
    static private float lt; // Length between Tail a.c. and c.g. [m]
    static private float VH; // Tail Volume [-]
    static private float deMAX; // Maximum elevator angle [deg]
    static private float tau; // Control surface angle of attack effectiveness [-]
    static private float CLt = 0.000f; // Lift Coefficient [-]

    // Fin
    static private float drMAX; // Maximum rudder angle

    // Ground Effect
    static private float CGEMIN; // Minimum Ground Effect Coefficient [-]

    static private float CGE = 0f; // Ground Effect Coefficient: CDiGE/CDi [-]

    // Stability derivatives
    static private float Cyb; // [1/deg]

    static private float Cyp; // [1/rad]
    static private float Cyr; // [1/rad]
    static private float Cydr; // [1/deg]
    static private float Cnb; // [1/deg]
    static private float Cnp; // [1/rad]
    static private float Cnr; // [1/rad]
    static private float Cndr; // [1/deg]
    static private float Clb; // [1/deg]
    static private float Clp; // [1/rad]
    static private float Clr; // [1/rad]
    static private float Cldr; // [1/deg]

    // Gust
    static private Vector3 Gust = Vector3.zero; // Gust [m/s]

    // Rotation
    static private float phi; // [deg]

    static private float theta;  // [deg]
    static private float psi; // [deg]

    static private Transform lastTransform;

    private Rigidbody AircraftRigidbody;

    // Start is called before the first frame update
    private void Start()
    {
        // Get rigidbody component
        AircraftRigidbody = GetComponent<Rigidbody>();

        // Input Specifications
        InputSpecifications();

        //Airspeed_TO = 5.0f; // Airspeed at take-off [m/s]
        AircraftRigidbody.linearVelocity = Vector3.zero;

        // Calculate CL at cluise
        CL0 = (AircraftRigidbody.mass * Physics.gravity.magnitude) / (0.5f * rho * Airspeed0 * Airspeed0 * Sw);
        CLt0 = (Cmw0 + CL0 * hw) / (VH + (St / Sw) * hw);
        CLw0 = CL0 - (St / Sw) * CLt0;
        if (Downwash) { epsilon0 = (CL0 / (Mathf.PI * ew * AR)) * Mathf.Rad2Deg; }

        dh0 = Screen.height / 2f; // Initial Mouse Position

        //Debug.Log(CLw0);
    }

    private void FixedUpdate()
    {
        DetectPressingSpaceKey();

        if (status == Status.PreFlight)
        {
            AircraftRigidbody.isKinematic = true; // 物理挙動を無効化.
            AircraftRigidbody.transform.position = new Vector3(-10f, 11f, 0f); // Take-off position
            AircraftRigidbody.transform.rotation = Quaternion.Euler(0f, 0f, alpha_TO); // Take-off attitude
            return;
        }
        else if (status == Status.Landing)
        {
            AircraftRigidbody.isKinematic = true; // 物理挙動を無効化.
            return;
        }
        else
        {
            AircraftRigidbody.isKinematic = false; // 物理挙動を有効化.
        }

        // Velocity and AngularVelocity
        // InverseTransformDirection(): ワールド空間からローカル空間にベクトルを変換.

        // ----- 航空力学で一般的に使用される座標系への変換（X座標前方）--------------------------------------------
        float u = transform.InverseTransformDirection(AircraftRigidbody.linearVelocity).x;
        float v = -transform.InverseTransformDirection(AircraftRigidbody.linearVelocity).z;
        float w = -transform.InverseTransformDirection(AircraftRigidbody.linearVelocity).y;

        float p = -transform.InverseTransformDirection(AircraftRigidbody.angularVelocity).x * Mathf.Rad2Deg;
        float q = transform.InverseTransformDirection(AircraftRigidbody.angularVelocity).z * Mathf.Rad2Deg;
        float r = transform.InverseTransformDirection(AircraftRigidbody.angularVelocity).y * Mathf.Rad2Deg;
        // ----------------------------------------------------------------------------------------------------

        float hE = AircraftRigidbody.position.y;

        // Force and Momentum
        Vector3 AerodynamicForce = Vector3.zero;
        Vector3 AerodynamicMomentum = Vector3.zero;
        Vector3 TakeoffForce = Vector3.zero;

        // Hoerner and Borst (Modified)
        CGE = (CGEMIN + 33f * Mathf.Pow((hE / bw), 1.5f)) / (1f + 33f * Mathf.Pow((hE / bw), 1.5f));

        // Get control surface angles
        de = 0.000f;
        dr = 0.000f;
        //if (MousePitchControl)
        //{
        //    dh = -(Input.mousePosition.y - dh0) * 0.0002f * MouseSensitivity;
        //}
        //Debug.Log(dh);

        Vector2 InputArrowKey = _actionProp.action.ReadValue<Vector2>();
        de = InputArrowKey.y * deMAX;
        dr = -InputArrowKey.x * drMAX;
        //Debug.Log(InputArrowKey);

        if (Mouse.current.leftButton.isPressed) { dr = drMAX; }
        else if (Mouse.current.rightButton.isPressed) { dr = -drMAX; }

        //Debug.Log(dh);

        // Gust
        LocalGustMag = GustMag * Mathf.Pow((hE / hE0), 1f / 7f);
        Gust = Quaternion.AngleAxis(GustDirection, Vector3.up) * (Vector3.right * LocalGustMag);
        Vector3 LocalGust = this.transform.InverseTransformDirection(Gust);
        float ug = LocalGust.x + 1e-10f;
        float vg = -LocalGust.z;
        float wg = -LocalGust.y;
        if (ug > 0) { LocalGustDirection = Mathf.Atan(vg / (ug + 1e-10f)) * Mathf.Rad2Deg; }
        else { LocalGustDirection = Mathf.Atan(vg / (ug + 1e-10f)) * Mathf.Rad2Deg + vg / Mathf.Abs((vg + 1e-10f)) * 180; }

        // Calculate angles
        Airspeed = Mathf.Sqrt((u + ug) * (u + ug) + (v + vg) * (v + vg) + (w + wg) * (w + wg));
        alpha = Mathf.Atan((w + wg) / (u + ug)) * Mathf.Rad2Deg;
        beta = Mathf.Atan((v + vg) / Airspeed) * Mathf.Rad2Deg;

        // Wing and Tail
        CLw = CLw0 + aw * (alpha - alpha0);
        CLt = CLt0 + at * ((alpha - alpha0) + (1f - CGE * (CLw / CLw0)) * epsilon0 + de * tau + ((lt - dh * cMAC) / Airspeed) * q);
        if (Mathf.Abs(CLw) > CLMAX) { CLw = (CLw / Mathf.Abs(CLw)) * CLMAX; } // Stall
        if (Mathf.Abs(CLt) > CLMAX) { CLt = (CLt / Mathf.Abs(CLt)) * CLMAX; } // Stall

        // Lift and Drag
        CL = CLw + (St / Sw) * CLt; // CL
        CD = CDp0 * (1f + Mathf.Abs(Mathf.Pow((alpha / 9f), 3f))) + ((CL * CL) / (Mathf.PI * ew * AR)) * CGE; // CD

        // Force
        Cx = CL * Mathf.Sin(Mathf.Deg2Rad * alpha) - CD * Mathf.Cos(Mathf.Deg2Rad * alpha); // Cx
        Cy = Cyb * beta + Cyp * (1f / Mathf.Rad2Deg) * ((p * bw) / (2f * Airspeed)) + Cyr * (1f / Mathf.Rad2Deg) * ((r * bw) / (2f * Airspeed)) + Cydr * dr; // Cy
        Cz = -CL * Mathf.Cos(Mathf.Deg2Rad * alpha) - CD * Mathf.Sin(Mathf.Deg2Rad * alpha); // Cz

        // Torque
        Cl = Clb * beta + Clp * (1f / Mathf.Rad2Deg) * ((p * bw) / (2f * Airspeed)) + Clr * (1f / Mathf.Rad2Deg) * ((r * bw) / (2f * Airspeed)) + Cldr * dr; // Cl
        Cm = Cmw0 + CLw * hw - VH * CLt + CL * dh; // Cm
        Cn = Cnb * beta + Cnp * (1f / Mathf.Rad2Deg) * ((p * bw) / (2f * Airspeed)) + Cnr * (1f / Mathf.Rad2Deg) * ((r * bw) / (2f * Airspeed)) + Cndr * dr; // Cn

        if (u > 0)
        {
            AerodynamicForce.x = 0.5f * rho * Airspeed * Airspeed * Sw * Cx;
            AerodynamicForce.y = 0.5f * rho * Airspeed * Airspeed * Sw * (-Cz);
            AerodynamicForce.z = 0.5f * rho * Airspeed * Airspeed * Sw * (-Cy);
        }

        AerodynamicMomentum.x = 0.5f * rho * Airspeed * Airspeed * Sw * bw * (-Cl);
        AerodynamicMomentum.y = 0.5f * rho * Airspeed * Airspeed * Sw * bw * Cn;
        AerodynamicMomentum.z = 0.5f * rho * Airspeed * Airspeed * Sw * cMAC * Cm;

        float DistanceXAxis = AircraftRigidbody.transform.position.x;
        if (DistanceXAxis < -0.5f)
        {
            CalculateRotation();

            float W = AircraftRigidbody.mass * Physics.gravity.magnitude;
            float L = 0.5f * rho * Airspeed * Airspeed * Sw * (Cx * Mathf.Sin(Mathf.Deg2Rad * theta) - Cz * Mathf.Cos(Mathf.Deg2Rad * theta));
            float N = (W - L) * Mathf.Cos(Mathf.Deg2Rad * 3.5f); // N=(W-L)*cos(3.5deg)
            float P = (AircraftRigidbody.mass * Airspeed_TO * Airspeed_TO) / (2f * 10f); // P=m*Vto*Vto/2*L

            TakeoffForce.x = P;
            TakeoffForce.y = N * Mathf.Cos(Mathf.Deg2Rad * 3.5f);
            TakeoffForce.z = 0f;

            AerodynamicForce.z = 0f;
            AerodynamicMomentum.x = 0f;
            AerodynamicMomentum.y = 0f;
        }
        //Debug.Log(Distance);

        AircraftRigidbody.AddRelativeForce(AerodynamicForce, ForceMode.Force); // ローカル座標.
        AircraftRigidbody.AddRelativeTorque(AerodynamicMomentum, ForceMode.Force); // ローカル座標.
        AircraftRigidbody.AddForce(TakeoffForce, ForceMode.Force); // ワールド座標.

        nz = AerodynamicForce.y / (AircraftRigidbody.mass * Physics.gravity.magnitude);

        float Distance = (AircraftRigidbody.position - PlatformPosition).magnitude;
        float Altitude = AircraftRigidbody.position.y - 0.5f; // 機体の図心が地面からおよそ0.5m
        Debug.Log("Distance: " + Distance + "m    Altitude: " + Altitude + "m");
    }

    private void CalculateRotation()
    {
        float q1 = AircraftRigidbody.transform.rotation.x;
        float q2 = -AircraftRigidbody.transform.rotation.y;
        float q3 = -AircraftRigidbody.transform.rotation.z;
        float q4 = AircraftRigidbody.transform.rotation.w;

        float C11 = q1 * q1 - q2 * q2 - q3 * q3 + q4 * q4;
        float C22 = -q1 * q1 + q2 * q2 - q3 * q3 + q4 * q4;
        float C12 = 2f * (q1 * q2 + q3 * q4);
        float C13 = 2f * (q1 * q3 - q2 * q4);
        float C32 = 2f * (q2 * q3 - q1 * q4);

        phi = -Mathf.Atan(-C32 / C22) * Mathf.Rad2Deg;
        theta = -Mathf.Asin(C12) * Mathf.Rad2Deg;
        psi = -Mathf.Atan(-C13 / C11) * Mathf.Rad2Deg;
    }

    // ===== 機体諸元を変数に適用する関数 ====================================
    private void InputSpecifications()
    {
        // ----- ARG-2 ----------------------------------------------------------------------------------------------

        // Aircraft
        AircraftRigidbody.mass = 103.100f;
        AircraftRigidbody.centerOfMass = new Vector3(0f, -0.019f, 0f);
        AircraftRigidbody.inertiaTensor = new Vector3(961f, 1024f, 80f); //Ixx（ロール）, Izz（ヨー）, Iyy（ピッチ）
        AircraftRigidbody.inertiaTensorRotation = Quaternion.AngleAxis(-3.929f, Vector3.forward);

        // Specification At Cruise without Ground Effect
        Airspeed0 = 10.500f; // Magnitude of ground speed [m/s]
        alpha0 = 1.407f; // Angle of attack [deg]
        CDp0 = 0.014f; // Parasitic drag [-]
        Cmw0 = -0.165f; // Pitching momentum [-]
        CLMAX = 1.700f;
        // Wing
        Sw = 18.009f; // Wing area of wing [m^2]
        bw = 23.350f; // Wing span [m]
        cMAC = 0.813f; // Mean aerodynamic chord [m]
        aw = 0.103f; // Wing Lift Slope [1/deg]
        hw = (0.3375f - 0.250f); // Length between Wing a.c. and c.g.
        ew = 0.986f; // Wing efficiency
        AR = (bw * bw) / Sw; // Aspect Ratio
                             // Tail
        Downwash = true; // Conventional Tail: True, T-Tail: False
        St = 1.651f; // Wing area of tail
        at = 0.074f; // Tail Lift Slope [1/deg]
        lt = 3.200f; // Length between Tail a.c. and c.g.
        deMAX = 10.000f; // Maximum elevator angle
        tau = 1.000f; // Control surface angle of attack effectiveness [-]
        VH = (St * lt) / (Sw * cMAC); // Tail Volume
                                      // Fin
        drMAX = 15.000f; // Maximum rudder angle
                         // Ground Effect
        CGEMIN = 0.215f; // Minimum Ground Effect Coefficient [-]
                         // Stability derivatives
        Cyb = -0.003764f; // [1/deg]
        Cyp = -0.411848f; // [1/rad]
        Cyr = 0.141631f; // [1/rad]
        Cydr = 0.001846f; // [1/deg]
        Clb = -0.003656f; // [1/deg]
        Clp = -0.816226f; // [1/rad]
        Clr = 0.219104f; // [1/rad]
        Cldr = 0.000032f; // [1/deg]
        Cnb = -0.000245f; // [1/deg]
        Cnp = -0.127263f; // [1/rad]
        Cnr = -0.002745f; // [1/rad]
        Cndr = -0.000308f; // [1/deg]
    }

    private void OnCollisionEnter(Collision collision)
    {
        status = Status.Landing; // 着陸ステータスに変更.
    }

    private void DetectPressingSpaceKey()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.isPressed && !pressingSpaceKey)
        {
            pressingSpaceKey = true;
            Debug.Log("Space key was pressed.");

            if (status == Status.PreFlight)
            {
                status = Status.InFlight;
            }
            else
            {
                status = Status.PreFlight;
            }
        }
        else if (!Keyboard.current.spaceKey.isPressed && pressingSpaceKey)
        {
            pressingSpaceKey = false;
        }
    }
}