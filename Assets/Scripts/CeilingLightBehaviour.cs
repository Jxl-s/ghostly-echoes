using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLightBehaviour : MonoBehaviour
{
    public bool canFlash = true;
    public float minFlashInterval = 4f;
    public float maxFlashInterval = 10f;
    public float minFlashDuration = 0.1f;
    public float maxFlashDuration = 1f;

    public Light lightComponent;
    private float delay;

    void Start()
    {
        // make a random delay
        delay = Random.Range(0f, maxFlashInterval);

        // Start the flashing coroutine
        StartCoroutine(FlashLight());
    }

    IEnumerator FlashLight()
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        if (canFlash){
            while (true)
            {
                // Turn on the light
                lightComponent.enabled = false;

                // Wait for the duration of the flash
                float randomDuration = Random.Range(minFlashDuration, maxFlashDuration);
                yield return new WaitForSeconds(randomDuration);

                // Turn off the light
                lightComponent.enabled = true;

                // Wait for a random interval before the next flash
                float randomInterval = Random.Range(minFlashInterval, maxFlashInterval);
                yield return new WaitForSeconds(randomInterval);
            }
        }
    }
}
