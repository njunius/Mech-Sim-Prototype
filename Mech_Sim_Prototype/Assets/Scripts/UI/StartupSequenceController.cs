using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupSequenceController : MonoBehaviour {

    public Image currentDisplayImage;
    public Sprite[] noiseAnim;
    private Canvas startupCanvas;
    public Canvas HUD;
    public Text sequence;
    private CanvasRenderer sequenceRenderer;
    //private Dictionary<KeyCode, string> startupPrep;
    private string[] startupPrep;
    private string checkPassed;
    private int checkStep;
    private int lastCheckStep;

    private bool passedCheck;

    private int delayCounter;
    private int toggleImage;

    private int numStartupButtons;
    private bool[] keysPressed;

    private bool startupSequence;

	// Use this for initialization
	void Start () {
        startupCanvas = GetComponent<Canvas>();
        delayCounter = 0;
        toggleImage = 0;

        numStartupButtons = 0;
        keysPressed = new bool[12];
        for(int i = 0; i < keysPressed.Length; ++i) {
            keysPressed[i] = false;
        }

        startupSequence = true;

        sequenceRenderer = sequence.gameObject.GetComponent<CanvasRenderer>();
        /*startupPrep = new Dictionary<KeyCode, string>();
        startupPrep.Add(KeyCode.R, "Press R to Enable HUD");
        startupPrep.Add(KeyCode.Q, "Press Q to Turn on the GunCam");
        startupPrep.Add(KeyCode.W, "Press W to Check Forward Movement");
        startupPrep.Add(KeyCode.S, "Press S to Check Reverse Movement");
        startupPrep.Add(KeyCode.A, "Press A to Check Perpendicular Movement");
        startupPrep.Add(KeyCode.D, "Press D to Check Perpendicular Movement");
        startupPrep.Add(KeyCode.UpArrow, "Press Up Arrow to Check Weapon Targeting");
        startupPrep.Add(KeyCode.DownArrow, "Press Down Arrow to Check Weapon Targeting");
        startupPrep.Add(KeyCode.LeftArrow, "Press Left Arrow to Check Rotation");
        startupPrep.Add(KeyCode.RightArrow, "Press Right Arrow to Check Rotation");
        */
        startupPrep = new string[]{
            "Press R to Enable HUD",
            "Press Q to Turn on the GunCam",
            "Press W to Check Forward Movement",
            "Press S to Check Reverse Movement",
            "Press A to Check Perpendicular Movement",
            "Press D to Check Perpendicular Movement",
            "Press Up Arrow to Check Weapon Targeting",
            "Press Down Arrow to Check Weapon Targeting",
            "Press Left Arrow to Check Rotation",
            "Press Right Arrow to Check Rotation",
            ""
        };
        checkPassed = "Check Passed";
        checkStep = 0;
        lastCheckStep = checkStep;

        passedCheck = false;

    }

    void Update() {
        lastCheckStep = checkStep;

        if (Input.GetKeyDown(KeyCode.W) && !keysPressed[0]) {
            keysPressed[0] = true;
            numStartupButtons++;
            if(checkStep == 2) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && !keysPressed[1]) {
            keysPressed[1] = true;
            numStartupButtons++;
            if (checkStep == 3) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && !keysPressed[2]) {
            keysPressed[2] = true;
            numStartupButtons++;
            if (checkStep == 4) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && !keysPressed[3]) {
            keysPressed[3] = true;
            numStartupButtons++;
            if (checkStep == 5) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !keysPressed[4]) {
            keysPressed[1] = true;
            numStartupButtons++;
            if (checkStep == 6) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !keysPressed[5]) {
            keysPressed[5] = true;
            numStartupButtons++;
            if (checkStep == 7) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !keysPressed[6]) {
            keysPressed[6] = true;
            numStartupButtons++;
            if (checkStep == 8) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !keysPressed[7]) {
            keysPressed[7] = true;
            numStartupButtons++;
            if (checkStep == 9) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !keysPressed[8]) {
            keysPressed[8] = true;
            numStartupButtons++;
            if (checkStep == 1) {
                checkStep++;
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !keysPressed[9]) {
            keysPressed[9] = true;
            numStartupButtons++;
            if (checkStep == 0) {
                checkStep++;
            }
            HUD.enabled = true;
        }

        if (keysPressed[8] && currentDisplayImage.color.a > 0.0f) {
            currentDisplayImage.CrossFadeAlpha(0, 0.5f, false);
        }

        if(checkStep >= startupPrep.Length) {
            checkStep = startupPrep.Length - 1;
        }

        if(sequenceRenderer.GetAlpha() >= 1.0f) {
            sequence.CrossFadeAlpha(0, 1f, false);
        }

        if(checkStep > lastCheckStep) {
            passedCheck = true;
        }

        if(sequenceRenderer.GetAlpha() <= 0.0f) {
            sequence.CrossFadeAlpha(1.0f, 1f, false);
            if (passedCheck) {
                sequence.text = checkPassed;
                passedCheck = false;
            }
            else {
                sequence.text = startupPrep[checkStep];
            }
            
        }
        if(numStartupButtons >= keysPressed.Length - 3 && sequenceRenderer.GetAlpha() <= 0.1f) {
            startupSequence = false;
            startupCanvas.enabled = false;
            this.enabled = false;
        }
    }

    void FixedUpdate () {
        // static animation
        if(currentDisplayImage.color.a > 0.0f) {
            if (delayCounter > 5) {
                delayCounter = 0;
                if (toggleImage < noiseAnim.Length - 1) {
                    toggleImage++;
                }
                else {
                    toggleImage = 0;
                }
            }
            currentDisplayImage.sprite = noiseAnim[toggleImage];
            delayCounter++;
        }
        // end static animation
	}

    public bool inSequence() {
        return startupSequence;
    }
}
