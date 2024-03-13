using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

//using Com.LuisPedroFonseca.ProCamera2D;
public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public GameObject cameraMarker;
	public GameObject ground;
	public GameObject center;
	public ScoreManager scoreManager;
	private List<BunnyBehaviour> actors = null;
	private Vector3 highPoint;
	private float highScorePoint = 0.0f;
	private bool gameOver = false;
	private GameObject cameraTarget;
	public float speed = 0.2f;
	float timeSpent = 0.0f;
	public int BunniesToStackForReward = 5;
	private int bunnyStackCounter = 0;
	public GameObject[] rewardObjects;
	public GameObject[] extraRewardObjects;
	public GameObject[] sounds;
	public Text highscoreText;
	public Text highscoreValue;
	int carrotScore;
	public BunnyBehaviour startBunny;

    public Slider positionSlider;

	private  bool soundEnabled = true;
	private bool sfxEnabled = true;
	private int started = 0;
	private int numberOfGamesPlayed = 0;
	private bool showStartedRate = false;
	private Color buttonOriginalColor;
	public  GameObject SettingsPanel;
	public GameObject soundButton;
	public GameObject sfxButton;
	public GameObject restartButton;
	public GameObject settingsButton;
	public StartWeatherSystem weather;
	BunnyBehaviour currentHighestBunny = null;
	public bool DebugCameraFocus = false;
	private bool showBirds = false;
	public PlayRandomSound uiClick;
	public GameObject videoPlane;
	public GameObject UI;
	private bool isready = false;
	private bool playVideo = false;
	private bool fadeCamera = false;
	private VideoPlayer videoPlayer;
	public GameObject UIMultiplier;
	public GameObject SceneBlocker;
	private Text uiMultiplierText;
	public bool debugCanNotDie = false;
	private bool showCredits = false;
	public PlayRandomSound fireworksSFX;

	public GameObject currentClearObj;
	public TextMesh levelText;
	int sectionCleared = 0;
	private bool showIntro;
	public VideoPlayer introVideo;
	private static bool  replay = false;
	public GameObject restartPrefab;

	//public ProCamera2D cam;
	void Awake ()
	{
		
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);    

		replay = false;

	}


	public bool isGameOver ()
	{
		return gameOver;
	}

	public float GetHighestPoint ()
	{
		return highScorePoint;

	}

	public void AddActor (BunnyBehaviour actor)
	{
		if (actors == null) {
			actors = new List<BunnyBehaviour> ();
			highScorePoint = 0;
		} 
		TestHighPoint (actor);
		actors.Add (actor);

	}
	public void SetShowCredits(bool show) {
		showCredits = show;
	}
	public bool ShowCredits() {
		return showCredits;
	}
	public IEnumerator SetReady(float delay) {
		yield return new WaitForSecondsRealtime (delay);
		isready = true;
	}

	public IEnumerator HideSection() {
		yield return new WaitForSecondsRealtime (2.0f);
		currentClearObj.SetActive (false);
		Time.timeScale = 1.0f;
		StartCoroutine(SetReady(2.0f));
			
	}
	public void SectionCleared() {
		isready = false;
		sectionCleared++;
		levelText.text = "Level " + sectionCleared;
	//	Debug.Log ("Section cleared" + sectionCleared);
		currentClearObj.SetActive (true);

	//	Time.timeScale = 0;
		StartCoroutine (HideSection ());

	}
	void LoadSystemPreferences ()
	{
		if (PlayerPrefs.HasKey ("intro")) {
			int times = PlayerPrefs.GetInt ("intro");
			if (times > 3) {
				showIntro = false;
			} else {
				times++;
				PlayerPrefs.SetInt ("intro", times);
				showIntro = true;
			}
		} else {
			showIntro = true;
			PlayerPrefs.SetInt ("intro", 1);
		}
		if (PlayerPrefs.HasKey ("music")) {
			soundEnabled = PlayerPrefs.GetInt ("music") == 1 ? true : false;

		} 

		if (PlayerPrefs.HasKey ("sfx")) {

			sfxEnabled = PlayerPrefs.GetInt ("sfx") == 1 ? true : false;
		} 


		if (PlayerPrefs.HasKey ("started") == false) {
			PlayerPrefs.SetInt ("started", 0);
			started = 1;
		} else {
			started = PlayerPrefs.GetInt ("started");
			started++;
			SavePrefs ();
		}
		if (PlayerPrefs.HasKey ("playedRateShown") == false) {
			PlayerPrefs.SetInt ("playedRateShown", 0);

		}
		if (PlayerPrefs.HasKey ("wonAIRateShown") == false) {
			PlayerPrefs.SetInt ("wonAIRateShown", 0);
		}
		SavePrefs ();
		SetMenuState ();
	}

	public bool PlayStartVideo ()
	{
		if (started == 0) {
			playVideo = true;
		}
		return playVideo;
	}

	public bool DebugDontDie ()
	{
		return debugCanNotDie;
	}

	public void GameOver ()
	{

        if(gameOver== false)
        {
            scoreManager.UpdateScoreOnTheInternet();
        }
            
		gameOver = true;
        scoreManager.SaveScores();
   		ShowRate ();
		SavePrefs ();
		highscoreText.gameObject.SetActive(true);
		highscoreValue.gameObject.SetActive(true);
        if(scoreManager.hs.positionInTheWorld >0.0f)
        {
            positionSlider.gameObject.SetActive(true);
            positionSlider.value = scoreManager.hs.positionInTheWorld;

        }

		if (started % 5 == 0 && started != 0) {
		
			weather.StartWeather ();
		}
		if (SettingsPanel.active == false) {
			restartButton.SetActive (true);
		}
		settingsButton.SetActive (true);
	}

	void PlayUIButtonClick ()
	{
		if (sfxEnabled)
			uiClick.Play ();
	}




	public void Restart ()
	{
		isready = true;
		replay = true;
		PlayUIButtonClick ();
		highscoreText.gameObject.SetActive(false);
        highscoreValue.gameObject.SetActive(false);
        positionSlider.gameObject.SetActive(false);
		BackgroundSpawner.level = 0;
		gameOver = false;
		numberOfGamesPlayed++;
		started++;
		Application.LoadLevel (Application.loadedLevel);
	}

	public void ReportWin ()
	{
		Debug.Log ("You won the level!");
		GameObject.FindObjectOfType<DeathTrigger> ().hasWon = true;
		RewardExplosion (false);
		AscendActors ();
		PlayerPrefs.SetInt ("won", 1);
		PlayerPrefs.Save ();
		GameOver ();

	}

	public void SetSFX (bool b)
	{
	}
	/*
	public void SetSFX (bool b)
	{
		
			foreach (GameObject obj in sfx) {
			if (obj != null)
				obj.GetComponent<AudioSource> ().enabled = b;
		}

	}
*/
	void SetButtonState (GameObject obj, bool active)
	{
		int activeIndex = active == true ? 0 : 1;
		int inactiveIndex = active == true ? 1 : 0;
		Image img = obj.transform.GetChild (inactiveIndex).gameObject.GetComponent<Image> ();
		img.enabled = false;
		img = obj.transform.GetChild (activeIndex).gameObject.GetComponent<Image> ();
		img.enabled = true;
	}

	public void SetPlayModeButton (GameObject obj, bool active)
	{
		PlayUIButtonClick ();
		Image img = obj.GetComponent<Image> ();
		if (active) {
			img.color = new Color (1, 1, 1);
		} else {
			img.color = buttonOriginalColor;
		}
	}

	public void SetMenuState ()
	{

		SetButtonState (soundButton, soundEnabled);
		SetButtonState (sfxButton, sfxEnabled);



		SavePrefs (soundEnabled, sfxEnabled);

	}

	public void SFX ()
	{ 
		PlayUIButtonClick ();
		sfxEnabled = !sfxEnabled;
		SetSFX (sfxEnabled);
		SetMenuState ();
	}

	void SetSound (bool b)
	{
		int randomIndex = Random.Range (0, sounds.Length);
		int i = 0;
		foreach (GameObject obj in sounds) {
			obj.SetActive (b);
			obj.GetComponent<AudioSource> ().enabled = b;
			if (b == true) {
				if (i == randomIndex) {
					obj.GetComponent<AudioSource> ().volume = 0.4f; //0.4 works much better for the volume for the song.
					if (obj.GetComponent<AudioSource> ().enabled) {
						obj.GetComponent<AudioSource> ().Play ();
					}
				}
			} else {
				obj.GetComponent<AudioSource> ().volume = 0.0f;
			}
					i++;
		}
		SetMenuState ();

	}

	private void AscendActors ()
	{
		for (int i = actors.Count - 1; i > 0; i--) {
			Rigidbody2D body = actors [i].gameObject.GetComponent<Rigidbody2D> ();
			if (body != null) {
				body.simulated = true;
				body.gravityScale = -1.0f;
			}
		}
	}


	public  void ToggleMenu ()
	{

		//	return;
		PlayUIButtonClick ();
		SettingsPanel.SetActive (!SettingsPanel.active);
		if (SettingsPanel.active) {
			Time.timeScale = 0;
			restartButton.SetActive (false);
			settingsButton.SetActive (false);

		} else {
			Time.timeScale = 1;
			restartButton.SetActive (gameOver);
			showCredits = false;
			settingsButton.SetActive (true);
		}


		//	LoadLevelObj.GetComponent<ShowMenuOnTouch> ().ToggleMenu ();
	}

	public void Sound ()
	{
		PlayUIButtonClick ();
		soundEnabled = !soundEnabled;
		SetSound (soundEnabled);
		SetMenuState ();
	}

	public void ReturnToMainMenu ()
	{
		SavePrefs ();
		//	LoadLevelObj.GetComponent<LoadGame> ().LoadMenu ();

	}

	public Vector2 GetBunnyMinMaxSize ()
	{
		float min = 0.3f;
		float max = 0.65f;
		if (bunnyStackCounter < 5) {
			min = 0.5f;
			max = 0.5f;
		} else if (bunnyStackCounter < 10) {
			min = 0.45f;
			max = 0.55f;
		} else if (bunnyStackCounter < 15) {
			min = 0.42f;
			max = 0.65f;

		} else if (bunnyStackCounter < 25) {
			min = 0.40f;
			max = 0.60f;
		} else {
			min = 0.39f;
			max = 0.68f;
		}
		return new Vector2 (min, max);
	}
	// Use this for initialization
	public void SetReady() {
		isready = true;

	}
	void StartGame ()
	{
		SceneBlocker.SetActive (false);
	//	isready = false;
		UI.SetActive (true);
		uiMultiplierText = UIMultiplier.GetComponent<Text> ();
		UIMultiplier.SetActive (false);
		//videoPlane.SetActive (false);
		Image img = soundButton.GetComponent<Image> ();
		buttonOriginalColor = img.color;
		carrotScore = 0;
		highPoint = cameraMarker.transform.position;
		AddActor (startBunny);
		if (fadeCamera == false) {
			SetSound (soundEnabled);
		}
		if (started == 0 || started == 1) {
			showBirds = true;
		} else {
			int rand = Random.Range (0, 5);
			if (rand == 0) {
				showBirds = true;
			}
		}
	}
	void IntroEndReached (VideoPlayer vp) {
		introVideo.gameObject.SetActive (false);
	}
	void EndReached (UnityEngine.Video.VideoPlayer vp)
	{	
		
		fadeCamera = true;
		if (soundEnabled == false) {
			SetSound (soundEnabled);
		}
		playVideo = false;
		SceneBlocker.SetActive (false);
	}
	/*
	void VideoPlayerFrameReady(UnityEngine.Video.VideoPlayer vp, long frameIndex) {
		if (frameIndex > 1) {
			//SceneBlocker.SetActive (false);
			videoPlayer.sendFrameReadyEvents = false;
		}
	}
*/
		void PlayVideo ()
	{
		isready = false;
		UI.SetActive (false);
		SetSound (true);
		//videoPlane.SetActive (true);
		videoPlayer = videoPlane.GetComponent<VideoPlayer> ();
		//videoPlayer.sendFrameReadyEvents = true;
		videoPlayer.loopPointReached += EndReached;
		//videoPlayer.frameReady += VideoPlayerFrameReady;
		//	double duration = player.clip.length;
		videoPlayer.Play ();
		//StartCoroutine(StartGameWithDelay(duration));
	}



	public bool IsReady ()
	{
		return isready;
	}

	void Start ()
	{ 
		sfxEnabled = true;
		soundEnabled = true;
		LoadSystemPreferences ();
	//	showIntro = true;
		GameObject restartObj = GameObject.FindGameObjectWithTag ("Restart");
		if (restartObj == null) {
			Instantiate (restartPrefab);

		} else {
			showIntro = false;
			// this is a restarted game so we dont show the intro video.
		}

		if (showIntro == false) {
			isready = true;
		}

		if (showIntro) {
			uiMultiplierText = UIMultiplier.GetComponent<Text> ();
			UI.SetActive (false);

			SetSound (false);
			SetSFX (false);
			introVideo.loopPointReached += IntroEndReached;
			return;
		}
		if (showIntro == false) {
			introVideo.gameObject.SetActive (false);
		}
		if (PlayStartVideo ()) {
			PlayVideo ();
		} else {
			SetMenuState ();
			StartGame ();
		}
	}

	public void AddCarrotScore ()
	{
		if (isGameOver() == false)
		{
			carrotScore += 5;
			RewardExplosion(extraRewardObjects);
		}
	}

	public int GetCarrotScore ()
	{
		return carrotScore;
	}

	public void TodaysBestScore ()
	{
		Debug.Log ("Todays best score!");
		RewardExplosion (extraRewardObjects);
	}

	public void AllTimeHighScore ()
	{
		Debug.Log ("Alltime best score!");
		RewardExplosion (extraRewardObjects);
	}

	public void SavePrefs ()
	{
		SavePrefs (soundEnabled, sfxEnabled);	
	}


	public  void Rate ()
	{

		UniRate r = GameObject.FindObjectOfType<UniRate> ();
		r.ShowPrompt ();
	}

	public void ShowRate ()
	{

		if (numberOfGamesPlayed == 3 && PlayerPrefs.GetInt ("playedRateShown") == 0) {
			PlayerPrefs.SetInt ("playedRateShown", 1);
			PlayerPrefs.Save ();
			Rate ();
			return;
		}

		if ((started == 5 || started == 10) && showStartedRate == false) {
			showStartedRate = true;
			Rate ();
		}

	}

	public bool ShouldStartBirds ()
	{
		return showBirds;
	}

	void SavePrefs (bool music, bool sfx)
	{
		PlayerPrefs.SetInt ("music", music == true ? 1 : 0);
		PlayerPrefs.SetInt ("sfx", sfx == true ? 1 : 0);
		PlayerPrefs.SetInt ("started", started);
		PlayerPrefs.Save ();
	}

	public bool SFXEnabled ()
	{
		if (playVideo)
			return false;
		if (showIntro)
			return false;
		return sfxEnabled;
	}

	void RewardExplosion (GameObject[] prefabs)
	{
		RewardExplosion (prefabs, false);
	}

	IEnumerator SetUIState (float delay, GameObject obj, bool state)
	{
		yield return new WaitForSeconds (delay);
		obj.SetActive (state);
	}


	private IEnumerator FadeInOut (float from, float to)
	{
		float duration = 0.9f;
		float currentTime = 0f;
		while (currentTime < duration) {
			float alpha = Mathf.Lerp (from, to, currentTime / duration);
			uiMultiplierText.color = new Color (uiMultiplierText.color.r, uiMultiplierText.color.g, uiMultiplierText.color.b, alpha);
			currentTime += Time.deltaTime;
			yield return null;
		}
		yield break;
	}

	IEnumerator StartCrossFadeOutWithDelay (float delay)
	{
		yield return new WaitForSeconds (delay);
		StartCoroutine (FadeInOut (1.0f, 0.0f));
		//uiMultiplierText.CrossFadeAlpha (0.9f, 1.0f, false);
//		uiMultiplierText.CrossFadeColor (new Color (1, 1, 1, 0.2f), 2.8f, true, true);
	}

	void RewardExplosion (GameObject[] prefabs, bool showMultiplier)
	{
		GameObject template = prefabs [Random.Range (0, prefabs.Length - 1)];

		GameObject explosion = Instantiate (template);
		explosion.SetActive (true);
		StartStopParticleSystem sp = explosion.GetComponent<StartStopParticleSystem> ();
		sp.Ignite2 (2.2f);
		explosion.transform.position = new Vector3 (0, highPoint.y + 1.5f, template.transform.position.z);
		sp.StartDisabled = false;
		if (showMultiplier) {
			
			StartCoroutine (StartCrossFadeOutWithDelay (1.0f));
			StartCoroutine (SetUIState (1.0f, UIMultiplier, true));
			StartCoroutine (SetUIState (1.8f, UIMultiplier, false));
		}

		if (fireworksSFX != null && sfxEnabled) {
			//fireworksSFX.source = explosion.GetComponent<AudioSource> ();
			fireworksSFX.PlayWithDelay (0.8f);
		}
	}

	void RewardExplosion (bool showMultiplier)
	{
		RewardExplosion (rewardObjects, showMultiplier);
	}

	public int TestHighPoint (BunnyBehaviour actor)
	{
		if (actor == null)
			return -1;
		Vector2 Dist1 = new Vector2 (0, actor.gameObject.transform.position.y);
		Vector2 Dist2 = new Vector2 (0, ground.transform.position.y);
		int countStillActors = 0;

		//float distancebetweenactorandground = Vector2.Distance (actor.gameObject.transform.position, ground.transform.position);


		float distancebetweenactorandground = Vector2.Distance (Dist1, Dist2);

		bool isStill = actor.IsStill ();

		if (isStill && distancebetweenactorandground > highScorePoint) {
			highScorePoint = distancebetweenactorandground;
		}
		if (isStill && actor.gameObject.transform.position.y > highPoint.y && cameraTarget != actor.gameObject) {
			highPoint = actor.gameObject.transform.position;
			timeSpent = 0;
			if (DebugCameraFocus) {
				if (currentHighestBunny != null) {
					currentHighestBunny.HighLightBunny (false);
				}
				currentHighestBunny = actor;
				currentHighestBunny.HighLightBunny (true);
			}
		cameraTarget = actor.gameObject;
		//	cam.RemoveAllCameraTargets ();
		//	cam.gameraTarget (actor.gameObject.transform);

			UpdateCameraToHighestPoint ();
			return -1;
		}
		if (isStill) {
			countStillActors++;
		}
		return countStillActors;
	}

	public void SetHighestPoint ()
	{
		bool clear = false;
		int stackNumber = 1;
		bool staticReachPointSet = false;
		if (actors != null) {
			int countStillActors = 0;
			for (int i = actors.Count - 1; i > 0; i--) {
				BunnyBehaviour actor = actors [i];
				if (staticReachPointSet == true && actor.IsBodyStatic () == true && actor.IsBodySimulated()== true) {
					actor.SetBodySimulation (false);
					//actor.gameObject.GetComponent<Rigidbody2D> ().simulated = false;
				}
				if (i>2 && actor.IsBodyStatic () && actor.IsBodySimulated() == true) {
					// we found the first stacked static body now we set staticreachpoint so following bunnies are not simulated
					staticReachPointSet = true;
					actor.SetBodySimulation (false);

				}
				//foreach (BunnyBehaviour actor in actors) {
				int result = TestHighPoint (actor);
				if (result == -1)
					result = 0;
				result += result;
				if (actor.IsStacked ()) {
					stackNumber++;
				}
			}
			if (stackNumber % BunniesToStackForReward == 0 && stackNumber > 0 && stackNumber != bunnyStackCounter) {
				bunnyStackCounter = stackNumber;
				uiMultiplierText.fontSize = 150;
				uiMultiplierText.text = "" + bunnyStackCounter;
				uiMultiplierText.color = new Color (uiMultiplierText.color.r, uiMultiplierText.color.g, uiMultiplierText.color.b, 1.0f);

				RewardExplosion (true);
			}

			if (clear) {
				//actors.Clear ();
			}

		}

	}

	void CenterCamera ()
	{
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));
		Vector3 pos = ray.GetPoint (0);
		cameraMarker.transform.position = pos;
	}

	void UpdateCameraToHighestPoint ()
	{
		Vector3 pos = cameraMarker.transform.position;
		timeSpent += Time.deltaTime * speed;
		if (center.transform.position.y > pos.y) {
			pos.y = center.transform.position.y;
		}
		pos.y = Mathf.Lerp (cameraMarker.transform.position.y, highPoint.y + 1.8f, Time.deltaTime * 2);

		cameraMarker.transform.position = pos;
	

	}

	void FixedUpdate ()
	{
		/*
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if(hit.collider != null)
		{
			Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
		}
		*/
		if (gameOver == false && isready) {
			SetHighestPoint ();
			UpdateCameraToHighestPoint ();
		}

	}
	// Update is called once per frame
	void Update ()
	{
		highscoreValue.text = "" + scoreManager.GetHighScore();
		if (showIntro && Input.GetMouseButtonUp(0)) {
			showIntro = false;
			UI.SetActive (true);
			introVideo.gameObject.SetActive (false);
			isready = true;

			StartGame ();


		}
		if (showIntro == false) {
			if(introVideo != null)
			introVideo.gameObject.SetActive (false);
		}
		
		if (fadeCamera) {
			videoPlayer.targetCameraAlpha -= 0.05f;
			if (videoPlayer.targetCameraAlpha <= 0) {
				videoPlayer.Stop ();
				StartGame ();
				fadeCamera = false;
			}
		}
		if (showIntro == false && UIMultiplier.active) {
			uiMultiplierText.fontSize++;
		}
	}
}
