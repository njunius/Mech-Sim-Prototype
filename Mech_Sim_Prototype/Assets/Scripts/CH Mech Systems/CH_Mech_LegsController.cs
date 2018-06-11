using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_LegsController : MonoBehaviour {

    public Transform torsoTransform;

    private Transform legsTransform;
    private Quaternion torsoRotationCached;
    private float rotationSpeedCached;
    private float rotationIncrement;

	// Use this for initialization
	void Start () {
        legsTransform = GetComponent<Transform>();
	}

    void FixedUpdate() {
        legsTransform.rotation = Quaternion.RotateTowards(legsTransform.rotation, torsoRotationCached, rotationSpeedCached);
        rotationSpeedCached = 0;
        torsoRotationCached = legsTransform.rotation;

    }

    // caches passed in direction and speed for rotation towards during the physics update
    public void rotateLegs(Quaternion torsoRotation, float rotationSpeed) {
        torsoRotationCached = torsoRotation;
        rotationSpeedCached = rotationSpeed;

    }

}
