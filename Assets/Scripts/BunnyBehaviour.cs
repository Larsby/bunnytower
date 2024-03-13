using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
	public RuntimeAnimatorController[] animators;
	public Sprite[] sprites;
	private bool hold = false;
	private bool release = false;
	Rigidbody2D rigidBody;
	private bool inverse = false;
	public float timeToAdjust = 1.0f;
	public bool stabilize = false;
	float currentheight;
	float previousheight = 0f;
	float travel = -1f;
	public bool splash = false;
	private bool isStatic = false;
	private bool mouseisdown = false;
	private bool still = false;
	private float distanceFromCameraBeforeGoingStatic = -0.01f; // -0.01f is just outside of camera -0.2 = 8 visible not locked -0.4 = 11
	private bool rotateInWater = false; 
	private bool stacked = false;
	public Color[] colors;
	private PlayRandomSound[] sfx;
	public GameObject sfxSounds;
	private bool touchingGround = false;
	public bool debugMe = false;
	private bool placedNearFrameEnd;
	private Transform bunnyStackParent;
	private bool actuallyFalling;
	private GameObject left;
	private GameObject right;
	private GameObject top;
	private GameObject bottom;
	enum SFX_INDEX {
		FLY = 0,
		HOLD = 1,
		IDLE = 2,
	};

	public bool isOnGround() {
		return touchingGround;
	}
	public bool IsBodyStatic() {
		return isStatic;
	}
	public bool IsBodySimulated() {
		return rigidBody.simulated;
	}
	public void SetBodySimulation(bool simulate) {
		if (touchingGround == true) {
			rigidBody.simulated = simulate;
		}
	}
	public bool HaveBunnyOnTop() {
		return !top == null;
	}
	// Use this for initialization
	void Start ()
	{
		top = null;
		bottom = null;
		rigidBody = GetComponent<Rigidbody2D> ();
		actuallyFalling = true;
		//transform.GetChild (0).rotation = transform.rotation;
		//	Debug.Break ();
		GameObject sfxInstance = Instantiate(sfxSounds);
		sfxInstance.transform.parent = transform;
		sfx = sfxInstance.GetComponents<PlayRandomSound>();
		sfx [0].source = sfxInstance.GetComponent<AudioSource> ();
		if (colors.Length != 0) {
			Color c = new Color (1, 1, 1);
			//SetSpriteColorOnChildren (gameObject, c);
			Fly ();
		}

	}



	public GameObject GetGameObj() {
		return gameObject;
	}
	public void HighLightBunny(bool color) {
		Color c = new Color (1, 1, 1);
		if (color)
			c = new Color (1, 0, 0f);

		SetSpriteColorOnChildren (gameObject, c);

	}

	public void SetSpriteColorOnChildren (GameObject parent, Color c)
	{
		SpriteRenderer ren = parent.GetComponent<SpriteRenderer> ();
		if (ren != null) {
			ren.color = new Color (c.r, c.g, c.b, 1.0f);
			;
		}
		foreach (Transform t in parent.transform) {

			SetSpriteColorOnChildren (t.gameObject, c);
		}
	}

	public void SetIsInverse (bool inverse)
	{
		this.inverse = inverse;
	}
	IEnumerator PlayFlySoundDelayed() {
		yield return new WaitForSeconds (0.6f);
		int index = (int)SFX_INDEX.FLY;
		sfx [index].Play ();
	}
	public void Fly ()
	{
		//		GetComponent<SpriteRenderer> ().sprite = sprites [0];
		GetComponent<Animator> ().runtimeAnimatorController = animators [0];
		StartCoroutine (PlayFlySoundDelayed ());
	}

	public bool IsStill ()
	{

		if (release == false)
			return false;	
		if (mouseisdown)
			return false;
		if (placedNearFrameEnd)
			return false;

		return IsStacked ();
	}

	void OnCollisionEnter2D (Collision2D coll)
	{

		if (coll.gameObject.CompareTag ("Platform") == true) {
			touchingGround = true;
			bottom = coll.gameObject;
		}



		release = true;
	}
	public bool IsStacked() {
		if (actuallyFalling)
			return false;
		return stacked;
	}



	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.CompareTag ("Platform") == true) {
			touchingGround = false;
			//	actuallyFalling = true;
			return;
		}

		if (coll.gameObject.CompareTag ("Bunny") == true) {
			Rigidbody2D coliderBody = coll.gameObject.GetComponent<Rigidbody2D> ();
			if (coliderBody != null && coliderBody.simulated == false ) {
				//stacked = true;

				if (coll.gameObject == bottom || top != null) {
                    SetToStatic();
					stacked = true;
					Debug.Log ("Setting myself " + name + " to static");
					return;
				}

                SetToStatic();
                Debug.Log (" collider under is not simulated" + coll.gameObject.name + "name " + gameObject.name + " " + top + "!!!!");
                //		actuallyFalling = false;
                return;

            } else {

				//	stacked = false;
			}
			if (coll.gameObject == top)
				stacked = false;
			if (coll.gameObject.transform.position.y > transform.position.y) {
				//	actuallyFalling = true;
			}

		}
	}

	void OnCollisionStay2D (Collision2D coll)
	{

		if (coll.gameObject.CompareTag ("Bunny") == true) {
			ContactPoint2D p = coll.contacts [0];
			//stacked = true;
			//actuallyFalling = false;
			if (p.point.y > 0.0f) {
				if (p.point.x > -0.5f && p.point.x < 0.5f) {
					top = coll.gameObject;
					stacked = true;
				}
			}
			if (p.point.y <= 0.0f) {
				if (p.point.x > -0.5f && p.point.x < 0.5f) {
					bottom = coll.gameObject;

				}
			}
			//	Debug.Log ("Debug point" + p.point);
			//	Vector2 normal = coll.contacts [0].normal;
			//	Debug.Log ("collider name" + coll.gameObject.name + " x " + normal.x + " y "+ normal.y);

		}
		if (coll.gameObject.CompareTag ("Platform") == true) {
			touchingGround = true;
			bottom = coll.gameObject;
			//actuallyFalling = false;
		}

	}
	public void Hold ()
	{
		if (hold == false) {
			StartStopParticleSystem ps = GetComponent<StartStopParticleSystem> ();
			if (ps != null) {
				ps.Extinguish ();
			}
			SetKinematicStateOnHead (true);
			//GetComponent<SpriteRenderer> ().sprite = sprites [1];
			GetComponent<Animator> ().runtimeAnimatorController = animators [1];
			hold = true;
		}

	}




	public void ReleaseState ()
	{
		if (hold) {

			SetKinematicStateOnHead (false);

			//	GetComponent<SpriteRenderer> ().sprite = sprites [2];
			GetComponent<Animator> ().runtimeAnimatorController = animators [2];
			hold = false;
			release = true;
		}
		if (!hold) {
			//	GetComponent<SpriteRenderer> ().sprite = sprites [2];
			GetComponent<Animator> ().runtimeAnimatorController = animators [2];
		}
	}

	void SetKinematicStateOnHead (bool active)
	{
		gameObject.transform.GetChild (0).gameObject.GetComponent<Rigidbody2D> ().isKinematic = active;

	}

	public bool IsHolding () {
		return hold;
	}
	void OnMouseDown ()
	{
		bunnyStackParent = transform.parent;
		transform.parent = null;
		if (mouseisdown == false) {
			if (sfx.Length - 1 < (int)SFX_INDEX.HOLD) {
				return;
			} else {
				sfx [(int)SFX_INDEX.HOLD].Play ();
			}
		}
		if ( rigidBody != null && rigidBody.simulated == false) {
			//	SetSim ();
		}
		Hold ();
		hold = true;

		mouseisdown = true;

	}

	public IEnumerator JoinStack() {
		yield return new WaitForSeconds (0.25f);
		transform.parent = bunnyStackParent;
	}

	void OnMouseUp ()
	{

		ReleaseState ();
		StartCoroutine (JoinStack());
		if (sfx.Length - 1 < (int)SFX_INDEX.IDLE) {
			return;
		} else {
			sfx [(int)SFX_INDEX.IDLE].Play ();
		}
		hold = false;
		mouseisdown = false;

		previousheight = transform.position.y;
		if (NearEndOfFrame (0.1f)) {
			placedNearFrameEnd = true;
		} 

	}
	public bool JustOutOfFrame() {
		if (actuallyFalling)
			return false;
		Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
		if (viewPos.y < -0.06f) {
			return true;
		} 
		return false;
	}
	public bool NearEndOfFrame() {
		float half = transform.localScale.y / 2;
		return NearEndOfFrame (0.09f);
	}
	private bool NearEndOfFrame(float distance) {
		if (actuallyFalling)
			return false;
		Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
		if (viewPos.y <distance) {
			// tested many different numbers. The bunny is howing a little bit but not to much. 0.5 does not sink the pile if you have 3 rows of bunnies.
			// where 0.1 did and 0.02 would sink 1 -2 rows a little.
			return true;
		}
		return false;
	}
	public bool isVisible() {

		Vector3 viewPos = Camera.main.WorldToViewportPoint (transform.position);
		if (debugMe) {
			Debug.Log ("Distance from camera" + viewPos.y);
		}
		if (viewPos.y < distanceFromCameraBeforeGoingStatic ) {

			return false;

		} 
		return true;
	}
	public void SetToStatic() {

		if (rigidBody == null) {
			rigidBody = GetComponent<Rigidbody2D> ();
		}
		if (rigidBody != null) {
			rigidBody.bodyType = RigidbodyType2D.Static;
			isStatic = true;
		}
	}
	public void SetSim() {
		if (rigidBody == null) {
			rigidBody = GetComponent<Rigidbody2D> ();
		}
		if (rigidBody != null) {
			rigidBody.simulated = true;
		}
	}
	public void SetNoSim() {
		if (rigidBody == null) {
			rigidBody = GetComponent<Rigidbody2D> ();
		}
		if (rigidBody != null) {
			rigidBody.simulated = false;
			isStatic = true;
		}
	}
	void  SetPhysicBodyToStatic () {
		if (touchingGround == false) {
			isStatic = true;
			rigidBody.bodyType = RigidbodyType2D.Static;
		}

	}
	void SetPhysicsBodyNormal (){
		isStatic = false;
		if (rigidBody == null) {
			rigidBody = GetComponent<Rigidbody2D> ();
		}
		rigidBody.simulated = true;

		rigidBody.bodyType = RigidbodyType2D.Dynamic;


	}


	public void RotateToUpright() {
		rotateInWater = true;
	}
	public void FreeSpin()
	{
		rotateInWater = false;

	}
	bool ActuallyFalling() {
		return actuallyFalling;
	}
	void LateUpdate() {

	}
	// Update is called once per frame
	void Update ()
	{

		currentheight = transform.position.y;
		float travel = currentheight - previousheight;
		if (travel > 0) {
			//Debug.Log ("Travel" + travel.ToString ());
			actuallyFalling = true;
		} else {
			actuallyFalling = false;
		}
		//Debug.Log ("rigidBody.velocity.magnitude == " + rigidBody.velocity.magnitude.ToString ("F4") + " v" + rigidBody.velocity.ToString ("F4"));
		previousheight = currentheight;
		//IsStill ();
		if (rigidBody.velocity.magnitude == 0 && hold == false) {
			ReleaseState ();
		}
		bool visible= isVisible ();

		bool gameOver = false;
		if (GameManager.instance != null) {
			gameOver = GameManager.instance.isGameOver ();
		}
		if (gameOver  && isStatic == true) {
			SetPhysicsBodyNormal ();
		}
		if (gameOver == false && visible == false && isStatic == false) {

			//	SetPhysicBodyToStatic ();
		}


		if (rotateInWater || (stabilize && gameOver == false )) {
			Quaternion q;
			if (inverse) {
				q = Quaternion.AngleAxis (180, new Vector3 (0, 180, 1f));
			} else {
				Vector3 rotationDirection = Vector3.back;

				q = Quaternion.AngleAxis (0, rotationDirection);
			}
			transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * timeToAdjust);
		}
	}
}
