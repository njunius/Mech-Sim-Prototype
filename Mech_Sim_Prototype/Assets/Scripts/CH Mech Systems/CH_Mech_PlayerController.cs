using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_PlayerController : MonoBehaviour {
    private Rigidbody2D torsoRigidBody;
    public Transform torsoTransform;
    private float torsoRotation;
    private Vector3 torsoXYRot;
    private float roundedTorsoRot;

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

        roundedLegsRot = Mathf.Round(legsRotation);
        roundedTorsoRot = Mathf.Round(torsoRotation);

        angleBetweenTorsoAndLegs = Mathf.Abs(Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot));

        torsoXYRot = Vector3.zero;
        Debug.Log("Legs: " + legsRotation);
        Debug.Log("torso: " + torsoRotation);
        Debug.Log("Delta Angle: " + Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot));
        if (Input.GetKey(KeyCode.W)) { // go forward relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                torsoXYRot += torsoTransform.right;
            }
            else {
                legsController.rotateLegs(torsoTransform.rotation, legsRotSpeed);
            }
        }
        if (Input.GetKey(KeyCode.S)) { // go backward relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 180 || angleBetweenTorsoAndLegs == 0) {
                torsoXYRot -= torsoTransform.right;
            }
            else {
                legsController.rotateLegs(torsoTransform.rotation, legsRotSpeed);
            }
        }
        if (Input.GetKey(KeyCode.A)) { // go left relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                torsoXYRot += torsoTransform.up;
            }
            else {
                legsController.rotateLegs(torsoTransform.rotation, legsRotSpeed);
            }
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            if (angleBetweenTorsoAndLegs == 90) {
                torsoXYRot -= torsoTransform.up;
            }
            else {
                legsController.rotateLegs(torsoTransform.rotation, legsRotSpeed);
            }
        }
        torsoXYRot.Normalize();

    }

    void FixedUpdate() {
        // torso rotation code
        torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(torsoRotSpeed);
        torsoRigidBody.MoveRotation(torsoRotationIncrement);

        // movement code
        torsoRigidBody.velocity = torsoXYRot * maxMoveSpeed;
    }

    // finds rotation perpendicular to given rotation in the direction indicated by sign
    // intersectingAngle must be in degrees between -360 and 360
    private Quaternion getIntersectingAngle(Quaternion originalAngle, float intersectingAngle) {
        float angle2D = originalAngle.eulerAngles.z;
        angle2D += intersectingAngle;
        return Quaternion.Euler(new Vector3(0, 0, angle2D));

    }
}
