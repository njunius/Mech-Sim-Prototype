using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CH_Mech_PlayerController : MonoBehaviour {

    private Rigidbody2D rigidBody;
    public Transform torsoTransform;
    private float torsoRotation;
    private float roundedTorsoRot;
    // used for rotating to move left and right
    // only perpendicular when rotating to move left and right
    private float torsoRotPerpToMove;
    private float torsoForwardAngleAbs;
    private float torsoPerpAngleAbs;
    private float torsoBackAngleAbs;

    //private Vector3 mouseLoc;
    public GameObject legs;
    private Transform legsTransform;
    private CH_Mech_LegsController legsController;
    private float legsRotation;
    //private Rigidbody2D legsRigidBody;
    private float roundedLegsRot;
    private Vector3 legsXYRot;
    private Vector3 checkBackwards;
    private RotateTowardMouse rotationFunction;
    private float angleBetweenTorsoAndLegs;
    private float angleBetweenTorsoAndLegsAbs;

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
        rigidBody = GetComponent<Rigidbody2D>();
        torsoRotation = torsoTransform.rotation.eulerAngles.z;
        torsoRotPerpToMove = torsoRotation + 90f;

        torsoBackAngleAbs = 180f;
        torsoForwardAngleAbs = 0f;
        torsoPerpAngleAbs = 90f;

        legsTransform = legs.GetComponent<Transform>();
        legsController = legs.GetComponent<CH_Mech_LegsController>();
        legsRotation = legsTransform.rotation.eulerAngles.z;

        legsXYRot = Vector3.zero;
        legsRotSpeed = 2.5f;
        maxMoveSpeed = 1.4f;
        maxRevSpeed = -1 * maxMoveSpeed;
        currMoveSpeed = 0.0f;
        accelerationRate = 3 * maxMoveSpeed;

        torsoRotationArrowKeyIncrement = 0f;
        torsoRotationAmount = 4.5f;

        crosshair = GetComponentInChildren<CH_Mech_CrosshairController>();
	}
	
	// Update is called once per frame
	void Update () {
        // start movement and rotation input handling code
        legsRotation = legsTransform.rotation.eulerAngles.z;
        torsoRotation = torsoTransform.rotation.eulerAngles.z;
        torsoRotPerpToMove = torsoTransform.rotation.eulerAngles.z;

        roundedLegsRot = Mathf.Round(legsRotation);
        roundedTorsoRot = Mathf.Round(torsoRotation);

        angleBetweenTorsoAndLegs = Mathf.DeltaAngle(roundedTorsoRot, roundedLegsRot);
        angleBetweenTorsoAndLegsAbs = Mathf.Abs(angleBetweenTorsoAndLegs);

        // decelerates when movement keys are not  pressed
        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {

            if (currMoveSpeed > 0) {
                currMoveSpeed -= accelerationRate * 0.99f * Time.deltaTime;
                currMoveSpeed = Mathf.Max(currMoveSpeed, 0f);
            }
            else {
                currMoveSpeed += accelerationRate * 0.99f * Time.deltaTime;
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
            if (angleBetweenTorsoAndLegs < 0f || Input.GetKey(KeyCode.S)) {
                accelerationDirection = -1.0f;
            }
            handleLeftRight(torsoPerpAngleAbs, accelerationDirection);
        }
        if (Input.GetKey(KeyCode.D)) { // go right relative to the direction the torso is facing
            accelerationDirection = 1.0f;
            if (angleBetweenTorsoAndLegs > 0f || Input.GetKey(KeyCode.S)) {
                accelerationDirection = -1.0f;
            }
            handleLeftRight(-torsoPerpAngleAbs, accelerationDirection);
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

        // Due to update loop ordering the legs and torso may be off by as much as 6 degrees
        // to get the clamping behavior to work, check the range of each of the torso/legs locked angles
        if(
            Mathf.Clamp(angleBetweenTorsoAndLegsAbs, torsoForwardAngleAbs, torsoForwardAngleAbs + 5) == angleBetweenTorsoAndLegsAbs || 
            Mathf.Clamp(angleBetweenTorsoAndLegsAbs, torsoPerpAngleAbs - 3, torsoPerpAngleAbs + 3) == angleBetweenTorsoAndLegsAbs || 
            Mathf.Clamp(angleBetweenTorsoAndLegsAbs, torsoBackAngleAbs - 3, torsoBackAngleAbs + 3) == angleBetweenTorsoAndLegsAbs
            ) {
            torsoRotationArrowKeyIncrement = Mathf.Clamp(torsoRotationArrowKeyIncrement, -legsRotSpeed, legsRotSpeed);

        }
        else {
            torsoRotationArrowKeyIncrement = Mathf.Clamp(torsoRotationArrowKeyIncrement, -torsoRotationAmount, torsoRotationAmount);
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
        rigidBody.velocity = legsXYRot * currMoveSpeed;

        // torso rotation code
        torsoTransform.Rotate(Vector3.forward, torsoRotationArrowKeyIncrement);
        torsoRotationArrowKeyIncrement = 0f;

    }

    // rotates legs towards the torsoRotation and maintains current velocity
    private void handleLegRotation(Quaternion torsoRotation, Vector3 directionVelocity) {
        legsController.rotateLegs(torsoRotation, legsRotSpeed);
        if (legsXYRot != Vector3.zero) {
            legsXYRot += directionVelocity;
        }
    }

    // sign value should only be 1 or -1
    // determines whether or not to rotate legs based on the current angle between the torso and legs
    private void handleForwardReverse(float legTransformSign) {
        if (angleBetweenTorsoAndLegsAbs == torsoForwardAngleAbs || angleBetweenTorsoAndLegsAbs == torsoBackAngleAbs) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            handleLegRotation(torsoTransform.rotation, legsTransform.right);
        }
    }

    // torsoRotPerp expects 90 or -90 degrees
    // legTransformSign expects 1 or -1
    // determines whether or not to rotate legs based on the current angle between the torso and legs
    private void handleLeftRight(float torsoRotPerp, float legTransformSign) {
        if (angleBetweenTorsoAndLegsAbs == torsoPerpAngleAbs) {
            legsXYRot += legsTransform.right;
            currMoveSpeed += accelerationRate * legTransformSign * Time.deltaTime;
        }
        else {
            torsoRotPerpToMove += torsoRotPerp;
            handleLegRotation(Quaternion.Euler(torsoTransform.rotation.eulerAngles.x, torsoTransform.rotation.eulerAngles.y, torsoRotPerpToMove), legsTransform.right);
        }
    }

}
