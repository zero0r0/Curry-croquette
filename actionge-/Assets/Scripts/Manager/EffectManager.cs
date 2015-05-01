using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EffectManager : SingletonMonoBehaviour<EffectManager> {

	public enum EffectId { ItemGet, KillEnemy,  }

	[System.Serializable]
	public struct Effect {
		public EffectId effectId;
		public GameObject particle;
		public AudioClip se;
	}
	
	public Effect getItem;
	public Effect killEnemy;

	private AudioSource audioSource;

	void Start() {
		audioSource = this.GetComponent<AudioSource>();
	}

	delegate void EffectParticle(GameObject particle, Vector3 position, AudioClip audioClip);
	public void InstantEffect(EffectId effectId, Vector3 position) {
		
		EffectParticle func = (particle, pos, audioClip) => {
			Instantiate(particle, pos, Quaternion.Euler(Vector3.zero));
			audioSource.PlayOneShot(audioClip);
		};

		switch (effectId) {
			case EffectId.ItemGet:
				func(getItem.particle, position, getItem.se);
				break;
			case EffectId.KillEnemy:
				func(killEnemy.particle, position, killEnemy.se);
				break;
		}
	}

	public void InstantEffect(EffectId effectId) {

		EffectParticle func = (particle, pos, audioclip) => {
			audioSource.PlayOneShot(audioclip);
		};

		switch (effectId) {
			case EffectId.ItemGet:
				audioSource.PlayOneShot(getItem.se);
				break;
			case EffectId.KillEnemy:
				audioSource.PlayOneShot(killEnemy.se);
				break;
		}
	}

}
