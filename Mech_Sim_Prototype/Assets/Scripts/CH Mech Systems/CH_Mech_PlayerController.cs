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
    private float accelerationDirection;

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
        accelerationRate = 0.35f;
	}
	
	// Update is called once per frame
	void Update () {

        legsRotation = legsRigidBody.rotation;
        torsoRotation = torsoRigidBody.rotation;

        torsoRotPerpToMove = torsoRotation;

        roundedLegsRot = Mathf.Round(legsRotation);
        roundedTorsoRot = Mathf.Round(torsoRotation);

        angleBetweenTorsoAndLegs = Mathf.Abs(Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot));

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            //legsXYRot = Vector2.zero;

            if (currMoveSpeed > 0) {
                currMoveSpeed -= accelerationRate * 0.9f * Time.deltaTime;
            }
            else {
                currMoveSpeed += accelerationRate * 0.9f * Time.deltaTime;
            }
            currMoveSpeed = Mathf.Clamp(currMoveSpeed, maxRevSpeed, maxMoveSpeed);
        }

        if (Input.GetKey(KeyCode.W)) { // go forward relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            handleForward(accelerationDirection);
        }
        if (Input.GetKey(KeyCode.S)) { // go backward relative to the direction the legs are facing
            accelerationDirection = -1.0f;
            handleForward(accelerationDirection);
        }
        if (Input.GetKey(KeyCode.A)) { // go left relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            if (Input.GetKey(KeyCode.S))
                accelerationDirection = -1.0f;
            handleLeftRight(90f, accelerationDirection);
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            if (Input.GetKey(KeyCode.S))
                accelerationDirection = -1.0f;
            handleLeftRight(-90f, accelerationDirection);
        }
        legsXYRot.Normalize();

        currMoveSpeed = Mathf.Clamp(currMoveSpeed, maxRevSpeed, maxMoveSpeed);
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

    // sign value should only be 1 or -1
    private void handleForward(float legTransformSign) {
        if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            handleLegRotation(torsoTransform.rotation, legsTransform.right * legTransformSign);
        }
    }

    private void handleReverse(float legTransformSign) {
        if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0 || angleBetweenTorsoAndLegs == 90) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        //else if (angleBetweenTorsoAndLegs == 90) {
        //    currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        //}
        else {
            handleLegRotation(torsoTransform.rotation, legsTransform.right * legTransformSign);
        }
    }

    // expects 90 or -90 degrees
    private void handleLeftRight(float torsoRotPerp, float legTransformSign) {
        if (angleBetweenTorsoAndLegs == 90) {
            /*if (Input.GetKey(KeyCode.S)) {
                legsXYRot -= legsTransform.right;
                currMoveSpeed -= accelerationRate;
            }
            else {
                legsXYRot += legsTransform.right;
                currMoveSpeed += accelerationRate * Time.deltaTime;
            }*/
            legsXYRot += legsTransform.right;// * legTransformSign;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            torsoRotPerpToMove += torsoRotPerp;
            handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), legsTransform.right * legTransformSign);
        }
    }

}
