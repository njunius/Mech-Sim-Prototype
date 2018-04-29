using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_PlayerController : MonoBehaviour {
    private Rigidbody2D torsoRigidBody;
    public Transform torsoTransform;
    private float torsoRotation;
    private float roundedTorsoRot;
    // used for rotating to move left and right
    // only perpendicular when rotating to move left and right
    private float torsoRotPerpToMove;

    //private Vector3 mouseLoc;
    public GameObject legs;
    private Transform legsTransform;
    private CH_Mech_LegsController legsController;
    private float legsRotation;
    private Rigidbody2D legsRigidBody;
    private float roundedLegsRot;
    private Vector3 legsXYRot;
    private Vector3 checkBackwards;
    private RotateTowardMouse rotationFunction;
    private float angleBetweenTorsoAndLegs;

    // rotation speed in degrees
    private float torsoRotSpeed;
    private float legsRotSpeed;

    private float maxMoveSpeed;
    private float maxRevSpeed;
    private float currMoveSpeed;
    private float accelerationRate;
    private bool moving;

    // rotation book-keeping
    private float torsoRotationIncrement;
    private float legsRotationIncrement;

    // Use this for initialization
    void Start () {
        torsoRigidBody = GetComponent<Rigidbody2D>();
        torsoTransform = GetComponent<Transform>();
        torsoRotation = torsoRigidBody.rotation;

        rotationFunction = GetComponent<RotateTowardMouse>();

        legsTransform = legs.GetComponent<Transform>();
        legsController = legs.GetComponent<CH_Mech_LegsController>();
        legsRigidBody = legs.GetComponent<Rigidbody2D>();
        legsRotation = legsRigidBody.rotation;

        torsoRotPerpToMove = torsoRotation + 90f;
        legsXYRot = Vector3.zero;
        //mouseLoc = Input.mousePosition;
        torsoRotSpeed = 2.0f;
        legsRotSpeed = 1.5f;
        maxMoveSpeed = 0.7f;
        maxRevSpeed = -0.7f;
        currMoveSpeed = 0.0f;
        accelerationRate = 0.004f;
        moving = false;
	}
	
	// Update is called once per frame
	void Update () {

        legsRotation = legsRigidBody.rotation;
        torsoRotation = torsoRigidBody.rotation;

        torsoRotPerpToMove = torsoRotation;

        roundedLegsRot = Mathf.Round(legsRotation);
        roundedTorsoRot = Mathf.Round(torsoRotation);

        angleBetweenTorsoAndLegs = Mathf.Abs(Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot));

        Debug.Log(torsoRigidBody.velocity);

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            //legsXYRot = Vector2.zero;

            if (currMoveSpeed > 0) {
                currMoveSpeed -= accelerationRate;
                currMoveSpeed = Mathf.Max(0f, currMoveSpeed);
            }
            //else {
            //    currMoveSpeed += accelerationRate;
            //    currMoveSpeed = Mathf.Min(0f, currMoveSpeed);
            //}
            
        }

        if (Input.GetKey(KeyCode.W)) { // go forward relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                legsXYRot += legsTransform.right;
                currMoveSpeed += accelerationRate;
            }
            else {
                handleLegRotation(torsoTransform.rotation, legsTransform.right);
            }
        }
        if (Input.GetKey(KeyCode.S)) { // go backward relative to the direction the legs are facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                legsXYRot -= legsTransform.right;
                //moving = true;
                //if (currMoveSpeed > 0 && torsoRigidBody.velocity != Vector2.zero)
                //    currMoveSpeed -= accelerationRate;
                //else
                currMoveSpeed += accelerationRate;
            }
            else {
                handleLegRotation(torsoTransform.rotation, -legsTransform.right);
            }
        }
        legsXYRot.Normalize();
        if (Input.GetKey(KeyCode.A)) { // go left relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                if (Input.GetKey(KeyCode.S)) {
                    legsXYRot -= legsTransform.right;
                    //currMoveSpeed -= accelerationRate;
                }
                else {
                    legsXYRot += legsTransform.right;
                    currMoveSpeed += accelerationRate;
                }
                //legsXYRot += legsTransform.right;
            }
            else {
                torsoRotPerpToMove += 90f;
                checkBackwards = legsTransform.right;
                if (Input.GetKey(KeyCode.S)) {
                    checkBackwards = -legsTransform.right;
                }
                handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), checkBackwards);
            }
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                if (Input.GetKey(KeyCode.S)) {
                    legsXYRot -= legsTransform.right;
                    //currMoveSpeed -= accelerationRate;
                }
                else {
                    legsXYRot += legsTransform.right;
                    currMoveSpeed += accelerationRate;
                }
                //legsXYRot += legsTransform.right;
            }
            else {
                torsoRotPerpToMove -= 90f;
                checkBackwards = legsTransform.right;
                if (Input.GetKey(KeyCode.S)) {
                    checkBackwards = -legsTransform.right;
                }
                handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), checkBackwards);
            }
        }
        legsXYRot.Normalize();

        //if (currMoveSpeed < 0) {
            //currMoveSpeed = Mathf.Max(maxRevSpeed, currMoveSpeed);
        //}
        //else {
            currMoveSpeed = Mathf.Min(maxMoveSpeed, currMoveSpeed);
        //}
    }

    void FixedUpdate() {
        // movement code
        Debug.Log("currMoveSpeed: " + currMoveSpeed);
        torsoRigidBody.velocity = legsXYRot * currMoveSpeed;

        // torso rotation code
        if (legsXYRot == Vector3.zero) {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(torsoRotSpeed);
        }
        else {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(legsRotSpeed);
        }
        torsoRigidBody.MoveRotation(torsoRotationIncrement);

    }

    private void handleLegRotation(Quaternion torsoRotation, Vector3 directionVelocity) {
        legsController.rotateLegs(torsoRotation, legsRotSpeed);
        if (legsXYRot != Vector3.zero) {
            legsXYRot += directionVelocity;
        }
    }

}
