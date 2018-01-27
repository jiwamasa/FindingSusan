﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffects : MonoBehaviour {
	public Tune tune; // for keeping track of tune state
	// light up masks for buttons
	public GameObject up_mask;
	public GameObject down_mask;
	public GameObject left_mask;
	public GameObject right_mask;
	public GameObject space_mask;

	List<GameObject> arrow_masks; // list all arrow masks

	const float blink_scale = 0.5f; // scaling for radar blink speed 

	void Start() {
		arrow_masks = new List<GameObject> ();
		foreach(Transform child in transform) arrow_masks.Add (child.gameObject);
		arrow_masks.Remove (space_mask);
		foreach (GameObject obj in arrow_masks) obj.SetActive (false);
		space_mask.SetActive (false);
		StartCoroutine (radar ());
	}

	void Update () {
		// turn off all masks
		foreach (GameObject obj in arrow_masks) obj.SetActive (false);
		if (!tune.memory_mode) {
			// get arrow keys input
			if (Input.GetKey (KeyCode.UpArrow))
				up_mask.SetActive (true);
			else if (Input.GetKey (KeyCode.DownArrow))
				down_mask.SetActive (true);
			else if (Input.GetKey (KeyCode.RightArrow))
				right_mask.SetActive (true);
			else if (Input.GetKey (KeyCode.LeftArrow))
				left_mask.SetActive (true);
		}
	}

	// radar effect when approaching nodes
	IEnumerator radar() {
		while (true) {
			if (!tune.memory_mode && tune.dist > Tune.find_dist) { // approaching
				float ratio = ((float)tune.dist / (float)Tune.visible_dist);
				if (ratio < 0f) ratio = 0f;
				yield return new WaitForSeconds (ratio * blink_scale);
				space_mask.SetActive (!space_mask.activeInHierarchy);
			} else { // found
				space_mask.SetActive (true);
				yield return new WaitForEndOfFrame ();
			}
		}
	}
}