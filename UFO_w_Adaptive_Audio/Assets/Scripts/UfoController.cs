using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Make sure to use the UI namespace to give yourself access to the Text class
using UnityEngine.UI;

public class UfoController : MonoBehaviour {

	// reference to an enemy controller script instance
	public EnemyController enemyController;

	// prefabs for temp audio sources
	public GameObject pickupSource, impactSource;

	// variable to hold reference to count (score) and win text
	public Text countText, winText;

	// whole number (integer) for the number of pickups we've collected
	public int count = 0;

	// variable to store the number of pickups in the scene
	private int numPickups;

	// speed (velocity) multiplier for your UFO
	public float speed, revUpTime;

	// Variable to hold reference to your Rigidbody2D (the physics component that allows you to apply forces, etc).
	private Rigidbody2D rb2d;

	// variable for motor/engine audiosource (since we'll manipulate its pitch, we need a variable to hold the reference)
	private AudioSource audioSource;

	// variabe for the animator component (again, we'll manipulate its speed, so we need to hold a reference)
	private Animator animator;

	// Vector3 for movement direction
	private Vector3 movement;

	private float animVel, pitchVel;


	// Use this for initialization
	void Start ()
	{
		// Count all the objects tagged pickup
		numPickups = GameObject.FindGameObjectsWithTag ("PickUp").Length;

		// Go find the Rigidbody2D component also attached to this game object, and apply that to our variable 
		rb2d = GetComponent<Rigidbody2D> ();

		// complete audiosource reference
		audioSource = GetComponent<AudioSource> ();
	
		// complete animator reference
		animator = GetComponent<Animator> ();

		// set the count and win texts
		SetCountText ();

	}

	// Fixed Update is called once per physics update cycle (a fixed time interval)
	void FixedUpdate () {


		// Get the input along the horizontal axis, or your left and right arrow keys, a number between -1, left and 1, right
		float moveHoriz = Input.GetAxis ("Horizontal");

		// Get the input along the vertical axis, or your up and down keys (between -1, down and 1, up)
		float moveVert = Input.GetAxis ("Vertical");

		// Create a movement vector out of those two float inputs
		movement = new Vector3 (moveHoriz, moveVert, 0).normalized;

		// Add a force along that movement vector
		rb2d.AddForce (movement * speed);

		// Change the animator speed
		animator.speed = Mathf.SmoothDamp (animator.speed, (movement * speed).magnitude / 5f, ref animVel, revUpTime);

		// Change the pitch of the engine
		audioSource.pitch = Mathf.SmoothDamp (audioSource.pitch, (movement * speed).magnitude / 5f, ref pitchVel, revUpTime);

	}

	// When we run over a trigger 
	void OnTriggerEnter2D (Collider2D other)
	{
		// check to see if that trigger belonged to a pickup
		if (other.gameObject.CompareTag ("PickUp"))
		{

			// disable the pickup
			other.gameObject.SetActive (false);

			// increment our score
			count = count + 1;

			// Set the counter text
			SetCountText ();

			// create a temporary audiosource, playing back pickupAudioClip, at the position in space of the other collider2D
			Instantiate(pickupSource, transform.position, Quaternion.identity);
		}
	}
		
	// When we hit a collider (walls)
	void OnCollisionEnter2D(Collision2D collision){

		// trigger a sound
		Instantiate(impactSource, transform.position, Quaternion.identity);
	}

	// Custom Set Counter funciton (called when useful above)
	public void SetCountText ()
	{

		// change the text field of the countText to a string plus an integer that has been converted to a string
		countText.text = "Score: " + count.ToString () + " of " + numPickups.ToString() +  " possible.";

		// if we just reached the number of pickups spawned
		if (0 == GameObject.FindGameObjectsWithTag ("PickUp").Length) {

			enemyController.aggressive = false;

			// then we won
			winText.text = "You won!";
								
		// or else
		} else {

			// winText is still blank
			winText.text = "";
		}

	}




}
