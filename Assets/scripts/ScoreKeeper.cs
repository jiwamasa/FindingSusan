using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// keeps track of number of memories collected
public class ScoreKeeper : MonoBehaviour {
	public static ScoreKeeper main = null; // global static reference
	public SpriteMask progress_mask; // for showing progress
	public Tune tune;
	public GameObject alert_text; 

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
		if (collected.Count > max_score)
			return;
		Debug.Log ("current score:" + collected.Count);
		float offset = mask_start - (mask_start * ((float)collected.Count/(float)(max_score + 2)));
		progress_mask.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, -1 * offset);
		if (collected.Count >= max_score) {
			progress_mask.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, 0);
			StartCoroutine (flashMeter ());
			Debug.Log ("BOSS BATTLE");
		}
	}

	// flashes meter and found susan warning and starts battle
	IEnumerator flashMeter() {
		for (int i = 0; i < 4; ++i) {
			progress_mask.gameObject.SetActive (false);
			alert_text.gameObject.SetActive (true);
			yield return new WaitForSeconds (0.5f);
			progress_mask.gameObject.SetActive (true);
			alert_text.gameObject.SetActive (false);
			yield return new WaitForSeconds (0.5f);
		}
		tune.activateRPG ();
	}

}
