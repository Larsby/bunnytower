using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeWater : MonoBehaviour {

	Renderer ren;
	// Use this for initialization
	void Start () {
		ren = GetComponent<Renderer> ();
		ren.material.SetFloat ("_SinDistortion", Random.Range(0.15f,0.30f));

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
