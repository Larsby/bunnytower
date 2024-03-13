using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWeatherSystem : MonoBehaviour {
	private bool started = false;
	// Use this for initialization
	void Awake () {
		foreach (Transform t in transform) {
			Vector3 v = transform.position;
			transform.position = new Vector3 (v.x, v.y, 90.0f);
		}
		
	}
	public void StartWeather() {
		if (started == false) {
			started = true;
			int index = Random.Range (0, transform.childCount );
			StartStopParticleSystem sp = transform.GetChild (index).GetComponent<StartStopParticleSystem> ();
			sp.gameObject.SetActive (true);
			Vector3 v = sp.gameObject.transform.position;
			sp.gameObject.transform.position = new Vector3 (v.x, v.y,0.0f);
			if (sp != null) {
				sp.SetEnabled (true);
			}

		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
