using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionInfo : MonoBehaviour {
	public int sectionLevel = 0;
	public GameObject [] variants;

	// Use this for initialization
	void Start () {


	}
	public void Generate() {
		int sectionIndex = Random.Range (0, variants.Length); 
		GameObject section = Instantiate(variants[sectionIndex]);

		section.transform.position = transform.position;
		section.transform.parent = transform;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
