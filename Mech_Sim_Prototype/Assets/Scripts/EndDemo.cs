using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndDemo : MonoBehaviour {

    private Image fadeOut;
    private bool fade;
    private float loadTimer;

	// Use this for initialization
	void Start () {
        fadeOut = GetComponent<Image>();
        fade = false;
        loadTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (loadTimer > 2.0f) {
            SceneManager.LoadScene(0);
        }

        if (fadeOut.color.a >= 1.0f) {
            loadTimer += Time.unscaledDeltaTime;
        }

        if (fade) {
            fadeOut.color = new Color(fadeOut.color.r, fadeOut.color.g, fadeOut.color.b, fadeOut.color.a + Time.unscaledDeltaTime / 3);
        }
	}

    public void startFade() {
        fade = true;
    }
}
