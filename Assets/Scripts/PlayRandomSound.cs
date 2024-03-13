using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
	public AudioClip[] clips;
	public AudioClip current;
	public AudioSource source;
	public string Description;
	public float randMax = 0;
	// Use this for initialization
	void Start ()
	{
		if(source == null)
		source = GetComponent<AudioSource> ();
	}

	public void PlayIndex(int index) {
		if (GameManager.instance != null && GameManager.instance.SFXEnabled () == false)
			return;
		if (source == null)
			return;
		if (clips.Length == 0)
			return;
		if (index > clips.Length)
			return;
		
		source.enabled = true;
		if (source.isPlaying) {
			//source.Stop ();

		}
	
		source.clip = clips [index];

		source.Play ();

		source.pitch = Random.Range (1.0f - randMax, 1.0f + randMax);
	}

	IEnumerator PlayDelay(float delay) {
		yield return new WaitForSeconds (delay);
		Play ();
	}
	public void PlayWithDelay(float delay) {
		StartCoroutine (PlayDelay (delay));
	}
	public void Play ()
	{
		PlayIndex (Random.Range (0, clips.Length));

	}
}
