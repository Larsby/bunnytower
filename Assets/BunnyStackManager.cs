using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyStackManager : MonoBehaviour {
	bool init = false;
	private int startIndex = 0;
	private int endIndex = 0;
	// Use this for initialization
	void TurnoffSimulationOnHiddenChildren() {

		// Idea is to turn off physics simulation on bunnies that we can't see.
		// This is crucial on mobile bc after a big stack the physics2d engine cant keep up and framerate jitters

		// go through the bunny stack backwards (newest first)
		// don't touch bunnies that are clearly visible.
		// when we reach bunnies that are somewhat visible we make them static so the dynamic bunnies on top have something 
		// to stand on. When we reach the bunnies that are completely hidden we just take them out of the physics simulation.
		// 
		// Since we run this method every frame we make sure that we only go over bunnies we have not covered yet.
		// go backwards until we reach the endIndex and the endIndex goes higher and higher as the stack goes higher.

		if (GameManager.instance != null && GameManager.instance.isGameOver () == false) {
			int childCount = transform.childCount;
			int tobeMadeStaticIndex = -1;

			if (childCount == 0)
				return;
			int start = childCount - 1;
			int end = -1;

			if (start >= 0) { 
				for (int i = start; i >= endIndex; i--) {
					BunnyBehaviour b = transform.GetChild (i).GetComponent<BunnyBehaviour> ();
					if (b.NearEndOfFrame () && b.isVisible () == true && b.IsHolding() == false && b.IsStill() && b.IsStacked()) {
						b.SetToStatic ();
						b.SetSim ();

					} 
					if (b.isVisible () == false) {
						if (end == -1) {
							end = i;
						}
						b.SetNoSim ();
					}
					

				}
			}
			if (end > -1) {
				endIndex = end;
			}
			return;

		}		
	}
	void Start () {
		init = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.instance != null && GameManager.instance.debugCanNotDie == false && init == false) {
			TurnoffSimulationOnHiddenChildren ();
		}
	}
}
