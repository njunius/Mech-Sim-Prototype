using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_PlayerController : MonoBehaviour {
    private Rigidbody2D rigidBody;
    public Transform torsoTransform;
    private Quaternion torsoRotation;
    private Vector3 mouseLoc;
    private Vector3 prevMouseLoc;
    private Vector3 directionToRotateToward;
    private Quaternion mouseRotationAngle;

    private float forwardDirectionOffset;

    // in degrees
    private float torsoRotSpeed;
    private float legsRotSpeed;

    public float degreesToMouse;
    

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        torsoTransform = GetComponent<Transform>();
        torsoRotation = torsoTransform.rotation;
        mouseLoc = Input.mousePosition;
        prevMouseLoc = mouseLoc;
        forwardDirectionOffset = 90.0f;
        torsoRotSpeed = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {

        
        
    }

    void FixedUpdate() {
        // torso rotation code
        mouseLoc = Input.mousePosition;
        // only update degreesToMouse when necessary
        if (prevMouseLoc != mouseLoc) {
            torsoRotation = torsoTransform.rotation;
            directionToRotateToward = Camera.main.ScreenToWorldPoint(mouseLoc) - torsoTransform.position;
            degreesToMouse = Mathf.Atan2(directionToRotateToward.y, directionToRotateToward.x) * Mathf.Rad2Deg - forwardDirectionOffset;
        }
        rigidBody.MoveRotation(Mathf.LerpAngle(rigidBody.rotation, degreesToMouse, torsoRotSpeed * Time.deltaTime));
        prevMouseLoc = mouseLoc;
    }
}
