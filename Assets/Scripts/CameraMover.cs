using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{

	public float targetY = 0;
	public GameObject target;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Bunny") == true) {
			BunnyBehaviour bh = other.gameObject.GetComponent<BunnyBehaviour> ();

			if (bh != null && bh.IsStill ()) {	
				Vector3 pos = transform.position;
				//if (target != null) {
				//	targety = target.transform.position.y;
				//}

				targetY = other.gameObject.transform.position.y;
				pos.y = Mathf.Lerp (transform.position.y, targetY+0.5f, 5 * Time.deltaTime);
				if (pos.y >= -0.5f) {
					transform.position = pos;
				}
			}
	}
	}
	// Update is called once per frame
	void Update ()
	{
		//float targety;// = transform.position.y;


	}
}
