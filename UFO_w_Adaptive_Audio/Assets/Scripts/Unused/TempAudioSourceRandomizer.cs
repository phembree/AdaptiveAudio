using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioSourceRandomizer : MonoBehaviour {

	// Create an array to hold several audio clips
	// You can make an array of any data type by following up the type with open & closed brackets []
	// Arrays in C# are objects and need to be constructed, so use the new keyword, and initialize the size of the array
	// The one weakness of arrays is that their size must be known in advance
	public AudioClip[] audioClips = new AudioClip[3]; 

	// Variables so we can customize pitch and volume ranges
	public float pitchMin, pitchMax, volMin, volMax;

	// Reference variable for the audioSource component
	private AudioSource audioSource;

	// Float to hold start time of audio source
	private float startTime;

	// Use this for initialization
	void Start () {

		// Get the AudioSource component
		audioSource = GetComponent<AudioSource> ();

		// assign the clip randomly by picking from within the audioClips array by a random index
		audioSource.clip = audioClips [Random.Range (0, audioClips.Length)];

		// you can also randomize the playback rate ("pitch")
		audioSource.pitch = Random.Range (pitchMin, pitchMax);

		// and the linear gain ("volume")
		audioSource.volume = Random.Range (volMin, volMax);

		// start the audio source playing manually
		audioSource.Play();

		// Get the time when this sound starts
		startTime = Time.time;

	}

	// Update is called once per frame
	void Update () {

		// if the time exceeds the length of the audio clip, plus start time, plus an extra second...
		if (Time.time >= (audioSource.clip.length + startTime + 1f)) {

			// ...destroy this object (the temporary source)
			Destroy (this.gameObject);
		}

	}

}
