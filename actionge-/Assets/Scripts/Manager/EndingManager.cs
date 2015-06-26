using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingManager : SingletonMonoBehaviour<EndingManager> {

	private Image illust;
	private Canvas canvas;
	public float illustFadeOutTime = 1f;
	private AudioSource audioSource;
	public float resultInterval = 1f;

	public GameObject result;

	// Use this for initialization
	new void Awake () {
		base.Awake();
	}

	void Start() {
		FadeInOutUtil.Instance.FadeOut(illustFadeOutTime, Color.blue);

		canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
		illust = canvas.transform.GetChild(0).GetComponent<Image>();
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(StartFadeIllust(illustFadeOutTime, illust));
	}

	IEnumerator StartFadeIllust(float fadeTime, Image image) {
		yield return new WaitForSeconds(2f);
		AudioManager.Instance.PlaySound(AudioManager.SoundId.CroquetteTaberu);
		yield return new WaitForSeconds(2f);
		float nowTime = 0f;
		float fadeRate = image.color.a / fadeTime / 30;
		while (nowTime < fadeTime) {
			audioSource.volume -= fadeRate;
			Color color = image.color;
			color.a -=fadeRate;
			image.color = color;
			nowTime += fadeRate;
			yield return new WaitForSeconds(fadeRate);
		}
		AudioManager.Instance.PlayBGM();
		yield return new WaitForSeconds(resultInterval);
		ShowResult();
		yield return new WaitForSeconds (resultInterval + 10f);
		Application.LoadLevel ("Title");
	}

	private void ShowResult() {
//		result.SetActive(true);
	}
}
