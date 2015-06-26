using UnityEngine;
using UniRx;

public class SplashManager : SingletonMonoBehaviour<SplashManager> {

	public AudioClip logoVoice;
	public float fadeTime;
	private AudioSource audioSource;

	void Start() {
		Cursor.visible = false;
		audioSource = GetComponent<AudioSource>();
		GetComponent<AudioSource>().clip = logoVoice;

		FadeInOutUtil.Instance.FadeOut(fadeTime, Color.white, () => {
			audioSource.Play();
			audioSource.ObserveEveryValueChanged(x => x.isPlaying)
				.Where(x => !x)
				.Subscribe(_ => FadeInOutUtil.Instance.FadeIn(fadeTime, Color.white, () => Application.LoadLevel("Title"))
			);
		});

	}

}
