using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	NodeComparer node_comparer; // compares nodes based on distance from blip
	float hdir; // horizontal input amount
	float vdir; // vertical input amount

	const float blip_speed = 0.01f;  // speed of blip per frame
	const float blip_x_bound = 150f; // horizontal bound
	const float blip_y_bound = 120f; // vertical bound
	const float blip_shift_back = 0.03f; // amount of shift back if out of bounds

	void Start () {
		node_comparer = new NodeComparer ();
		node_comparer.blip_pos = blip_obj.transform.localPosition;
		nodes = new List<Pair<Vector2, GameObject>> ();

		// initialize nodes
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(-20f, -20f), GameObject.Find("FrisbeeMinigame")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(20f, 0), GameObject.Find("BoneDigMinigame")));
		nodes.Add(new Pair<Vector2, GameObject>(new Vector2(0, 50f), GameObject.Find("SpaceMemory")));
		nodes.Sort (node_comparer);
	}

	void FixedUpdate () {
		// BLOCK NORMAL INPUT IF IN MINIGAME
		// get arrow keys input
		hdir = 0;
		vdir = 0;
		if (Input.GetKey (KeyCode.UpArrow))
			++vdir;
		else if (Input.GetKey (KeyCode.DownArrow))
			--vdir;
		else if (Input.GetKey (KeyCode.RightArrow))
			++hdir;
		else if (Input.GetKey (KeyCode.LeftArrow))
			--hdir;
	}

	void Update() {
		updateBlip (); // update blip's position based on keyboard input
		node_comparer.blip_pos = blip_obj.transform.localPosition; // update comparer
		nodes.Sort(node_comparer);

		Debug.Log("nodes:");
		for (int i = 0; i < nodes.Count; ++i)
			Debug.Log (nodes[i].first);
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

}
