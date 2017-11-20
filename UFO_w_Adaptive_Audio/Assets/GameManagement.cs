using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Need this namespace for dealing with our menu image
using UnityEngine.UI;

// Need this for access to the mixer
using UnityEngine.Audio;

public class GameManagement : MonoBehaviour {

	// Reference variable for our mixer (complete in inspector)
	public AudioMixer master;

	// Reference variable for our image (menu) object (complete in inspector)
	public Image menu;

	// Variable so we can keep track of if the game is running
	public bool playing;

	// Use this for initialization
	void Start () {

		// When we start the scene, pause the game
		Pause ();
	}
	
	// Update is called once per frame
	void Update () {

		// Each frame check if we are playing
		if (playing) {

			// if so, look for a key press of Escape
			if (Input.GetKeyDown (KeyCode.Escape)) {

				// if so, pause
				Pause ();
			}

		// if we are not playing
		} else {

			// look for an escape key press
			if (Input.GetKeyDown (KeyCode.Escape)) {

				// if so, unpause the game
				Play ();
			}
		}
	}

	// public function to handle restart
	public void Restart () {

		// We only have one scene, so when we restart we just reload that scene
		UnityEngine.SceneManagement.SceneManager.LoadScene ("scene_01");
	}

	// public function to quit the application
	public void Quit () {
		Application.Quit ();
	}

	// pause function (this is custom)
	public void Pause () {

		// Active our menu
		menu.gameObject.SetActive (true);

		// flag that we are not playing
		playing = false;

		// Set the physics engine to stop
		Time.timeScale = 0f;

		// Set the audio mixer to low pass aggressively
		master.SetFloat("LowPassCF", 300f);
	}

	// play (unpause) function
	public void Play () {

		// Deactivate menu
		menu.gameObject.SetActive (false);

		// flat that we ARE playing
		playing = true;

		// set the physics engine to start running at normal speed again
		Time.timeScale = 1f;

		// Set the mixer to not low pass at all (20KHz is top of human hearing)
		master.SetFloat("LowPassCF", 20000f);
	}

}
