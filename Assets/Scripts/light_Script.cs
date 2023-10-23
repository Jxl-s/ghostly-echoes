using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class light_Script : MonoBehaviour
{
    public int battery = 100;
    public float maxLightIntensity = 20;
    public float maxLightRange = 10;
    public float drainSpeed = 1f;
    public bool isOn = false;
    public GameObject spotLight;
    public Light light;
    public Text batteryText;
    public  Color batteryTextcolor = Color.red; 

    // Start is called before the first frame update
    void Start()
    {
        batteryText = GameObject.FindGameObjectWithTag("batteryText").GetComponent<Text>();
        spotLight = GameObject.FindGameObjectWithTag("flashlight_light");
        light = spotLight.GetComponent<Light>();
        InvokeRepeating("DecrementBattery", 1.0f, drainSpeed);
        light.intensity = maxLightIntensity;
        light.range = maxLightRange;
        toggleFlashlight(isOn);
        SetBatteryText();

    }

    // Update is called once per frame
    void Update()
    {
        HandleLight();
    }
    void LateUpdate(){
        
    }

    // light manager
    public void HandleLight(){
        SetFlashlight();

        if(Input.GetKeyUp("r") && battery > 0 && !isOn){
            isOn = true;
            toggleFlashlight(isOn);
        }
        else if (Input.GetKeyUp("r") && isOn){
            isOn = false;
            toggleFlashlight(isOn);
        }
        else if (battery == 0){
            batteryTextcolor = Color.red;
            isOn = false;
            toggleFlashlight(isOn);
        }
        if (Input.GetKeyUp("r") && battery == 0){
            Debug.Log("tryflash");
            StartCoroutine(BatteryFlash(3f));

        }
    }

    public float LightPower(){
        if (battery > 70){
            return 1f;
        }
        else if (battery > 40 && battery <= 70){
            return 0.6f;
        }
        else if (battery > 0 && battery <= 40){
            return 0.4f;
        }
        else{
            return 0;
        }
    }

    public void toggleFlashlight(bool isOn){
        spotLight.SetActive(isOn);
    }

    public void SetFlashlight(){
        light.intensity = maxLightIntensity * LightPower();
        light.range = maxLightRange * LightPower();
    }
    // lowers the battery power over time
    public void DecrementBattery(){
        if (battery > 0 && isOn){
            battery -= 1;
            SetBatteryText();
        }
    }

    public void SetBatteryText(){
        if(battery > 70){
            batteryTextcolor = Color.green;
        }
        else if (battery > 40 && battery <= 70){
            batteryTextcolor = Color.blue;
        }
        else if (battery > 0 && battery <= 40){
            batteryTextcolor = Color.yellow;
        }
        else{
            batteryTextcolor = Color.red;
        }
        batteryText.color = batteryTextcolor;
        batteryText.text = "Battery: " + battery + "%";
    }

    public IEnumerator BatteryFlash(float time){
        float maxtime = time;
        bool shouldBlink = true;
        batteryText.text = "No more battery!!!";
        while(shouldBlink){
            batteryText.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            batteryText.color = batteryTextcolor;
            yield return new WaitForSeconds(0.2f);
            if (maxtime != 0){
                maxtime -= 1;
            }
            else{
                shouldBlink = false;
            }
        }
    }
}
