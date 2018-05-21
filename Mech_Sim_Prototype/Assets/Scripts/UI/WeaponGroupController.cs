using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponGroupController : MonoBehaviour {
    public int numGroups;
    public GameObject groupDisplay;
    private Image[] groupTracker;

	// Use this for initialization
	void Start () {
        groupTracker = new Image[numGroups];
        
        for (int i = 0; i < numGroups; ++i) {
            GameObject temp = (GameObject)Instantiate(groupDisplay, gameObject.transform.position, gameObject.transform.rotation);
            temp.transform.SetParent(gameObject.transform, false);
            groupTracker[i] = temp.GetComponent<Image>();

            Text tempText = temp.GetComponentInChildren<Text>();
            tempText.text = "" + (i + 1);
            if(i % 2 != 0) {
                groupTracker[i].gameObject.transform.Rotate(new Vector3(180, 0, 0));
                tempText.gameObject.transform.Rotate(new Vector3(180, 0, 0));
            }
        }	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
