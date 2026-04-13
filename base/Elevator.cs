using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private AerodynamicCalculator script;

    // Start is called before the first frame update
    private void Start()
    {
        script = GameObject.Find("Aircraft").GetComponent<AerodynamicCalculator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.AngleAxis(script.de, Vector3.forward);
    }
}