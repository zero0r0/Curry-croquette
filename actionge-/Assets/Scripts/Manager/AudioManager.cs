using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Managers {

	/// <summary>
	/// オーディオ管理クラス
	/// </summary>
	public class AudioManager : SingletonMonoBehaviour<AudioManager> {

		public enum SoundId { GameOver, CroquetteTaberu, }

		[System.Serializable]
		public struct Sound {
			public SoundId soundId;
			public AudioClip sound;
		}

		public Sound gameOver;
		public Sound croquette;

		private AudioSource audioSource;
		private float defaultVolume;

		// Use this for initialization
		void Start() {
			audioSource = GetComponent<AudioSource>();
			defaultVolume = audioSource.volume;
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
			audioSource.volume = defaultVolume;
			audioSource.Play();
		}

		public void FadeOutBGM(float fadeTime) {
			float startTime = Time.time;
			this.FixedUpdateAsObservable()
				.TakeWhile(x => Time.time  - startTime < fadeTime)
				.Subscribe(x => audioSource.volume -= audioSource.volume / fadeTime / 20);
		}

	}

}
