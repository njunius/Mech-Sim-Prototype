using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float angleBetweenTorsoAndLegsAbs;

    // rotation speed in degrees
    private float torsoRotSpeed;
    private float legsRotSpeed;

    // movement book-keeping
    private float maxMoveSpeed;
    private float maxRevSpeed;
    private float currMoveSpeed;
    private float accelerationRate;
    private float accelerationDirection;

    // rotation book-keeping
    private float torsoRotationIncrement;
    private float legsRotationIncrement;

    // arrow key rotation increment in degrees
    private float torsoRotationArrowKeyIncrement;
    private float torsoRotationAmount;

    // arrow key crosshair distance
    private CH_Mech_CrosshairController crosshair;

    // for camera toggling
    public CameraFollowController mainCamera;

    // for map/HUD switching
    public Canvas HUD;
    public Canvas map;

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
        maxRevSpeed = -1 * maxMoveSpeed;
        currMoveSpeed = 0.0f;
        accelerationRate = 2 * maxMoveSpeed;

        torsoRotationArrowKeyIncrement = 0f;
        torsoRotationAmount = 0.25f;

        crosshair = GetComponentInChildren<CH_Mech_CrosshairController>();
	}
	
	// Update is called once per frame
	void Update () {
        // start movement and rotation input handling code
        legsRotation = legsRigidBody.rotation;
        torsoRotation = torsoRigidBody.rotation;

        torsoRotPerpToMove = torsoRotation;

        roundedLegsRot = Mathf.Round(legsRotation);
        roundedTorsoRot = Mathf.Round(torsoRotation);

        angleBetweenTorsoAndLegs = Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot);
        angleBetweenTorsoAndLegsAbs = Mathf.Abs(angleBetweenTorsoAndLegs);

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {

            if (currMoveSpeed > 0) {
                currMoveSpeed -= accelerationRate * 0.9f * Time.deltaTime;
                currMoveSpeed = Mathf.Max(currMoveSpeed, 0f);
            }
            else {
                currMoveSpeed += accelerationRate * 0.9f * Time.deltaTime;
                currMoveSpeed = Mathf.Min(currMoveSpeed, 0f);
            }
        }

        if (Input.GetKey(KeyCode.W)) { // go forward relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            handleForwardReverse(accelerationDirection);
        }
        if (Input.GetKey(KeyCode.S)) { // go backward relative to the direction the legs are facing
            accelerationDirection = -1.0f;
            handleForwardReverse(accelerationDirection);
        }
        if (Input.GetKey(KeyCode.A)) { // go left relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            if (angleBetweenTorsoAndLegs < 0f) {
                accelerationDirection = -1.0f;
            }
            handleLeftRight(90f, accelerationDirection);
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            if (angleBetweenTorsoAndLegs > 0f) {
                accelerationDirection = -1.0f;
            }
            handleLeftRight(-90f, accelerationDirection);
        }
        legsXYRot.Normalize();

        currMoveSpeed = Mathf.Clamp(currMoveSpeed, maxRevSpeed, maxMoveSpeed);
        // end movement and rotation handling code

        // start camera focus change code
        if (Input.GetKey(KeyCode.Q)) {
            mainCamera.switchCameraFocus();
        }
        // end camera focus change code

        // start arrow keys rotation code
        if (Input.GetKey(KeyCode.LeftArrow)) {
            torsoRotationArrowKeyIncrement += torsoRotationAmount;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            torsoRotationArrowKeyIncrement -= torsoRotationAmount;
        }
        // end arrow keys rotation code

        // start arrow keys crosshair distance code
        if (Input.GetKey(KeyCode.UpArrow)) {
            crosshair.updateDistance(1.0f);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            crosshair.updateDistance(-1.0f);
        }
        // end arrow keys crosshair distance code

        // start map/HUD toggle code
        if (Input.GetKeyDown(KeyCode.R)) {
            if (map.enabled) {
                map.enabled = false;
                HUD.enabled = true;
            }
            else if (HUD.enabled) {
                map.enabled = true;
                HUD.enabled = false;
            }
        }
        // end map/HUD toggle code
    }

    void FixedUpdate() {
        // movement code
        torsoRigidBody.velocity = legsXYRot * currMoveSpeed;

        // torso rotation code
        /*if (legsXYRot == Vector3.zero) {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(torsoRotSpeed);
        }
        else {
            torsoRotationIncrement = rotationFunction.getAngleToRotateTowards(legsRotSpeed);
        }
        torsoRigidBody.MoveRotation(torsoRotationIncrement);
        */
        torsoRigidBody.MoveRotation(torsoRigidBody.rotation + torsoRotationArrowKeyIncrement);
        torsoRotationArrowKeyIncrement = 0f;
    }

    private void handleLegRotation(Quaternion torsoRotation, Vector3 directionVelocity) {
        legsController.rotateLegs(torsoRotation, legsRotSpeed);
        if (legsXYRot != Vector3.zero) {
            legsXYRot += directionVelocity;
        }
    }

    // sign value should only be 1 or -1
    private void handleForwardReverse(float legTransformSign) {
        if (angleBetweenTorsoAndLegsAbs == 180 || angleBetweenTorsoAndLegsAbs == 0) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            handleLegRotation(torsoTransform.rotation, legsTransform.right);
        }
    }

    // expects 90 or -90 degrees
    private void handleLeftRight(float torsoRotPerp, float legTransformSign) {
        if (angleBetweenTorsoAndLegsAbs == 90) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            torsoRotPerpToMove += torsoRotPerp;
            handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), legsTransform.right);
        }
    }

}
