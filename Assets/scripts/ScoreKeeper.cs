using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// keeps track of number of memories collected
public class ScoreKeeper : MonoBehaviour {
	public static ScoreKeeper main = null; // global static reference
	public SpriteMask progress_mask; // for showing progress

	List<GameObject> collected; // list of collected memories

	const int max_score = 6; // number of memories collectable
	const float mask_start = 200f; // starting 'Top' of mask (goes up to 0)

	void Start() {
		if (main == null) main = this;
		collected = new List<GameObject> ();
	}

	// add collected memories
	public void addScore(GameObject memory) {
		if (!collected.Contains (memory)) collected.Add (memory);
		Debug.Log ("current score:" + collected.Count);
		float offset = mask_start - (mask_start * ((float)collected.Count/(float)(max_score + 2)));
		progress_mask.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, -1 * offset);
		if (collected.Count >= 6) {
			progress_mask.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, mask_start);
			Debug.Log ("BOSS BATTLE");
		}
	}
	

}
