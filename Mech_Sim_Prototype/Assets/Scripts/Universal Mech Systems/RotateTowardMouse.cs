using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardMouse : MonoBehaviour {
    private Transform gameObjectTransform;
    private Quaternion transformRotation;
    private Vector3 mouseLoc;
    private Vector3 directionToRotateToward;

    private float degreesToMouse;

    // rotation book-keeping
    private Quaternion degreesToMouseToQuaternion;

    // Use this for initialization
    void Start () {
        gameObjectTransform = GetComponent<Transform>();
        transformRotation = gameObjectTransform.rotation;
        mouseLoc = Input.mousePosition;
    }

    // rotation speed in degrees
    public float getAngleToRotateTowards(float rotationSpeed) {
        mouseLoc = Input.mousePosition;
        transformRotation = gameObjectTransform.rotation;
        directionToRotateToward = Camera.main.ScreenToWorldPoint(mouseLoc) - gameObjectTransform.position;
        degreesToMouse = Mathf.Atan2(directionToRotateToward.y, directionToRotateToward.x) * Mathf.Rad2Deg;
        degreesToMouseToQuaternion = Quaternion.Euler(0f, 0f, degreesToMouse);
        return Quaternion.RotateTowards(transformRotation, degreesToMouseToQuaternion, rotationSpeed).eulerAngles.z;
    }
}
