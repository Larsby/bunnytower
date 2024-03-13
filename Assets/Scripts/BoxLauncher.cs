using UnityEngine;
using System.Collections;

public class BoxLauncher : MonoBehaviour
{

	public GameObject[] boxPrefabs;

	public float fireDelay = 3f;
	public float nextFire = 1f;
	public bool flip = false;
	public float fireVelocity = 10f;
	public float xVelocity = 5;
	private static int spawnID = 0;
	public GameObject bunnyStack;
	public DeathTrigger deathTrigger;
	public float standardSize = 0.5f;

	void Awake() {
		spawnID = 0;
	}
	void FixedUpdate ()
	{
			
		if (gameObject.active == false)
			return;
		if (GameManager.instance == null)
			return;
		if (GameManager.instance.IsReady () == false)
			return;
		if (deathTrigger.hasLost) {
			return;
		}
		if (deathTrigger.hasWon) {
			return;
		}

		nextFire -= Time.deltaTime;

		if (nextFire <= 0) {
			// Spawn a new box!
			nextFire = fireDelay;
			Quaternion rot = transform.rotation;


			if (flip) {
				//rot = Quaternion.LookRotation (Vector3.back, Vector3.up);
				//	rot = new Quaternion (rot.x, -160.0f,rot.z, rot.w);
			} else {
				//	rot = Quaternion.LookRotation (Vector3.forward, Vector3.up);
			}
			//rot = Quaternion.RotateTowards ( rot,transform.rotation,100f);
			GameObject boxGO = (GameObject)Instantiate (
				                   boxPrefabs [Random.Range (0, boxPrefabs.Length)], 
				                   transform.position,
				                   rot
			                   );

			Quaternion jumpRotation = transform.rotation;
			if (flip) {
				jumpRotation = Quaternion.Inverse (jumpRotation);
				//	boxGO.GetComponent<Rigidbody2D> ().MoveRotation (v.z);

				boxGO.transform.Rotate (new Vector3 (0.0f, 180.0f, 0.0f));
			} else {
				boxGO.transform.Rotate (new Vector3 (0.0f, 0.0f, -90.0f));
			}
			Vector2 minMax = GameManager.instance.GetBunnyMinMaxSize ();
			float scale = Random.Range (minMax.x, minMax.y);


			boxGO.transform.localScale = new Vector2 (scale, scale);
			float mass = boxGO.GetComponent<Rigidbody2D> ().mass;
			float diff = standardSize - scale;
		
			if (diff < 0) {
				diff = diff * -1;
				mass = diff * 1000;

			} else {
				diff = 1.0f - diff;
				mass = diff * 100;
			}
			//Debug.Log ("scale " + scale + " diff " + diff);
		
			boxGO.GetComponent<Rigidbody2D> ().mass = mass;
			boxGO.GetComponent<Rigidbody2D> ().velocity = jumpRotation * new Vector2 (xVelocity, fireVelocity);
			BunnyBehaviour bunny = null;
			bunny = boxGO.GetComponent<BunnyBehaviour> ();
			GameManager.instance.AddActor (bunny);
			boxGO.name = "Bunny" + spawnID+ " "+boxGO.name;
			boxGO.transform.parent = bunnyStack.transform;
			spawnID++;
			if (bunny != null) {
				bunny.SetIsInverse (flip);
			}
		
		}

	}

}
