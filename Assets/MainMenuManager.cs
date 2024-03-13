using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Facebook.Unity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		socialstuff ();
	}
	private int HighScore;
	void socialstuff ()
	{
	//	Facebook.Unity.FB.Init ();
	}

	public void Rate ()
	{

		UniRate r = GameObject.FindObjectOfType<UniRate> ();
		r.ShowPrompt ();
	}

	public void MoreFromPastille ()
	{
		Application.OpenURL ("http://www.pastille.se");
	}

	public void Share ()
	{
		/*
		Facebook.Unity.FB.FeedShare ("",
			new System.Uri ("http://www.pastille.se/"),
			"Get "+Application.productName,
			"Join me and get "+Application.productName,
			"Can you beat my high score of " + HighScore + "?",
			null, null);
*/
	}

	public void LoadGame() {
		SceneManager.LoadScene (1);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
