using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battery_pickup : MonoBehaviour
{
    public light_Script flashlight;
    // Start is called before the first frame update
    void Start()
    {
        flashlight = GameObject.FindGameObjectWithTag("flashlight").GetComponent<light_Script>();
    }

    void Pickup() {
        if(flashlight.battery + 20 > 100 && flashlight.battery < 100){
            flashlight.battery = 100;
        }
        else if (flashlight.battery < 100)
        {
            flashlight.battery += 20;
        }
        else
        {
            return;
        }
        flashlight.SetBatteryText();
        Destroy(gameObject);
    }
    public void OnMouseDown() {
        Pickup();
    }
}
