using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 2-tuple helper class
public class Pair<T, U> {
	public T first;
	public U second;

	public Pair(T _first, U _second) {
		first = _first;
		second = _second;
	}
}
