using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages dog control during frisbee minigame
public class FrisbeeDog : MonoBehaviour {
	public FrisbeeMinigame frisbee_minigame; // manages minigame
	public AudioSource jump_sfx;

	Rigidbody2D r_body; // rigid body of dog
	Vector2 jump_vec; // jump force vector
	bool grounded; // is dog on ground?

	void Start () {
		r_body = GetComponent<Rigidbody2D> ();
		jump_vec = new Vector2 (0, 300000);
		grounded = false;
	}
	
	void FixedUpdate() {
		if (grounded && Input.GetKeyDown (KeyCode.UpArrow)) { // jump
			jump_sfx.Play();
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
