using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static int score = 0;
	public float distance = 0;
	public GameObject start;
	public GameObject current;
    public HSController hs;
	public RectTransform scoreRect;
	public GameObject SFX;
	private PlayRandomSound sfxPlayer;
	Text scoreText;
	int slowCount = 15;

	int maxScore = 0;
	int privScore = 0;
	private int AllTimeHighScore = 0;
	private int TodaysBestScore = 0;
	private bool CelebratedTodaysBest = false;
	private bool CelebratedAllTimeHigh = false;
	float oldDistance = 0;
	string GetTodayAsHashKey() {
		 System.DateTime time = System.DateTime.Now;

		int year = time.Year;
		int month = time.Month;
		int day = time.Day;

		return "" +year+""+ month + "" + day; 
	}
	void LoadScores() {
		if (PlayerPrefs.HasKey ("HighScore")) {
			AllTimeHighScore = PlayerPrefs.GetInt ("HighScore");
		} else {
			PlayerPrefs.SetInt ("HighScore", 0);
		}
		string today = GetTodayAsHashKey ();
		if (PlayerPrefs.HasKey (today)) {
			TodaysBestScore = PlayerPrefs.GetInt (today);
		} else {
			PlayerPrefs.SetInt (today, 0);
		}
		PlayerPrefs.Save ();

	}
	public int GetHighScore() {
		if (privScore > AllTimeHighScore) { AllTimeHighScore = privScore;SaveScores(); }
		return AllTimeHighScore;
	}
	void DeletePrefs() {
		PlayerPrefs.DeleteAll ();
	}
	public void SaveScores() {
		string today = GetTodayAsHashKey ();
		PlayerPrefs.SetInt (today, TodaysBestScore);
		PlayerPrefs.SetInt ("HighScore", AllTimeHighScore);
		PlayerPrefs.Save();

      
	}

	public int GetScore() {
		return  (int)(distance * 100);
	}
	void Start ()
	{

		maxScore = 0;
		scoreText = scoreRect.GetComponent<Text> ();
		CelebratedTodaysBest = false;
		CelebratedAllTimeHigh = false;
		LoadScores ();
		sfxPlayer = SFX.GetComponent<PlayRandomSound> ();

        hs.setScoreAndUpload(AllTimeHighScore); //this uploads the best score you have to the internet. It also checks your position in the universe
       
	}

	public IEnumerator AddOneScore (float delay)
	{  //pulsateScore = true;
		yield return new WaitForSeconds(delay);

		scoreText.text = "" + ++privScore;
		//	Invoke ("SetColor", 0.1f);
	//	Invoke ("TurnOfPulsate", 0.3f);
		sfxPlayer.Play();
	}

	public void AddScore (int nscore)
	{ 
		int value = nscore;
	
	

		float time = 0;
		float min = 0.5f;
		float max = 1.0f;
		for (int i = 0; i <= value; i++) {
			float slowdown = 0;
			if (slowCount < value) {
				if (value - i < slowCount) {
					slowdown = Random.Range (0.5f, 1.2f);
				}
			}
			if (i < 10) { 
				max = 0.2f;
				min = 0.0f;
			} else {
			min = 0.5f;
         max = 1.0f;
			}
			float randomTime = Random.Range (slowdown +min, slowdown + max);


			//Pulsate (index, i % 9);
			time += randomTime;
			StartCoroutine (AddOneScore (randomTime));

		}




	}

    public void UpdateScoreOnTheInternet()
    {
        if(AllTimeHighScore == privScore)
        {
            hs.setScoreAndUpload(AllTimeHighScore);
        }

    }

	public void Update ()
	{
		if (GameManager.instance) {
			oldDistance = distance;
			distance = GameManager.instance.GetHighestPoint ();
			if (distance == oldDistance)
				return; // we have not moved the camera, so no rewards!
			score = (int)(distance * 100);
			score += GameManager.instance.GetCarrotScore ();
			if (score > maxScore) {
				AddScore (score - maxScore);
				maxScore = score;
				//scoreText.text = "" + (int)(distance * 100);
			} 
			if (distance == 0)
				maxScore = 0;

			if (privScore > TodaysBestScore) {
				TodaysBestScore = privScore;
				if (CelebratedTodaysBest == false) {
					CelebratedTodaysBest = true;
					GameManager.instance.TodaysBestScore ();

				}
			}
			if (privScore > AllTimeHighScore) {
				AllTimeHighScore = privScore;
				if (CelebratedAllTimeHigh == false) {
					CelebratedAllTimeHigh = true;
					GameManager.instance.AllTimeHighScore ();
                    //hs.setScoreAndUpload(AllTimeHighScore);
				}
			}



		}


	}



}
