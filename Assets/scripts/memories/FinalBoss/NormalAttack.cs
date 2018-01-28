using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles normal attack effect
public class NormalAttack : MonoBehaviour {
	public FinalBossRPG final_boss_rpg;
	public List<Sprite> sprites; 
	public Transform screen;
	public SpriteRenderer sprite_r;
	public AudioSource attack_sfx;

	public Coroutine playAnimation() {
		attack_sfx.Play ();
		final_boss_rpg.menu.SetActive (false);
		final_boss_rpg.cursor.SetActive (false);
		return StartCoroutine (animate ());
	}

	IEnumerator animate() {
		StartCoroutine (screenShake ());
		for (int i = 0; i < sprites.Count; ++i) {
			sprite_r.sprite = sprites [i];
			yield return new WaitForSeconds (0.1f);
		}
		yield return new WaitForSeconds (1f);
		for (int i = sprites.Count - 1; i >= 0; --i) {
			sprite_r.sprite = sprites [i];
			yield return new WaitForSeconds (0.1f);
		}
		sprite_r.sprite = null;
		for (int i = 0; i < 60; ++i) {
			final_boss_rpg.dog_catcher.GetComponent<SpriteRenderer> ().color = 
				new Color (1, 1, 1, 1f - (float)i/60f);
			yield return new WaitForEndOfFrame ();
		}
		StartCoroutine (final_boss_rpg.endCutscene ());
	}

	IEnumerator screenShake() {
		float intensity = 0.01f;
		Vector3 old_pos = screen.position;
		float curr_time= 0;
		while (curr_time < 2f) {
			Vector3 new_pos = Random.insideUnitSphere * intensity;
			new_pos.z = 0f;
			screen.position = new_pos + old_pos;
			yield return new WaitForEndOfFrame ();
			curr_time += Time.deltaTime;
			intensity *= 1.05f;
		}
		curr_time = 0;
		while (curr_time < 0.5f) {
			Vector3 new_pos = Random.insideUnitSphere * intensity;
			new_pos.z = 0f;
			screen.position = new_pos + old_pos;
			yield return new WaitForEndOfFrame ();
			curr_time += Time.deltaTime;
			intensity /= 1.3f;
		}
		screen.position = old_pos;
	}

}
