﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sets resolution of screen
public class SetResolution : MonoBehaviour {

	void Awake () {
		Screen.SetResolution (1280, 720, false);	
	}
}
