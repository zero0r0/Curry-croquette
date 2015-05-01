using UnityEngine;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	public enum SoundId { GameOver, }

	[System.Serializable]
	public struct Sound {
		public SoundId soundId;
		public AudioClip sound;
	}

	public Sound gameOver;

	private AudioSource audioSource;

	void Start() {
		audioSource = this.GetComponent<AudioSource>();
	}

	public void PlaySound(SoundId soundId) {
		switch (soundId) {
			case SoundId.GameOver:
				audioSource.PlayOneShot(gameOver.sound);
				break;
		}
	}
}
