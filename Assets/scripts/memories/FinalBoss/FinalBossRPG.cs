using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages final RPG boss fight
public class FinalBossRPG : Memory {
	public GameObject end_memory;
	public NormalAttack normal_attack;
	public GameObject a_appear;
	public GameObject b_appear;
	public GameObject a_fainted;
	public GameObject b_fainted;
	public GameObject defeated;
	public GameObject menu;
	public GameObject cursor;
	public GameObject dog_catcher;
	public GameObject out_of_mana;
	public AudioSource music_channel;

	public AudioClip fanfare;
	public AudioSource cursor_move_sfx;
	public AudioSource menu_select_sfx;
	public AudioSource out_of_mana_sfx;

	bool menu_mode;
	int cursor_pos;

	const float cursor_y = 2.23f;

	void Update() {
		if (menu_mode) {
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				cursor_pos = cursor_pos == 0 ? 0 : (cursor_pos - 1);
				cursor_move_sfx.Play ();
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				cursor_pos = cursor_pos == 2 ? 2 : (cursor_pos + 1);
				cursor_move_sfx.Play ();
			}
			cursor.transform.localPosition = new Vector2 (-2.05f, cursor_y + (-1 * cursor_pos * 0.37f));
			if (Input.GetKeyDown (KeyCode.Space)) {
				menu_select_sfx.Play ();
				if (cursor_pos == 0) {
					menu_mode = false;
					normal_attack.playAnimation ();
				} else {
					StartCoroutine (outOfMana ());
				}
			}
		}
	}

	public override void startMemory () {
		menu_mode = false;
		cursor_pos = 0;
		StartCoroutine (startCutscene ());
	}

	public override void endMemory () {
		this.gameObject.SetActive (false);
	}

	IEnumerator outOfMana() {
		menu_mode = false;
		cursor.SetActive (false);
		menu.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		out_of_mana_sfx.Play ();
		out_of_mana.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		out_of_mana.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		cursor.SetActive (true);
		menu.SetActive (true);
		menu_mode = true;
	}

	IEnumerator startCutscene() {
		yield return new WaitForSeconds (0.2f);
		a_appear.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		a_appear.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		b_appear.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		b_appear.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		menu.SetActive (true);
		cursor.SetActive (true);
		menu_mode = true;
	}

	public IEnumerator endCutscene() {
		yield return new WaitForSeconds (1f);
		a_fainted.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		a_fainted.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		b_fainted.SetActive (true);
		yield return new WaitForSeconds (1.5f);
		b_fainted.SetActive (false);
		yield return new WaitForSeconds (0.2f);
		defeated.SetActive (true);
		music_channel.clip = fanfare;
		music_channel.loop = false;
		music_channel.Play ();
		yield return new WaitForSeconds (4f);
		end_memory.SetActive (true);
		end_memory.GetComponent<Memory> ().startMemory ();
		music_channel.clip = end_memory.GetComponent<Memory> ().music;
		music_channel.loop = true;
		music_channel.Play ();
		this.endMemory ();
	}
}
