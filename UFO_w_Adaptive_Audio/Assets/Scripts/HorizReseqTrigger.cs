using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizReseqTrigger : MonoBehaviour {

	// Which audio segment does this trigger?
	[Header("Which audio segment should be played next after hitting this trigger?")]
	public int targetSegment;

	// Which HorizReseq script manages this segment
	[Header("Which horizReseq script are we controlling?")]
	public HorizReseq horizReseq;

	// Does the player start inside this trigger (is it the openning area)
	[Header("Check this box if the player starts within this trigger.")]
	public bool playerIsInside = false;

	// Unity calls this message when an object with a 2D collider enters THIS object's 2D trigger collider 
	void OnTriggerEnter2D (Collider2D otherCollider){

		// If the object with a collider that enters this collider is tagged player, and the player did NOT start inside this
		if (otherCollider.gameObject.CompareTag ("Player") && playerIsInside != true) {

			// Print which trigger we entered
			Debug.Log ("Player entered trigger " + this.gameObject.name);

			// Call the ChangeScheduledSegment method on the horizontal resequencer to this trigger's targetSegment
			horizReseq.ChangeScheduledSegment (targetSegment);

			// Flag that the player is inside
			playerIsInside = true;
		}

	}

	// Unity calls this message when an object with a 2D collider exits THIS object's 2D trigger collider
	void OnTriggerExit2D (Collider2D otherCollider){

		// If the object that left was the player, and the player was last inside this 
		if (otherCollider.gameObject.CompareTag ("Player") && playerIsInside == true) {

			// Print that we exited 
			Debug.Log ("Player left trigger " + this.gameObject.name);

			// and flag that the player is no longer inside
			playerIsInside = false;
		}

	}
}
