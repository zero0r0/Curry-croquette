using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// オーディオ管理クラス
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	public enum SoundId { GameOver, CroquetteTaberu,}

	[System.Serializable]
	public struct Sound {
		public SoundId soundId;
		public AudioClip sound;
	}

	public Sound gameOver;
	public Sound croquette;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}

	public void PlaySound(SoundId soundId) {
		switch (soundId) {
			case SoundId.GameOver:
				audioSource.PlayOneShot(gameOver.sound);
				break;
			case SoundId.CroquetteTaberu:
				audioSource.PlayOneShot(croquette.sound);
				break;
		}
	}

	public void PlayBGM() {
		audioSource.volume = 0.2f;
		audioSource.Play();
	}

	public void FadeOutBGM(float fadeTime) {
		this.FixedUpdateAsObservable()
			.TakeWhile(x => audioSource.volume > 0)
			.Subscribe(x => audioSource.volume -= audioSource.volume / fadeTime);
	}

	private IEnumerator FadeOut(float fadeTime) {
		float nowTime = 0f;
		float fadeRate = audioSource.volume / fadeTime / 30;
		while (nowTime < fadeTime) {
			audioSource.volume -= fadeRate;
			if (audioSource.volume <= 0)
				break;
			nowTime += fadeRate;
			yield return new WaitForSeconds(1 / 30);
		}
		audioSource.Stop();
	}
	
}
