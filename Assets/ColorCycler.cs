using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorCycler : MonoBehaviour
{
	public Color c1 = new Color (1f, 0.30196078f, 0.30196078f);
	public Color c2 = new Color (0.30196078f, 1f, 0.30196078f);
	public float speed = 0.1f;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		//transform.Rotate (Vector3.back * Time.deltaTime * 10);	

		Color lerpedColor = Color.Lerp (c1, c2, Mathf.PingPong (Time.unscaledTime * speed, 1));
		GetComponent<RawImage> ().color = lerpedColor;

	}
}
