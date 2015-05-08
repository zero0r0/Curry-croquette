using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashManager : SingletonMonoBehaviour<SplashManager> {

	public Image logo;
	public AudioClip logoVoice;
	public float fadeTime;
	private AudioSource audioSource;

	void Start() {
		Cursor.visible = false;
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = logoVoice;
		StartCoroutine(StartSplash());
	}

	IEnumerator StartSplash() {
		FadeInOutUtil.Instance.FadeOut(fadeTime, Color.white);
		yield return new WaitForSeconds(fadeTime);
		audioSource.Play();
		while (true) {
			yield return new WaitForSeconds(0.1f);
			if (!audioSource.isPlaying)
				break;
		}
		FadeInOutUtil.Instance.FadeIn(fadeTime, Color.white);
		yield return new WaitForSeconds(fadeTime * 2);
		Application.LoadLevel("Title");
	}


}
