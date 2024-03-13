using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectsToParent : MonoBehaviour {
	public Transform [] childrenToBe;
	// Use this for initialization
	void Start () {
		foreach (Transform c in childrenToBe) {
			c.parent = transform;
		}
	}
	

}
