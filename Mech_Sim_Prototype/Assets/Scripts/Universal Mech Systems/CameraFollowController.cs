using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowController : MonoBehaviour {

    public Transform playerLocation; // read only
    public Transform crosshairLocation; // read only
    private Transform currentFollowTransform;
    private Transform oldFollowTransform;
    private bool movingCamera;

    private float timeMoveStarted;
    private float timeMoving;
    private float timeToMove;
    private float percentMoveComplete;
    Vector3 movingPosition;

	// Use this for initialization
	void Start () {
        currentFollowTransform = playerLocation;
        movingCamera = false;
        timeToMove = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (!movingCamera) {
            transform.position = new Vector3(currentFollowTransform.position.x, currentFollowTransform.position.y, transform.position.z);
        }

    }

    void FixedUpdate() {
        if (movingCamera) {
            timeMoving = Time.time - timeMoveStarted;
            percentMoveComplete = timeMoving / timeToMove;
            movingPosition = new Vector3(currentFollowTransform.position.x, currentFollowTransform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(oldFollowTransform.position, movingPosition, percentMoveComplete);

            if(percentMoveComplete >= 1.0f) {
                movingCamera = false;
            }
        }
    }

    public void switchCameraFocus() {
        if (!movingCamera) {
            oldFollowTransform = transform.transform;
            if (currentFollowTransform == playerLocation) {
                currentFollowTransform = crosshairLocation;
                movingCamera = true;
            }
            else {
                currentFollowTransform = playerLocation;
                movingCamera = true;
            }

            timeMoveStarted = Time.time;
        }
    }
}
