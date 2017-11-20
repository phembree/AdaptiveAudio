using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRotator : MonoBehaviour {

	private float r;

	// Use this for initialization
	void Start () {

		// generate a random number for rotation
		r = Random.Range (-180, 180);
	}
	
	// Update is called once per frame
	void Update () {

		// Rotate the pickup by 180 degrees, scaled by the duration of the last frame (about 0.02 usually)
		transform.Rotate (0, 0, r * Time.deltaTime);

	}
}
