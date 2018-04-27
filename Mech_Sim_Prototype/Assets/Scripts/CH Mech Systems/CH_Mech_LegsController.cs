using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_LegsController : MonoBehaviour {

    public Transform torsoTransform;
    private Transform legsTransform;
    private Quaternion legsRotation;
    private Rigidbody2D legsRigidBody;

    private float rotationIncrement;

	// Use this for initialization
	void Start () {
        legsTransform = GetComponent<Transform>();
        legsRigidBody = GetComponent<Rigidbody2D>();
        legsRotation = legsTransform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        legsTransform.position = torsoTransform.position;
	}

    // rotates legs towards torsoRotation at rate of rotationSpeed
    public void rotateLegs(Quaternion torsoRotation, float rotationSpeed) {
        legsRotation = legsTransform.rotation;
        rotationIncrement = Quaternion.RotateTowards(legsRotation, torsoRotation, rotationSpeed).eulerAngles.z;
        legsRigidBody.MoveRotation(rotationIncrement);
    }

    public float getRotationIncrement(Quaternion torsoRotation, float rotationSpeed) {
        return Quaternion.RotateTowards(legsRotation, torsoRotation, rotationSpeed).eulerAngles.z;
    }
}
