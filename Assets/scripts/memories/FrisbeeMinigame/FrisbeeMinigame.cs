using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// manages frisbee minigame
public class FrisbeeMinigame : Memory {
	public FrisbeeDog dog; // dog script
	public Rigidbody2D frisbee_r_body; // frisbee rigid body
	public TextMesh score_text; // displays score

	int score; // keeps track of frisbees caught
	Vector2 frisbee_start_pos; // starting position of frisbee
	Vector2 frisbee_throw_vec; // force vector for frisbee throwing

	const float throw_wait = 4f; // time between frisbee throws
	const float frisbee_grav = 0.5f; // gravity scale of frisbee

	void Start() {
		frisbee_start_pos = new Vector2 (1.2f, 0.25f);
		frisbee_throw_vec = new Vector2 (-380f, 130f);
	}

	public override void startMemory () {
		Debug.Log ("starting frisbee game");
		dog.gameObject.SetActive (true);
		frisbee_r_body.gameObject.SetActive (true);
		score_text.gameObject.SetActive (true);
		score_text.text = "0";
		resetFrisbee ();
		StartCoroutine (throwFrisbee ());
	}

	public override void endMemory() {
		StopAllCoroutines ();
		score_text.text = "0";
		resetFrisbee ();
		dog.gameObject.SetActive (false);
		frisbee_r_body.gameObject.SetActive (false);
		score_text.gameObject.SetActive (false);
	}

	// throws frisbee in a loop
	IEnumerator throwFrisbee() {
		while (true) {
			yield return new WaitForSeconds (throw_wait);
			resetFrisbee ();
			frisbee_r_body.gravityScale = frisbee_grav;
			frisbee_r_body.AddForce (frisbee_throw_vec);
		}
	}

	// called when frisbee is caught
	public void caughtFrisbee(GameObject frisbee_obj) {
		score_text.text = (++score).ToString();
		resetFrisbee ();
	}

	// catches frisbees that are about to go off screen
	void OnCollisionEnter2D() {
		resetFrisbee ();
	}

	// rests frisbee
	void resetFrisbee() {
		frisbee_r_body.gravityScale = 0;
		frisbee_r_body.position = frisbee_start_pos;
		frisbee_r_body.velocity = Vector2.zero;
	}
}
