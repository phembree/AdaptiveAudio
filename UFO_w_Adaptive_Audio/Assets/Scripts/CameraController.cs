using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Declare a variable to hold a reference to our player gameObject
	// This reference will be completed in the inspector by dragging our player object into the player field of the CameraController script
	public GameObject player;

	// Declare a variable to hold the difference in coordates between the camera's starting position and the player's starting position
	private Vector3 positionOffset;

	// Use this for initialization
	void Start () {

		// Calculate the difference in coordinates between the camera and player
		positionOffset = transform.position - player.transform.position; 
		
	}
	
	// Update is called once per frame
	void Update () {

		// move the camera to the player's position, plus the difference in coordiates specified by the offset
		transform.position = player.transform.position + positionOffset;
		
	}
}
