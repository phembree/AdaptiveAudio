using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Namespace for audio
using UnityEngine.Audio;

public class HorizReseq : MonoBehaviour {

	[Header("Target AudioMixerGroup.")]
	public AudioMixerGroup myOutputMixerGroup;

	// First let's make a custom struct
	// which will allow us to group several variables together

	// Tagging it with System.Serializable allows us to see this in the inspector
	[System.Serializable]

	// We'll call the struct "AudioSegment" to be different from AudioClip and AudioSource
	public struct AudioSegment {

		// each audio segment will contain a clip
		public AudioClip clip;

		// it will also contain a double-precision duration
		// allowing us to have audio-sample level time accuracy
		public double duration;

		// we'll also have an integer that represents which segment to play next
		// (assuming we'll be using a list or array of these later)
		public int nextSegment;
	}
		
	[Header("The following is an array of AudioSegments that can be sequenced in any order. ")]
	[Header("Each AudioSegment has a Clip, a Duration, and a Next Segment.")]
	[Header("Clip: the audio file to be triggered.")]
	[Header("Duration: the duration in seconds before the next segment is triggered.")]
	[Header("Next Segment: this is the segment to go to next.")]
	[Header("Note that the Next Segment is specified by its Element number.")]

	// Now let's create an array of these segments 
	// Arrays are just places to store multiple variables of the same type,
	// which you can access by index
	// The size of the array must be known in advance!
	public AudioSegment[] audioSegments = new AudioSegment[4];

	// we also want a variety of integers to control various things:
	private int nextSource, curSource, nextSegment, curSegment;

	// this public int will be our polyphony
	// or the number of AudioSources we want to use
	[Header("Number of voices to use (minimum is 2)")]
	[Range(2,16)]
	public int polyphony = 4;

	[Header("Initial delay in seconds to give the computer time to spool audio off the hard drive.")]
	public double initialDelay = 1;

	// A list is like an Array but you don't need to know the length of a list in advance.
	// We want the user to specify polyphony dynamically, so we'll use a list instead of an array
	private List<AudioSource> audioSources = new List<AudioSource>();

	// let's declare a double (64-bit high precision number)
	// to specify the absolute dsp time when the next audio segement comes in
	private double scheduledTime;

	// bool for an initialization flag
	private bool doOnce;

	// When start is called 
	void Start() {

		// we'll use a while loop to add audio sources to our horiz reseq player
		// start at 0
		int i = 0;

		// then keep adding audio sources while i is less than the number of sources we want
		while (i < polyphony) {

			// audioSources is our List, and we add to it each of
			// the AudioSources that we add using AddComponent
			audioSources.Add (this.gameObject.AddComponent<AudioSource> ());

			// increment counter
			i++;
		}

		foreach (AudioSource audioSource in audioSources) {
			audioSource.outputAudioMixerGroup = myOutputMixerGroup;
		}

		// Set the scheduled event time to NOW (as soon as Start() is done)
		scheduledTime = AudioSettings.dspTime;

	}

	// each video frame
	void Update() {
		
		// we check and see if we've reached the scheduled event time
		if (AudioSettings.dspTime >= scheduledTime) {
			
			// if so, and this is the first time this happens (no clip playing already)
			if (!doOnce) {
				
				// Delay this slightly into the future to give the CPU time
				// to start streaming audio off the hard drive
				scheduledTime = scheduledTime + initialDelay;

				// Assign the next clip to be scheduled
				audioSources [nextSource].clip = audioSegments [nextSegment].clip;

				// schedule the next audio source to play at a point in the future
				audioSources [nextSource].PlayScheduled (scheduledTime);

				// flag that we've done this initialization
				doOnce = true;

			// if we've reached the scheduled event time and we've already done our initialization
			} else {

				// Assign the next clip to be scheduled
				audioSources [nextSource].clip = audioSegments [nextSegment].clip;

				// calculate when to play it based on current clip duration
				scheduledTime = audioSegments [curSegment].duration + scheduledTime;

				// schedule the next audio source to play at a point in the future
				audioSources [nextSource].PlayScheduled (scheduledTime);
			}
				
			// get the next curSegment from nextSegment
			curSegment = nextSegment;

			// get the next nextSegment by looking it up in the segment array
			nextSegment = audioSegments[nextSegment].nextSegment;

			// get the current source from the old next source
			curSource = nextSource;

			// update next source
			nextSource = (nextSource + 1) % polyphony;
		}

	}

	public void ChangeScheduledSegment (int segment) {

		// Future (t + 1)
		// Change the currently scheduled segment based on input
		curSegment = segment;

		// Find the segment that follows that
		nextSegment = audioSegments[curSegment].nextSegment;

		// stop the audio source that is scheduled to play the currently scheduled segment
		audioSources [curSource].Stop ();

		// assign its new target clip
		audioSources [curSource].clip = audioSegments [curSegment].clip;

		// set play time (time to play next is still accurate
		// because it is based on a clip already playing).
		audioSources [curSource].PlayScheduled (scheduledTime);

	}


}
