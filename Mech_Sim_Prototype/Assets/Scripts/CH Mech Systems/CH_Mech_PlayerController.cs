using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_PlayerController : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public Transform torsoTransform;
    private Quaternion torsoRotation;
    private Vector3 mouseLoc;
    private Vector3 directionToRotateToward;

    private float forwardDirectionOffset;
    public float degreesToMouse;

    // rotation speed in degrees
    private float torsoRotSpeed;
    private float legsRotSpeed;

    // torso rotation book-keeping
    private Quaternion degreesToMouseToQuaternion;
    private float rotationIncrement;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        torsoRotation = torsoTransform.rotation;
        mouseLoc = Input.mousePosition;
        forwardDirectionOffset = 90.0f;
        torsoRotSpeed = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {

        
        
    }

    void FixedUpdate() {
        // torso rotation code
        mouseLoc = Input.mousePosition;
        torsoRotation = torsoTransform.rotation;
        directionToRotateToward = Camera.main.ScreenToWorldPoint(mouseLoc) - torsoTransform.position;
        degreesToMouse = Mathf.Atan2(directionToRotateToward.y, directionToRotateToward.x) * Mathf.Rad2Deg - forwardDirectionOffset;
        degreesToMouseToQuaternion = Quaternion.Euler(0f, 0f, degreesToMouse);
        rotationIncrement = Quaternion.RotateTowards(torsoRotation, degreesToMouseToQuaternion, torsoRotSpeed).eulerAngles.z;
        rigidBody.MoveRotation(rotationIncrement);
    }
}
