using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// displays instructions (represented as a simple memory at 0,0)
public class Instructions : Animatic {
	public GameObject instruction_text;

	void Start() {
		startMemory ();
	}

	public override void startMemory () {
		StartCoroutine (animate ());
	}

	public override void endMemory () {
		
	}

	IEnumerator animate() {
		for (int i = 0; i < sprites.Count; ++i) {
			yield return new WaitForSeconds (frame_len);
			sprite_r.sprite = sprites [i];
		}
		instruction_text.SetActive (true);
	}
}
