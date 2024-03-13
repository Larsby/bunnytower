using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class SectionCleared : MonoBehaviour {
		private bool reportedSection = false;
	public GameObject clearAnimObject;
		// Use this for initialization
		void Start () {
		reportedSection = false;
		//clearAnimObject.SetActive (false);
			BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		Vector2 colliderSize = Vector2.zero;
			if (collider == null) {
				Renderer render = GetComponent<Renderer> ();
			if (GetComponent<Renderer> () == null) {
				colliderSize = new Vector2 (19.3f, 18.5f);
				//return;
			} else {
				colliderSize = render.bounds.size;
			}
				gameObject.AddComponent<BoxCollider2D> ();
				collider = GetComponent<BoxCollider2D> ();
			collider.size = colliderSize;
				collider = GetComponent<BoxCollider2D> ();
				collider.isTrigger = true;

			}

		}
		void CheckSection(Collider2D other) {
			if (other.CompareTag ("Bunny") == true) {
				BunnyBehaviour bh = other.gameObject.GetComponent<BunnyBehaviour> ();

				if (bh != null && bh.IsStill ()) {
				if (bh.IsHolding () == false && reportedSection == false) {
					GameManager.instance.SectionCleared ();
						reportedSection = true;
					}
				}
			}
		}
		void OnTriggerStay2D(Collider2D other)
		{
		CheckSection (other);
		}
		void OnTriggerEnter2D (Collider2D other) {

		CheckSection (other);	
		}


	}