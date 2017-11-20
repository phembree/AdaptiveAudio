using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

	// declare a public variable to hold a reference to our pickup prefab gameObject (assign in inspector)
	public GameObject pickup;

	// declare a static whole number of pickups to spawn
	public int numberOfPickups = 12;

	// Use this for initialization
	void Awake () {

		// for loop:
		for (
			
			// start at 0
			int i = 0;

			// check to see if we've reached the number of pickups to spawn
			// if not, keep executing this code
			// if so, exit the loop (below the last curly bracket)
			i < numberOfPickups;

			// when this loop is over, increment this number and start over (a counter)
			i++)
		{

			Vector3 location = new Vector3 (Random.Range (-10, 10), Random.Range(-10, 10), 0);

			// each time th loop is run, we instantiate a pickup at a random location
			Instantiate (
				
				// pickup prefab
				pickup,

				// random location between -10 and 10 unity units above or below origin
				this.gameObject.transform.position + location,

				// don't change the object's rotation
				Quaternion.identity, 

				// parent the pickup to the pickup parent
				this.gameObject.transform
			);  
		}

	}

}
