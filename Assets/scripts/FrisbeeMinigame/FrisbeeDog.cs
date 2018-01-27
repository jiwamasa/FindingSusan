using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages dog control during frisbee minigame
public class FrisbeeDog : MonoBehaviour {
	public FrisbeeMinigame frisbee_minigame; // manages minigame

	Rigidbody2D r_body; // rigid body of dog
	Vector2 jump_vec; // jump force vector
	bool grounded; // is dog on ground?

	const float gravity = 0.01f; // acceleration due to gravity
	const float jump_speed = 0.1f; // acceleration of jump
	const float ground_y = -0.71f; // location of ground
	const float ground_lim = 0.1f; // min speed to be "grounded"

	void Start () {
		r_body = GetComponent<Rigidbody2D> ();
		jump_vec = new Vector2 (0, 250000);
		grounded = false;
	}
	
	void FixedUpdate() {
		if (grounded && Input.GetKeyDown (KeyCode.Space)) { // jump
			r_body.AddForce(jump_vec);
			grounded = false;
		}
	}

	void Update () {
	}

	// check collisions with frisbee
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag ("ground"))
			grounded = true;
		if (collision.gameObject.CompareTag("frisbee"))
			frisbee_minigame.caughtFrisbee (collision.gameObject);
	}
}
