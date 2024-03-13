using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceOffOnTrigger : MonoBehaviour {
	private bool triggered = false;
	private bool scoreGiven = false;
	Rigidbody2D body;

	void Bounce(float otherXCenter) {
		if (scoreGiven == false) {
			GameManager.instance.AddCarrotScore ();
		}
		scoreGiven = true;
		float xVector = -400f; // bounce carrrot to the left of the bunny
		float myXCenter = GetComponent<Renderer> ().bounds.center.x;
		// quick and dirty compare, see if the carrot should bounce to the left or the right.
		float distance = myXCenter - otherXCenter;
		if (distance > 0.18) {  // trial and error value.
			xVector = 400; // bounce carrot of the right of the bunny;
			body.AddTorque (-1000f);
		} else {
			body.AddTorque (1000f);
		}

		body.AddForce(new Vector2(xVector,400.5f),ForceMode2D.Impulse);
		Collider2D col = GetComponent<Collider2D> ();
		col.isTrigger = true;
	}
	void OnTriggerEnter2D (Collider2D other)
	{	
		if (triggered == false && other.CompareTag ("Bunny") == true) {
			Bounce (other.gameObject.GetComponent<Renderer> ().bounds.center.x);
			triggered = true;
		}
		if (other.CompareTag ("Water") == true) {
			Collider2D col = GetComponent<Collider2D> ();
			col.isTrigger = false;
			gameObject.tag = "Object";
			body.mass = 60;
		}



	}
	void OnTriggerExit2D (Collider2D other) {


	}

	// Use this for initialization
	void Start () {
 	body = GetComponent<Rigidbody2D> ();
		body.AddTorque (1000f);
		gameObject.tag = "Untagged";
		//body.MoveRotation (360f);
	}

	void OnMouseDown ()
	{
		Bounce (Input.mousePosition.x);
	}


}
