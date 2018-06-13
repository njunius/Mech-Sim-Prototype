using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalController : MonoBehaviour {
    public Text goalText;
    public SpriteRenderer goalMapView; 
    private SpriteRenderer goalWorldView;
    private CircleCollider2D goalTrigger;
    public GoalManagerController goalManager;
    public EndDemo goalFade;
    public string thoughtText;
    public Text dialogueBoxHUD;
    public Text dialogueBoxMap;

	// Use this for initialization
	void Start () {
        goalWorldView = GetComponent<SpriteRenderer>();
        goalTrigger = GetComponent<CircleCollider2D>();
        goalManager = GetComponentInParent<GoalManagerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("CH-Mech")) {
            if (this.gameObject.CompareTag("Goal-Exit")) {
                goalText.enabled = true;
                goalFade.startFade();
            }
            else {
                dialogueBoxHUD.text = thoughtText;
                dialogueBoxMap.text = thoughtText;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("CH-Mech")) {
            goalText.enabled = false;
            goalWorldView.enabled = false;
            goalMapView.enabled = false;
            goalTrigger.enabled = false;
            goalManager.achieveGoal();
        }
    }
}
