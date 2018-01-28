using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages digging minigame
public class DiggingMinigame : Memory {	
	public Tune tune; // main tuner script
	public Transform main_camera; // main camera object for screen shake
	public AudioSource dig_sfx;

	GameObject[] holes; // array of hole sprites (0-8)
	int[] hole_state; // states of holes (how much dug)
	bool digging; // is dog currently digging?

	const float shake_amt = 0.2f;  // amount of screen shake
	const float shake_time = 0.4f; // length of screen shake
	const int dig_amt = 3;  // number of times needed to dig
	const int bone_loc = 3; // location of bone

	void Start() {
		digging = false;
		holes = new GameObject[9];
		hole_state = new int[9];
		for (int i = 0; i < 9; ++i) {
			holes [i] = transform.GetChild (i).gameObject;
			hole_state [i] = dig_amt;
		}
		resetHoles ();
	}

	void Update() {
		if (tune.memory_mode) {
			if (Input.GetKeyDown (KeyCode.Q))
				digHole (0);
			else if (Input.GetKeyDown (KeyCode.W))
				digHole (1);
			else if (Input.GetKeyDown (KeyCode.E))
				digHole (2);
			else if (Input.GetKeyDown (KeyCode.A))
				digHole (3);
			else if (Input.GetKeyDown (KeyCode.S))
				digHole (4);
			else if (Input.GetKeyDown (KeyCode.D))
				digHole (5);
			else if (Input.GetKeyDown (KeyCode.Z))
				digHole (6);
			else if (Input.GetKeyDown (KeyCode.X))
				digHole (7);
			else if (Input.GetKeyDown (KeyCode.C))
				digHole (8);
		}
	}

	public override void startMemory () {
		digging = false;
		resetHoles ();
	}

	public override void endMemory () {
		resetHoles ();
	}

	// dig a specific hole if not already digging
	// if bone is found, add score
	void digHole(int i) {
		if (hole_state[i] <= 0) return;
		if (!digging) {
			dig_sfx.Play ();
			StartCoroutine (screenShake ());
			if (--hole_state [i] == 0) {
				holes [i].SetActive (true);
				Debug.Log (i);
				if (i == bone_loc) {
					ScoreKeeper.main.addScore (gameObject);
				}
			}
		}
	}

	// reset dig process on all holes
	void resetHoles() {
		foreach (GameObject hole in holes)
			hole.SetActive (false);
	}

	// shakes screen briefly
	IEnumerator screenShake() {
		digging = true;
		Vector3 old_pos = main_camera.position;
		float curr_time= 0;
		while (curr_time < shake_time) {
			Vector3 new_pos = Random.insideUnitSphere * shake_amt;
			new_pos.z = 0f;
			main_camera.position = new_pos + old_pos;
			yield return new WaitForEndOfFrame ();
			curr_time += Time.deltaTime;
		}
		main_camera.position = old_pos;
		digging = false;
	}
}
