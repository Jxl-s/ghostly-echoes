using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }

    float sensitivity = 1f;
    public float smoothTurnRate = 0.15f; //0 - 1 || 1 -> instant
    // since this is camera movement, -> higher than 0.3 is already not noticable

    float yRotationLimit = 60f;
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    public GameObject InHandObject;

    void Update()
    {
        if (GameManager.Instance.ControlsEnabled == false) {
            return;
        }

        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);
        

        transform.parent.transform.localRotation = Quaternion.Slerp(transform.parent.transform.localRotation, xQuat, smoothTurnRate);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, yQuat, smoothTurnRate);

        Vector3 rot = transform.localRotation.eulerAngles;
        Quaternion rotQuat = Quaternion.Euler(rot.x + 90, rot.y, rot.z);

        InHandObject.transform.localRotation = rotQuat;
    }
}
