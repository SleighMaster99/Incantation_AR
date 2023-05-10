using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampassTest : MonoBehaviour
{
    public float north;
    public float realNorth;

    public Text northText;
    public Text realNorthText;

    bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        Input.location.Start();
        Input.compass.enabled = true;

        isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            north = Input.compass.magneticHeading;
            realNorth = Input.compass.trueHeading;

            northText.text = north.ToString();
            realNorthText.text = north.ToString();

            Camera.main.transform.rotation = Quaternion.Euler(0, realNorth, 0);
        }
        else
        {
            northText.text = "North";
            realNorthText.text = "RealNorth";
        }
    }

    public void EnableButton()
    {
        isOn = true;
    }

    public void DisableButton()
    {
        isOn = false;
    }
}
