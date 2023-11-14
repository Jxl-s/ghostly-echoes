using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHandObjectBehaviour : MonoBehaviour
{
    public GameObject HandPosition;

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = HandPosition.transform.localRotation.eulerAngles;
        Quaternion rotQuat = Quaternion.Euler(rot.x + 90, rot.y, rot.z);


        transform.localPosition = HandPosition.transform.localPosition;
        transform.localRotation = rotQuat;
    }
}
