using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalManagerController : MonoBehaviour {

    public SpriteRenderer[] goalMoments;
    public SpriteRenderer exitGoal;
    public SpriteRenderer exitGoalMap;

    public int numGoalsReached;

	// Use this for initialization
	void Start () {
        numGoalsReached = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(numGoalsReached == goalMoments.Length && !exitGoal.enabled) {
            exitGoal.enabled = true;
            exitGoalMap.enabled = true;
        }
	}

    // should only be called by a GoalController when the player leaves its radius
    // increments numGoalsReached
    public void achieveGoal() {
        numGoalsReached++;
    }
}
