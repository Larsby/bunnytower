using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour {
	public GameObject[] backgroundPrefabs;
	float startPadding = 1.59f;
	float padding = 1.59f;
	float startY = 0;
	public static int level = 0;
	public bool spawn = false;
	public int numberOfSections = 15;
	public GameObject backgroundParent;
	// Use this for initialization
	private int seed = -1;
	GameObject background;
	public GameObject clearSectionTemplate;
	void Start () {
		
		background = GameObject.FindGameObjectWithTag ("Backgrounds");
		CreateLevel (level);
	}

	void SetSeedForLevel(int level) {
		System.DateTime time = System.DateTime.Now;
		int month = time.Month;
		int day = time.Day;

		seed = System.Int32.Parse ("" + month + "" + day + "" + level);
		Random.InitState (seed);
	}

	void CreateLevel(int level) {
		SetSeedForLevel (level);
		int size = background.transform.childCount;
		for (int i = 0; i < size; i++) {
			GameObject child = background.transform.GetChild (i).gameObject;
			child.GetComponent<SectionInfo> ().Generate ();
			if (i == 0) {
				GameObject sectionParent = child.transform.GetChild (0).gameObject;
				if (sectionParent != null) {
						sectionParent.AddComponent<ReportWin> ();
								}
			} else {
				if (i < size - 2) {
					GameObject sectionParent = child.transform.GetChild (0).gameObject;
					if (sectionParent != null) {
						sectionParent.AddComponent<SectionCleared> ();
					}

				}
		//		Debug.Log ("First object in backgrounds must have a gameobject with a child and that child must have a render component in order for us to add report win"+ i + " "+child.name);
			}
		}
		Random.seed = (int)System.DateTime.Now.Ticks; // "reset" random seed.
	}

}
