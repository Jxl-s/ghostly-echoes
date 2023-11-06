using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class light_Script : MonoBehaviour
{
    public float maxLightIntensity = 20;
    public float maxLightRange = 10;
    public float drainSpeed = 1f;
    public bool isOn = false;
    public GameObject spotLight;
    public Light light;
    public Text batteryText;
    public HUDManager hud;

    // Start is called before the first frame update
    void Start()
    {
        hud = HUDManager.Instance;

        batteryText = GameObject.FindGameObjectWithTag("batteryText").GetComponent<Text>();
        spotLight = GameObject.FindGameObjectWithTag("flashlight_light");
        light = spotLight.GetComponent<Light>();
        InvokeRepeating("DecrementBattery", 1.0f, drainSpeed);
        light.intensity = maxLightIntensity;
        light.range = maxLightRange;
        toggleFlashlight(isOn);
        // SetBatteryText();

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

        if(Input.GetKeyUp("r") && GameManager.Instance.BatteryPercentage > 0 && !isOn){
            isOn = true;
            toggleFlashlight(isOn);
        }
        else if (Input.GetKeyUp("r") && isOn){
            isOn = false;
            toggleFlashlight(isOn);
        }
        else if (GameManager.Instance.BatteryPercentage == 0){

            isOn = false;
            toggleFlashlight(isOn);
        }
        if (Input.GetKeyUp("r") && GameManager.Instance.BatteryPercentage == 0){
            Debug.Log("tryflash");
            // hud.Blink();

        }
    }

    public float LightPower(){
        if (GameManager.Instance.BatteryPercentage > 70){
            return 1f;
        }
        else if (GameManager.Instance.BatteryPercentage > 40 && GameManager.Instance.BatteryPercentage <= 70){
            return 0.6f;
        }
        else if (GameManager.Instance.BatteryPercentage > 0 && GameManager.Instance.BatteryPercentage <= 40){
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
    // lowers the GameManager.Instance.BatteryPercentage power over time
    public void DecrementBattery(){
        if (GameManager.Instance.BatteryPercentage > 0 && isOn){
            // GameManager.Instance.BatteryPercentage -= 1;
            hud.DecrementBatteryPercentage(1f);
        }

    }



    // public IEnumerator BatteryFlash(float time){
    //     float maxtime = time;
    //     bool shouldBlink = true;
    //     // batteryText.text = "No more battery!!!";
    //     hud.UpdateBatteryBar(100f);
    //     while(shouldBlink){
    //         batteryText.color = Color.white;
    //         hud.SetBatteryColor(Color.white);
    //         yield return new WaitForSeconds(0.2f);
    //         hud.SetBatteryColor(batteryTextcolor);
    //         batteryText.color = batteryTextcolor;
    //         yield return new WaitForSeconds(0.2f);
    //         if (maxtime != 0){
    //             maxtime -= 1;
    //         }
    //         else{
    //             shouldBlink = false;
    //         }
    //     }
    //     hud.UpdateBatteryBar(GameManager.Instance.BatteryPercentage);
    // }
}
