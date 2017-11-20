using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertRemixTrigger : MonoBehaviour {

	[Header("Assign the reference for which VertRemix script to use")]
	public VertRemix vertRemix;

	[Header("Override fade duration")]
	public float fadeTime;

	[Header("Specify an audio layer to fade in when triggered")]
	public int audioLayerToFade;

	[Header("If you want to fade out when the player leaves this trigger, check this")]
	public bool fadeOutOnExit = false;

	// when an object enters this trigger
	void OnTriggerEnter2D(Collider2D other){

		// if the object attached to the collider is tagged player
		if (other.gameObject.CompareTag ("Player")) {

			// override the fade time on our vertRemix instance
			vertRemix.fadeTime = fadeTime;

			// turn up the volume on the target audio layer
			vertRemix.audioLayers [audioLayerToFade].targetVol = 1f;
		}
	}
		
	// when an object exits this trigger
	void OnTriggerExit2D(Collider2D other){

		// if the object attached ot the collider is tagged player AND we want to fade out on exit
		if (other.gameObject.CompareTag ("Player") && fadeOutOnExit) {

			// overrride the fade time on our vertRemix instance
			vertRemix.fadeTime = fadeTime;

			// turn down the volume on the target audio layer
			vertRemix.audioLayers [audioLayerToFade].targetVol = 0f;
		}
	}
}
