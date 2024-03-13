using UnityEngine;
using System.Collections;

public class DeathTrigger : MonoBehaviour
{

	public bool hasLost = false;
	public bool hasWon = false;
	public GameObject ground;

	void OnTriggerEnter2D (Collider2D other)
	{
		if(GameManager.instance.DebugDontDie()) 
			return;

		if (other.CompareTag ("Bunny") == true) {
			Vector3 viewPos = Camera.main.WorldToViewportPoint (ground.transform.position);
			//Debug.Log ("viewPos for ground " + viewPos.y);
			if(viewPos.y > 0) {
			//	Debug.Log ("Still see the ground!!");
				//return;
			}

			Rigidbody2D rigidbody = other.GetComponent<Rigidbody2D> ();
			//Debug.Log (rigidbody.velocity);
			if (rigidbody.velocity.y < -1.0f) {
				hasLost = true;
				GetComponent<Zoomer> ().DoAction ();
				GameManager.instance.GameOver ();

			}
		}
	}

}
