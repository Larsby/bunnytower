using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoomer : MonoBehaviour {
	public GameObject target;
	public GameObject bushToLeft;
	public bool zoomToTarget = false;
	public GameObject platform;
	public Camera cam;
	private GameObject whale;
	public GameObject info;

	// Use this for initialization
	bool recalc = false;
	void Start () {
		zoomToTarget =  false;
		 recalc = false;
	}
	public void DoAction() {
		if (zoomToTarget == false) {
			zoomToTarget = true;
			recalc = true;
		}
	}
	IEnumerator TipPlatform() {
		whale = GameObject.FindGameObjectWithTag ("Whale");
		yield return new WaitForSeconds (1);
		//platform.SetActive (false);
		platform.transform.Rotate(new Vector3(0f,0.0f,15f));
//		iTween.MoveBy (whale, new Vector3 (0.0f, -1.0f, 0.0f), 1.0f);
		iTween.MoveBy(whale,iTween.Hash("amount",new Vector3 (0.0f, -1.0f, 0.0f),"time",1.0f,"easetype",iTween.EaseType.easeInOutSine));
		iTween.ScaleBy (whale, iTween.Hash("amount",new Vector3 (0.98f, 0.98f, 0.0f),"time",1.0f,"easetype",iTween.EaseType.easeInOutSine));
		//whale.transform.Rotate (new Vector3 (0f, 0.0f, 15f));
	}
	IEnumerator RemovePlatform() {
		yield return new WaitForSeconds (1.3f);
		platform.SetActive (false);
	//	platform.transform.Rotate(new Vector3(0f,0.0f,15f));
	}


	public void HideAd() {
		info.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		if (zoomToTarget && recalc) {
			Vector3 viewPos = cam.WorldToViewportPoint (target.transform.position);

			if ((viewPos.x < 0.0f || viewPos.x > 0.995f) || (viewPos.y < 0.05f || viewPos.y > 0.995f)) {
				//cam.orthographicSize += 0.08f;
				if (recalc) {
					cam.transform.position = new Vector3 (cam.transform.position.x, cam.transform.position.y - 0.1f, cam.transform.position.z);
				}
				recalc = true;
				if (cam.transform.position.y <= 0) {
					recalc = false;
					StartCoroutine (TipPlatform());
					StartCoroutine (RemovePlatform());
					info.SetActive (true);
				}
				//Camera.main.fieldOfView +=0.08f;

				if (bushToLeft != null) {
					Vector3 pos = bushToLeft.transform.position;
					if (pos.x < -20.40f) {
						bushToLeft.transform.position = new Vector3 (pos.x + 0.08f, pos.y, pos.z);
					}

				}
			} else {
				if (recalc) {
					recalc = false;

					//Camera.main.transform.parent = null;
				} 	
			} 


		}
	}
}
