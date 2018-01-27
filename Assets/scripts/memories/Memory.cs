using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// represents a memory node (should be inherited)
abstract public class Memory : MonoBehaviour {
	public AudioClip music; // music theme for memory
	abstract public void startMemory (); // starts memory
	abstract public void endMemory(); // end memory
}
