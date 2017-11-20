using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// add this to namespace for audiomixergroup
using UnityEngine.Audio;

public class VertRemix : MonoBehaviour {

	[Header("Target AudioMixerGroup.")]
	public AudioMixerGroup myOutputMixerGroup;

	[Header("Fade duration in seconds.")]
	public float fadeTime;

	// This struct allows us to encapsulate four useful variables in a package
	[System.Serializable]
	public struct AudioLayer {

		// Audio clip to play in our audio source for this layer
		public AudioClip audioClip;

		// Target volume to move toward over the fade time
		public float targetVol;

		// Audio source (which we'll dynamically instantiate and assign to this reference slot)
		[HideInInspector]
		public AudioSource audioSource;

		// Current velocity - used by our Mathf.Smoothdamp() function
		// We need one per layer
		[HideInInspector]
		public float curVel;
	}

	// We can make an array of our custom AudioLayer struct
	// instead of making multiple arrays, one for each variable we want to manipulate
	[Header("Audio Layers for vertical remixing.")]
	[Header("Size specifies the number of layers.")]
	[Header("AudioClip is the sound file assigned to loop on the specified layer.")]
	[Header("Target volume is the volume to fade to next.")]
	public AudioLayer[] audioLayers = new AudioLayer[2]; 

	void Start () {

		// On start, repeat this for loop for as many audioLayers as we have in our array
		for (int index = 0; index < audioLayers.Length; index++) {

			// Add an AudioSource component to this game object, and store that reference in our layer's audioSource slot
			audioLayers [index].audioSource = this.gameObject.AddComponent<AudioSource> ();

			// Assign the clip to the new source we just added
			audioLayers [index].audioSource.clip = audioLayers [index].audioClip;

			// Assign a starting volume (0f is silent)
			audioLayers [index].audioSource.volume = 0f;

			// Make the audioSource loop
			audioLayers [index].audioSource.loop = true;

			// bypass reverb zones
			audioLayers [index].audioSource.bypassReverbZones = true;

			// Set destination mixer group
			audioLayers [index].audioSource.outputAudioMixerGroup = myOutputMixerGroup;

			// Start it playing as well
			audioLayers [index].audioSource.Play ();

		}

	}

	void Update () {

		// For each of our audio layers
		for (int index = 0; index < audioLayers.Length; index++) {

			// Change the audioSource.volume property smoothly over time by calculating a new value this frame with SmoothDamp
			audioLayers[index].audioSource.volume = Mathf.SmoothDamp
			(
					// We base the new value on the old value...
					audioLayers[index].audioSource.volume, 

					// ...the target value...
					audioLayers[index].targetVol, 

					// ...a reference velocity...
					ref audioLayers[index].curVel,

					// ...and an approximate fade time
					fadeTime
			);
		}
	}
}
