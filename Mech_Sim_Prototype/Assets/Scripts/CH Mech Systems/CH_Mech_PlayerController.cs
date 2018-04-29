using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_PlayerController : MonoBehaviour {
    private Rigidbody2D torsoRigidBody;
    public Transform torsoTransform;
    private float torsoRotation;
    private Vector3 torsoXYRot;
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

    private RotateTowardMouse rotationFunction;
    private float angleBetweenTorsoAndLegs;

    // rotation speed in degrees
    private float torsoRotSpeed;
    private float legsRotSpeed;

    private float maxMoveSpeed;
    private float currMoveSpeed;
    private float accelerationRate;

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
        torsoXYRot = Vector3.zero;
        //mouseLoc = Input.mousePosition;
        torsoRotSpeed = 2.0f;
        legsRotSpeed = 1.5f;
        maxMoveSpeed = 0.7f;
        currMoveSpeed = 0.0f;
        accelerationRate = 0.05f;
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
            torsoXYRot = Vector3.zero;
        }
            
        if (Input.GetKey(KeyCode.W)) { // go forward relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                torsoXYRot += torsoTransform.right;
            }
            else {
                handleLegRotation(torsoTransform.rotation, torsoTransform.right);
            }
        }
        if (Input.GetKey(KeyCode.S)) { // go backward relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                torsoXYRot -= torsoTransform.right;
            }
            else {
                handleLegRotation(torsoTransform.rotation, -torsoTransform.right);
            }
        }
        if (Input.GetKey(KeyCode.A)) { // go left relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                torsoXYRot += torsoTransform.up;
            }
            else {
                torsoRotPerpToMove += 90f;
                handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), torsoTransform.up);
            }
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                torsoXYRot -= torsoTransform.up;
            }
            else {
                torsoRotPerpToMove -= 90f;
                handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), -torsoTransform.up);
            }
        }
        torsoXYRot.Normalize();

    }

    void FixedUpdate() {
        // movement code
        torsoRigidBody.velocity = torsoXYRot * maxMoveSpeed;

        // torso rotation code
        if (torsoXYRot == Vector3.zero) {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(torsoRotSpeed);
        }
        else {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(legsRotSpeed);
        }
        torsoRigidBody.MoveRotation(torsoRotationIncrement);

    }

    private void handleLegRotation(Quaternion torsoRotation, Vector3 directionVelocity) {
        legsController.rotateLegs(torsoRotation, legsRotSpeed);
        if (torsoXYRot != Vector3.zero) {
            torsoXYRot += directionVelocity;
        }
    }

}
