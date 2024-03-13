using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBunny : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	IEnumerator Rotate(float time, BunnyBehaviour b) {
		yield return new WaitForSeconds (time);
		b.RotateToUpright ();
	}
	void FreeSpin(BunnyBehaviour b) {
		
		b.FreeSpin ();
	}
	void OnTriggerExit2D (Collider2D other)
	{



		if (other.CompareTag ("Bunny") == true) {

			BunnyBehaviour bunny = other.gameObject.GetComponent<BunnyBehaviour> ();
			if (bunny !=null) {
				StartCoroutine (Rotate (Random.Range (0.9f, 2.0f), bunny));
			}
		}
	}
	void OnTriggerEnter2D (Collider2D other)
	{



		if (other.CompareTag ("Bunny") == true) {
			
			BunnyBehaviour bunny = other.gameObject.GetComponent<BunnyBehaviour> ();
			if (bunny != null) {
				StartCoroutine (Rotate (Random.Range (0.9f, 2.0f), bunny));
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
