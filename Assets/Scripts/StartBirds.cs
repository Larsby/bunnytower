using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBirds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (GameManager.instance.ShouldStartBirds ()) {
			GetComponent<StartStopParticleSystem> ().toggle = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
