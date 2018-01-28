using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// compares nodes by their distances from blip
class NodeComparer : IComparer<Pair<Vector2, GameObject>> {
	public Vector2 blip_pos; // position of blip

	public int Compare(Pair<Vector2, GameObject> a, Pair<Vector2, GameObject> b) {
		float a_dist = Vector2.Distance (a.first, blip_pos);
		float b_dist = Vector2.Distance (b.first, blip_pos);
		if (a_dist > b_dist) return 1;
		else if (a_dist == b_dist) return 0;
		else return -1;
	}
}

// manages tuning via the arrow keys and space bar
public class Tune : MonoBehaviour {
	public GameObject blip_obj; // blip sprite on the minimap
	public SpriteRenderer static_sprite; // static sprite for creating static effect
	public List<Pair<Vector2, GameObject>> nodes; // tunable locations (memories)
	public AudioSource static_channel; // plays static on loop
	public AudioSource music_channel; // plays music themes for memories
	public Text x_text; // shows x_coord
	public Text y_text; // shows y_coord
	public GameObject final_boss_memory; // memory of final boss
	public GameObject view_memory_mask; 

	[HideInInspector]
	public Pair<Vector2, GameObject> nearest; // currently nearest node
	[HideInInspector]
	public float dist; // current distance to nearest node
	[HideInInspector]
	public bool memory_mode; // has a node been activated?
	[HideInInspector]
	public bool final_boss; // are we fighting the final boss?

	NodeComparer node_comparer; // compares nodes based on distance from blip
	float hdir; // horizontal input amount
	float vdir; // vertical input amount

	const float blip_speed = 0.005f;  // speed of blip per frame
	const float blip_x_bound = 100f; // horizontal bound
	const float blip_y_bound = 78.5f; // vertical bound
	const float blip_shift_back = 0.03f; // amount of shift back if out of bounds
	public const float visible_dist = 30f; // distance before static starts to fade away
	public const float find_dist = 5f; // distance before player can activate memory

