using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

	// Variables for other game objects - player is meant to be an instance, attackSound a prefab
	public GameObject player, attackSound;

	// Variable to hold a reference to the vertical remixer
	public VertRemix vertRemix;

	// Floats for range of awareness in unity units, speed, and attack cool down time
	public float awareness, speed, coolDown;

	// Reference to damage image UI object (solid color)
	public Image damageVignette;

	// Color of damage vignette (to be randomized)
	public Color damageColor;

	// Is this adversary agressive?
	public bool aggressive = true;

	// Reference for rigidibody
	private Rigidbody2D rb2d;

	// Reference for difference in position and direction
	private Vector3 difference, direction;

	// Flags for if it is chasing and recently did damage
	private bool isChasing, didDamage;

	// floats that are updated by functions below
	private float alphaVelocity, lastTouch;

	// Use this for initialization
	void Start () {

		// Complete our reference to the rigidbody
		rb2d = GetComponent<Rigidbody2D> ();

		// Record last touch time as now
		lastTouch = Time.time;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Get the difference in position between the player and this object
		difference = player.transform.position - this.gameObject.transform.position;

		// Caculate a direction by normalizing the difference vector to a unit of 1
		direction = difference.normalized;

		// If the player is within a certain distance (magnitude of the difference in position, and the enemy is aggressive)
		if (difference.magnitude <= awareness && aggressive) {

			// Flag that we are chasing
			isChasing = true;

			// Crossfade music
			vertRemix.fadeTime = 0.05f;
			vertRemix.audioLayers [3].targetVol = 1f;
			vertRemix.audioLayers [2].targetVol = 0f;

		} 

		// If we are chasing and we are aggressive
		if (isChasing && aggressive) {

			// Add a force to move the enemy toward the player
			rb2d.AddForce (direction * speed);
		}

		// If we recently did damage and we are within the cooldown period
		if (didDamage && Time.time <= lastTouch + coolDown) {

			// Calculate the vignette color (random with fadeout)
			float rand = Random.Range (0f, 1f);
			damageColor.r = Color.HSVToRGB (rand, 1f, 1f).r;
			damageColor.g = Color.HSVToRGB (rand, 1f, 1f).g;
			damageColor.b = Color.HSVToRGB (rand, 1f, 1f).b;
			damageColor.a = Mathf.SmoothDamp (damageColor.a, 0f, ref alphaVelocity, coolDown * 0.25f);
			damageVignette.color = damageColor;
		}
			
	}

	// If this collider is rubbing against something
	void OnCollisionStay2D(Collision2D other) {
			
		// If that other collider belongs to the player and we are past the cooldown time and we are aggressive
		if (other.gameObject.CompareTag ("Player") && (lastTouch + coolDown) <= Time.time && aggressive) {

			// Lower the player's score
			player.GetComponent<UfoController> ().count -= 1;

			// Update the score text
			player.GetComponent<UfoController> ().SetCountText ();

			// Record the last touch time
			lastTouch = Time.time;

			// Flag that we just did damage
			didDamage = true;

			// Change the damage color to opaque (alpha = 1)
			damageColor.a = 1f;

			// Set the color of the vignette
			damageVignette.color = damageColor;

			// Instantiate a temporary attack sound
			Instantiate (attackSound, this.transform.position, Quaternion.identity, this.transform);
		}
	
	}
}
