using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.UI;

public class SplashManager : SingletonMonoBehaviour<SplashManager> {

	public Image logo;
	public AudioClip logoVoice;
	public float fadeTime;

	void Start() {

	}

	IEnumerator StartSplash() {
		FadeInOutUtil.Instance.FadeOut(fadeTime, Color.black);
		yield return new WaitForSeconds(fadeTime);

	}


}
