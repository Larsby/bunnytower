using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCarrots : MonoBehaviour {
	public GameObject prefab;
	bool run = true;
	public int minWaitInSec= 10;
	public int maxWaitInSec = 40;
	// Use this for initialization
	void Start () {
		StartCoroutine (InstantiatePrefab(Random.Range (minWaitInSec, maxWaitInSec)));
	}
	IEnumerator InstantiatePrefab(int delay) {
		yield return new WaitForSeconds (delay);
		if (run) {
			if (GameManager.instance.IsReady () == true) {
				Quaternion rot = prefab.transform.rotation;
				Vector3 pos = new Vector3 (Random.Range (transform.position.x - 0.2f, transform.position.x + 0.2f), transform.position.y, 0.0f);
				Instantiate (prefab, pos, rot);
			}
			StartCoroutine (InstantiatePrefab(Random.Range (minWaitInSec, maxWaitInSec)));
		}
	}
	// Update is called once per frame
	void Update () {
		if (GameManager.instance.IsReady () == false)
			return;
		if (GameManager.instance.isGameOver ()) {
			run = false;
		}
	}
}
