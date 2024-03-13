using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {
	CircleCollider2D collider;
	public bool explode = false;
	float currentTime = 0f;
	public float timeToMove = 0.5f;
	public float ExplosionRadius = 8.0f;
	// Use this for initialization
	void Start () {
		collider = GetComponent<CircleCollider2D> ();
		collider.radius = 0.0f;
		collider.enabled = false;
	}

	public void DoExplosion() {
		if (currentTime <= timeToMove) {
			currentTime += Time.deltaTime;
			collider.radius = Mathf.Lerp (0.0f, ExplosionRadius, currentTime / timeToMove);
		} else {
			currentTime = 0;
			collider.radius = 0.0f;
			explode = false;
			collider.enabled = false;
		}


	}
	// Update is called once per frame
	void FixedUpdate () {
		if (explode) {
		//	collider.enabled = true;
		//	DoExplosion ();

		} 
	}
}
