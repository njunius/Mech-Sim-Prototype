using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_LegsController : MonoBehaviour {

    public Transform torsoTransform;
    private Rigidbody2D torsoRigidBody;

    private Transform legsTransform;
    private Quaternion legsRotation;
    private Rigidbody2D legsRigidBody;

    private float rotationIncrement;

	// Use this for initialization
	void Start () {
        legsTransform = GetComponent<Transform>();
        legsRigidBody = GetComponent<Rigidbody2D>();
        legsRotation = legsTransform.rotation;

        torsoRigidBody = torsoTransform.gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        legsTransform.position = torsoTransform.position;        
	}

    // rotates legs towards torsoRotation at rate of rotationSpeed
    public void rotateLegs(Quaternion torsoRotation, float rotationSpeed) {
        legsRotation = legsTransform.rotation;

        if(Mathf.Abs(Mathf.DeltaAngle(legsRotation.eulerAngles.z, torsoRotation.eulerAngles.z)) > 90.0f) {
            //torsoRotation = Quaternion.Euler(torsoRotation.x, torsoRotation.y, torsoRotation.z + 180f);
        }
        rotationIncrement = Quaternion.RotateTowards(legsRotation, torsoRotation, rotationSpeed).eulerAngles.z;
        legsRigidBody.MoveRotation(rotationIncrement);
    }

    public float getRotationIncrement() {
        return rotationIncrement;
    }
}
