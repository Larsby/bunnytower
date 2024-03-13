using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInMiddle : MonoBehaviour {
	float centerX;
	float centerY;
	// Use this for initialization
	void Start () {
		centerX = Screen.width / 2;
		centerY = Screen.height / 2;
	}
	
	// Update is called once per frame
	void Update () {

	//	Debug.Log(""+Camera.main.transform.localPosition.y);
		Ray ray =Camera.main.ViewportPointToRay(new Vector3(0.5f,0.89f,0f));
		Vector3 pos = ray.GetPoint (0);
		transform.position = pos;
		//transform.position = Camera.main.ScreenToViewportPoint(new Vector3(centerX,centerY,0.0f));
	}
}
