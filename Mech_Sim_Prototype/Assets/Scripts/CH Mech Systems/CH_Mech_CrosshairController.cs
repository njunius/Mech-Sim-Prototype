using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_Mech_CrosshairController : MonoBehaviour {

    private Transform torsoTransform;

    private float minDistance;
    private float maxDistance;
    private float currentDistance;
    private float distanceDeltaIncrement;

    // Use this for initialization
    void Start () {
        torsoTransform = GetComponentInParent<Transform>();
        minDistance = 1.0f;
        maxDistance = 10.0f;
        currentDistance = 7.0f;
        distanceDeltaIncrement = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {

    }

    // signValue should be -1 or 1
    public void updateDistance(float signValue) {
        currentDistance += distanceDeltaIncrement * signValue * Time.deltaTime;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        transform.localPosition = new Vector3(currentDistance, transform.localPosition.y, transform.localPosition.z);
    }
}