	void Start () {
		// initialize node list
		node_comparer = new NodeComparer ();
		node_comparer.blip_pos = blip_obj.transform.localPosition;
		nodes = new List<Pair<Vector2, GameObject>> ();
		// initialize nodes
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(0f, 0f), GameObject.Find("Instructions")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(-30f, -30f), GameObject.Find("FrisbeeMinigame")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(50f, 0), GameObject.Find("BoneDigMinigame")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(0, 55f), GameObject.Find("SpaceMemory")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(20f, -40f), GameObject.Find("WatermelonMemory")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(-50f, 50f), GameObject.Find("CuddleMemory")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(35f, 30f), GameObject.Find("RubMemory")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(-45f, 0f), GameObject.Find("CowboyMemory")));
		nodes.Sort (node_comparer);
		foreach (Pair<Vector2, GameObject> node in nodes)
			node.second.SetActive (false);
		// initialize other state
		memory_mode = false;
		final_boss = false;
		approachNode(); 
	}

	void FixedUpdate () {
		hdir = 0;
		vdir = 0;
		if (final_boss) return;
		if (!memory_mode) {
			// get arrow keys input
			if (Input.GetKey (KeyCode.UpArrow))
				++vdir;
			else if (Input.GetKey (KeyCode.DownArrow))
				--vdir;
			else if (Input.GetKey (KeyCode.RightArrow))
				++hdir;
			else if (Input.GetKey (KeyCode.LeftArrow))
				--hdir;
			// check spacebar
			if (Input.GetKeyDown (KeyCode.Space))
				activateNode ();
		} else {
			// check spacebar
			if (Input.GetKeyDown (KeyCode.Space))
				deactivateNode ();
		}
	}

	void Update() {
		if (final_boss) return;
		if (!memory_mode && (hdir != 0 || vdir != 0)) {
			updateBlip (); // update blip's position based on keyboard input
			node_comparer.blip_pos = blip_obj.transform.localPosition; // update comparer
			nodes.Sort(node_comparer); // resort list based on distance
			approachNode(); // check if approaching any memory node, and act accordingly
			x_text.text = blip_obj.transform.localPosition.x.ToString(); // set x coordinate text
			y_text.text = blip_obj.transform.localPosition.y.ToString(); // set y coordinate text
		}
		/*
		Debug.Log("nodes:");
		for (int i = 0; i < nodes.Count; ++i)
			Debug.Log (nodes[i].first);
		*/
	}

	// updates blips position
	void updateBlip() {
		// move minimap blip
		blip_obj.transform.Translate (hdir * blip_speed, vdir * blip_speed, 0);
		// check blip bounds, and move back if past
		Vector2 blip_pos = blip_obj.transform.localPosition;
		if (Mathf.Abs (blip_pos.x) > blip_x_bound) 
			blip_obj.transform.Translate (-1 * hdir * blip_shift_back, -0, 0);
		if (Mathf.Abs (blip_pos.y) > blip_y_bound)
			blip_obj.transform.Translate (0, -1 * vdir * blip_shift_back, 0);
	}

	// updates distance when nearing memory node
	// spawns memory object, fades out static, and fades in music 
	void approachNode() {
		// get distance from nearest memory
		nearest = nodes[0];
		dist = Vector2.Distance (blip_obj.transform.localPosition, nearest.first);
		if (dist < visible_dist) { // if within visible distance
			static_sprite.color = new Color (1, 1, 1, dist / visible_dist); // fade out static
			static_channel.volume = dist / visible_dist; // fade out static sound
			music_channel.volume = 1 - (dist / visible_dist); // fade in music
			if (!nearest.second.activeInHierarchy) { // show nearest memory node
				nearest.second.SetActive (true);
				for (int i = 1; i < nodes.Count; ++i) // hide others
					nodes[i].second.SetActive(false);
				music_channel.clip = nearest.second.GetComponent<Memory> ().music;
				music_channel.Play ();
			}
		} else {
			nearest.second.SetActive (false); // hide all memory nodes if too far
		}
	}

	// activates node if node is within find dist
	void activateNode() {
		if (dist < find_dist) {
			if (nearest.second.name.CompareTo("Instructions") == 0) return; // dont count instructions
			memory_mode = true;
			static_sprite.gameObject.SetActive (false); // hide static
			static_channel.mute = true; // turn off static noise
			nearest.second.GetComponent<Memory>().startMemory(); // start memory
		}
	}

	// go back into tuning mode from node
	void deactivateNode() {
		nearest.second.GetComponent<Memory> ().endMemory (); // end memory sequence
		memory_mode = false;
		static_sprite.gameObject.SetActive (true); // show static
		static_channel.mute = false; // turn on static noise
	}

	// activates final rpg battle memory node
	public void activateRPG() {
		Debug.Log ("starting rpg battle...");
		StartCoroutine (preBossStatic ());
	}

	// show static before final battle
	IEnumerator preBossStatic() {
		view_memory_mask.SetActive (false);
		for (int i = 0; i < nodes.Count; ++i) // hide all other nodes
			nodes[i].second.SetActive(false);
		final_boss = true;
		music_channel.Stop(); // stop music
		static_sprite.gameObject.SetActive (true); // show static
		static_sprite.color = new Color(1, 1, 1, 1);
		static_channel.mute = false; // turn on static noise
		static_channel.volume = 1;
		yield return new WaitForSeconds (2.5f);
		static_sprite.gameObject.SetActive (false); // hide static
		static_channel.mute = true; // turn off static noise
		final_boss_memory.SetActive(true);
		music_channel.clip = final_boss_memory.GetComponent<Memory> ().music;
		music_channel.volume = 1;
		music_channel.Play ();
		final_boss_memory.GetComponent<Memory> ().startMemory ();
	}
}
