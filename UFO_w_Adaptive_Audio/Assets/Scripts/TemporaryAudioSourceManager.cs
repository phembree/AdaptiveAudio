using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryAudioSourceManager : MonoBehaviour {

	public float pitchMin, pitchMax, volMin, volMax;

	private AudioSource audioSource;
	private float startTime;

	// Use this for initialization
	void Start () {

		audioSource = GetComponent<AudioSource> ();

		audioSource.pitch = Random.Range (pitchMin, pitchMax);
		audioSource.volume = Random.Range (volMin, volMax);

		audioSource.Play ();
		startTime = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time >= (audioSource.clip.length + startTime + 1)) {
			Destroy (this.gameObject);
		}
		
	}
}
