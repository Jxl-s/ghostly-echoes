using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class LightScript : MonoBehaviour
{
    public float maxLightIntensity = 5;
    public float maxLightRange = 10;
    public float drainSpeed = 1f;
    public bool isOn = false;

    public GameObject spotLight;
    private Light flashLight;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = spotLight.GetComponent<Light>();
        InvokeRepeating("DecrementBattery", 1.0f, drainSpeed);
        flashLight.intensity = maxLightIntensity;
        flashLight.range = maxLightRange;
        toggleFlashlight(isOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ControlsEnabled)
        {
            HandleLight();
        }
    }

    void LateUpdate()
    {

    }

    // light manager
    public void HandleLight()
    {
        SetFlashlight();

        if (Input.GetKeyUp("r") && GameManager.Instance.BatteryPercentage > 0 && !isOn)
        {
            isOn = true;
            toggleFlashlight(isOn);
        }
        else if (Input.GetKeyUp("r") && isOn)
        {
            isOn = false;
            toggleFlashlight(isOn);
        }
        else if (GameManager.Instance.BatteryPercentage == 0)
        {

            isOn = false;
            toggleFlashlight(isOn);
        }
        if (Input.GetKeyUp("r") && GameManager.Instance.BatteryPercentage == 0)
        {
            Debug.Log("tryflash");
            // hud.Blink();

        }
    }

    public float LightPower()
    {
        if (GameManager.Instance.BatteryPercentage > 70)
        {
            return 1f;
        }
        else if (GameManager.Instance.BatteryPercentage > 40 && GameManager.Instance.BatteryPercentage <= 70)
        {
            return 0.6f;
        }
        else if (GameManager.Instance.BatteryPercentage > 0 && GameManager.Instance.BatteryPercentage <= 40)
        {
            return 0.4f;
        }
        else
        {
            return 0;
        }
    }

    public void toggleFlashlight(bool isOn)
    {
        spotLight.SetActive(isOn);
    }

    public void SetFlashlight()
    {
        flashLight.intensity = maxLightIntensity * LightPower();
        flashLight.range = maxLightRange * LightPower();
    }
    // lowers the GameManager.Instance.BatteryPercentage power over time
    public void DecrementBattery()
    {
        if (GameManager.Instance.BatteryPercentage > 0 && isOn)
        {
            // GameManager.Instance.BatteryPercentage -= 1;
            HUDManager.Instance.DecrementBatteryPercentage(1f);
        }
    }
}
