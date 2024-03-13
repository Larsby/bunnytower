using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportWin : MonoBehaviour {
	private bool reportedWin = false;
	// Use this for initialization
	void Start () {
		reportedWin = false;
		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		if (collider == null) {
			Renderer render = GetComponent<Renderer> ();
			if (GetComponent<Renderer>() == null) {
				Debug.Log ("Cant auto add collider, no renderer found!");
				return;
			}
			gameObject.AddComponent<BoxCollider2D> ();
			collider = GetComponent<BoxCollider2D> ();
			collider.size = render.bounds.size;
			collider = GetComponent<BoxCollider2D> ();
			collider.isTrigger = true;

		}

	}
	void CheckWin(Collider2D other) {
		if (other.CompareTag ("Bunny") == true) {
			BunnyBehaviour bh = other.gameObject.GetComponent<BunnyBehaviour> ();

			if (bh != null && bh.IsStill ()) {
				if (bh.IsHolding () == false && reportedWin == false) {
					GameManager.instance.ReportWin ();
					reportedWin = true;
				}
			}
		}
	}
	void OnTriggerStay2D(Collider2D other)
	{
		CheckWin (other);
	}
	void OnTriggerEnter2D (Collider2D other) {
			
		CheckWin (other);	
	}
	

}
