using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnTouch : MonoBehaviour {
	public PlayRandomSound player = null;
	// Use this for initialization
	void Start () {
		if (player == null) {
			player = GetComponent<PlayRandomSound> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown ()
	{
		if (player != null) {
			player.Play ();
		}
	}
}
