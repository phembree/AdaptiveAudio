using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour {

	public float speed;

	private GameObject player;
	private Rigidbody2D rb2d;
	private Vector3 difference, direction;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		rb2d = GetComponent<Rigidbody2D> ();

	}
	
	void Update () {

		difference = player.transform.position - this.gameObject.transform.position;
		direction = difference.normalized;
		rb2d.AddForce (direction * speed);
		
	}
}
