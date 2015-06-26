using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EffectManager : SingletonMonoBehaviour<EffectManager> {

	public enum EffectId { GetItem, KillEnemy, GetSweet, GameOver, CheckPoint }

	[System.Serializable]
	public struct Effect {
		public EffectId effectId;
		public GameObject particle;
		public AudioClip se;
	}
	
	public Effect getItem;
	public Effect killEnemy;
	public Effect getSweet;
	public Effect gameOver;
	public Effect checkPoint;

	public float destroyInterval = 2;

	private AudioSource audioSource;

	void Start() {
		audioSource = this.GetComponent<AudioSource>();
	}

	delegate void EffectParticle(GameObject particle, Vector3 position, AudioClip audioClip);
	public void InstantEffect(EffectId effectId, Vector3 position) {
		
		EffectParticle func = (particle, pos, audioClip) => {
			GameObject obj = Instantiate(particle, pos, Quaternion.Euler(Vector3.zero)) as GameObject;
			Destroy(obj, destroyInterval);
			audioSource.PlayOneShot(audioClip);
		};

		switch (effectId) {
			case EffectId.GetItem:
				func(getItem.particle, position, getItem.se);
				break;
			case EffectId.KillEnemy:
				func(killEnemy.particle, position, killEnemy.se);
				break;
			case EffectId.GetSweet:
				func(getSweet.particle, position, getSweet.se);
				break;
			case EffectId.GameOver:
				func(checkPoint.particle, position, checkPoint.se);
				break;
		}
	}

	public void InstantEffect(EffectId effectId) {

		switch (effectId) {
			case EffectId.GetItem:
				audioSource.PlayOneShot(getItem.se);
				break;
			case EffectId.KillEnemy:
				audioSource.PlayOneShot(killEnemy.se);
				break;
			case EffectId.GetSweet:
				audioSource.PlayOneShot(getSweet.se);
				break;
			case EffectId.GameOver:
				audioSource.PlayOneShot(gameOver.se);
				break;
			case EffectId.CheckPoint:
				audioSource.PlayOneShot(checkPoint.se);
				break;
		}
	}

}
