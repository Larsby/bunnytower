using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDuckClicks : MonoBehaviour {
	int clicks = 0;
	// Use this for initialization
	void Start () {
		clicks = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown ()
	{
		clicks++;
		if (clicks == 10) {
			GameManager.instance.SetShowCredits (true);
			clicks = 0;
		}
	}
}
