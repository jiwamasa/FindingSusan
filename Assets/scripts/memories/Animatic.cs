using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// plays short animatic
public class Animatic : Memory {
	public SpriteRenderer sprite_r; // renders sprite
	public List<Sprite> sprites; // sprites in animatic
	public float frame_len; // length of frame in seconds

	public override void startMemory () {
		StartCoroutine (animate ());
	}

	public override void endMemory() {
		StopAllCoroutines ();
	}

	IEnumerator animate() {
		int i = sprites.IndexOf(sprite_r.sprite);
		while (true) {
			yield return new WaitForSeconds (frame_len);
			sprite_r.sprite = sprites [i];
			i = (i + 1) % sprites.Count;
		}
	}
}
