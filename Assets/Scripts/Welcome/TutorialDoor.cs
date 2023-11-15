using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) {
            return;
        }

        // Load the next level
        StartCoroutine(HUDManager.Instance.UpdateMask(5f, 5f));

        Debug.Log("next level...");
    }
}
