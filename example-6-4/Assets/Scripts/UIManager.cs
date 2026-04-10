using UnityEngine;
using UnityEngine.UIElements; // UI Toolkit

public class UIManager : MonoBehaviour
{
    private VisualElement preflight;
    private VisualElement inflight;
    private VisualElement landing;

    private GameObject altitudeSensor;

    private void Start()
    {
        preflight = GameObject.Find("PreFlight").GetComponent<UIDocument>().rootVisualElement;
        inflight = GameObject.Find("InFlight").GetComponent<UIDocument>().rootVisualElement;
        landing = GameObject.Find("Landing").GetComponent<UIDocument>().rootVisualElement;

        altitudeSensor = GameObject.Find("AltitudeSensor");
    }

    private void Update()
    {
        //Debug.Log(altitudeSensor.transform.position.y.ToString("0.000"));
        //parameters.altitudeString = altitudeSensor.transform.position.y.ToString("0.000");
        //Debug.Log(parameters.altitudeString);

        switch (GameManager.instance.status)
        {
            case GameManager.Status.PreFlight:
                preflight.style.display = DisplayStyle.Flex; // PreflightのUIを表示
                inflight.style.display = DisplayStyle.None; // InFlightのUIを非表示
                landing.style.display = DisplayStyle.None; // LandingのUIを非表示

                Button start = preflight.Q<Button>("Start");
                start.clicked += () =>
                {
                    GameManager.instance.status = GameManager.Status.InFlight;
                };
                break;

            case GameManager.Status.InFlight:
                preflight.style.display = DisplayStyle.None; // PreflightのUIを非表示
                inflight.style.display = DisplayStyle.Flex; // InFlightのUIを表示
                landing.style.display = DisplayStyle.None; // LandingのUIを非表示

                Label distanceValue = inflight.Q<Label>("DistanceValue");
                if (distanceValue != null)
                {
                    float distance = Vector3.Distance(GameManager.instance.Aircraft.transform.position, GameManager.instance.PlatformPosition);
                    distanceValue.text = distance.ToString("0.000");
                }
                Label altitudeValue = inflight.Q<Label>("AltitudeValue");
                if (altitudeValue != null)
                {
                    altitudeValue.text = altitudeSensor.transform.position.y.ToString("0.000");
                }

                break;

            case GameManager.Status.Landing:
                preflight.style.display = DisplayStyle.None; // PreflightのUIを非表示
                inflight.style.display = DisplayStyle.None; // InFlightのUIを非表示
                landing.style.display = DisplayStyle.Flex; // LandingのUIを表示

                Button retry = landing.Q<Button>("Retry");
                retry.clicked += () =>
                {
                    GameManager.instance.status = GameManager.Status.PreFlight;
                };

                Label distanceLabel = landing.Q<Label>("Distance");
                if (distanceLabel != null)
                {
                    float distance = Vector3.Distance(GameManager.instance.Aircraft.transform.position, GameManager.instance.PlatformPosition);
                    distanceLabel.text = distance.ToString("0.000");
                }
                break;

            default:
                preflight.style.display = DisplayStyle.None; // PreflightのUIを非表示
                landing.style.display = DisplayStyle.None; // LandingのUIを非表示
                break;
        }
    }
}