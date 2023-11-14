using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Make it go UP and down with sin, make it rotate on itself
        transform.position = initialPosition + new Vector3(0, Mathf.Sin(Time.time) * 0.5f, 0);
        transform.Rotate(new Vector3(1, 0, 0), Time.deltaTime * 50f);
    }
}
