using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBubbleOnCollide : MonoBehaviour
{
	public GameObject splash;

	IEnumerator RemoveFromScene (GameObject obj)
	{
		yield return new WaitForSeconds (2.0f);
		Destroy (obj);
	}

	void DoSplash (GameObject obj)
	{
		GameObject spl = Instantiate (splash);
		//spl.GetComponent<StartStopParticleSystem> ().Ignite (2f);
		spl.GetComponent<PlayRandomSound> ().Play ();
		spl.transform.position = new Vector3 (obj.transform.position.x, splash.transform.position.y, 0.0f);
		StartCoroutine (RemoveFromScene (spl));
		spl.transform.parent = gameObject.transform;
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.CompareTag ("Bunny") == true) {
			BunnyBehaviour bh = other.gameObject.GetComponent<BunnyBehaviour> ();
			if (bh != null) {
				bool splashAlready = bh.splash;
				if (splashAlready == false) {
					DoSplash (other.gameObject);
					bh.splash = true;
				} else {
					DoSplash (other.gameObject);
				}
			} 
			//spl.transform.parent = other.gameObject.transform;
		} else if (other.CompareTag ("Object") == true) {
			DoSplash (other.gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
