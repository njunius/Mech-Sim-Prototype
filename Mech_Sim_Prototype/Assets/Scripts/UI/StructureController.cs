using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureController : MonoBehaviour {
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
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
