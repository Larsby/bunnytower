using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	public bool useSpring = false;

	public LineRenderer dragLine;
	public GameObject explosion;
	Rigidbody2D grabbedObject = null;
	GameObject grabbedGameObject = null;
	SpringJoint2D springJoint = null;
	private float mouseDownTime;
	float velocityRatio = 4f; 	// If we aren't using a spring

	void Update() {
		if( Input.GetMouseButtonDown(0) ) {
			mouseDownTime = Time.time;
			// We clicked, but on what?
			Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector2 mousePos2D = new Vector2(mouseWorldPos3D.x, mouseWorldPos3D.y);

			Vector2 dir = Vector2.zero;

			RaycastHit2D hit = Physics2D.Raycast(mousePos2D, dir);
			if(hit.collider!=null) {
				// We clicked on SOMETHING that has a collider
				grabbedObject = null;
				if(hit.collider.GetComponent<Rigidbody2D>() != null && hit.collider.gameObject.CompareTag("Head") == true){
					grabbedObject = hit.collider.gameObject.transform.parent.GetComponent<Rigidbody2D> ();
				} else if(hit.collider.GetComponent<Rigidbody2D>() != null && (hit.collider.gameObject.CompareTag("Bunny") == true || hit.collider.gameObject.CompareTag("Object") == true)) {
					grabbedObject = hit.collider.GetComponent<Rigidbody2D>();

				}
					if(grabbedObject != null) {

					if(useSpring) {
						springJoint = grabbedObject.gameObject.AddComponent<SpringJoint2D>();
						// Set the anchor to the spot on the object that we clicked.
						Vector3 localHitPoint = grabbedObject.transform.InverseTransformPoint(hit.point);
						springJoint.anchor = localHitPoint;
						springJoint.connectedAnchor = mouseWorldPos3D;
						springJoint.distance = 0.25f;
						springJoint.dampingRatio = 1;
						springJoint.frequency = 5;

						// Enable this if you want to collide with objects still (and you probably do)
						// This will also WAKE UP the spring.
						springJoint.enableCollision = true;

						// This will also WAKE UP the spring, even if it's a totally
						// redundant line because the connectedBody should already be null
						springJoint.connectedBody = null;
					}
					else {
						// We're using velocity instead
						grabbedObject.gravityScale=0;
					}


					dragLine.enabled = true;
				
				if (grabbedObject.CompareTag ("Bunny") == true) {
						BunnyBehaviour bh = grabbedObject.GetComponent<BunnyBehaviour>();
						if (bh != null) {
							bh.Hold ();
							bh.stabilize = true;
						}
						mouseDownTime = 0; 
				}
				}
			}
		}

		float pressTime = Time.time - mouseDownTime;
		if (pressTime > 0.5f && GameManager.instance.isGameOver ()) {
			/*
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			explosion.transform.position = pos;

		//	Debug.Log ("" + pressTime);
			Explode exploder = explosion.GetComponent<Explode> ();
			exploder.explode = true;
			exploder.ExplosionRadius = Mathf.Lerp (3, 10, pressTime - 1.0f);
			*/
		
	}
		if( Input.GetMouseButtonUp(0) && grabbedObject!=null ) {
			
			if(useSpring) {
				Destroy(springJoint);
				springJoint = null;
			}
			else {
				grabbedObject.gravityScale=1;
			}
			if (grabbedGameObject != null && grabbedObject.CompareTag ("Bunny") == true) {
				BunnyBehaviour bh = grabbedObject.GetComponent<BunnyBehaviour>();
				if (bh != null) {
					bh.Fly ();
					bh.stabilize = false;
				}

			}
			grabbedObject = null;
			grabbedGameObject = null;
			dragLine.enabled = false;
		} 

	}


	void FixedUpdate () {
		if(grabbedObject != null) {
			Vector2 mouseWorldPos2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if(useSpring) {
				springJoint.connectedAnchor = mouseWorldPos2D;
			}
			else {
				grabbedObject.velocity = (mouseWorldPos2D - grabbedObject.position) * velocityRatio;
			}
		}
	}

	void LateUpdate() {
		if(grabbedObject != null) {
			if(useSpring) {
				Vector3 worldAnchor = grabbedObject.transform.TransformPoint(springJoint.anchor);
				dragLine.SetPosition(0, new Vector3(worldAnchor.x, worldAnchor.y, -1));
				dragLine.SetPosition(1, new Vector3(springJoint.connectedAnchor.x, springJoint.connectedAnchor.y, -1));
			}
			else {
				Vector3 mouseWorldPos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				dragLine.SetPosition(0, new Vector3(grabbedObject.position.x, grabbedObject.position.y, -1));
				dragLine.SetPosition(1, new Vector3(mouseWorldPos3D.x, mouseWorldPos3D.y, -1));
			}
		}
	}

}
