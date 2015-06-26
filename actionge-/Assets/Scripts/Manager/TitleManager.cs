using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Utils;

/// <summary>
/// タイトルシーン用マネージャクラス
/// </summary>
public class TitleManager : MonoBehaviour {

	[SerializeField]
	private AudioClip startVoice;

	private AudioSource audioSource;

	public float changeSceneInterval = 1f;

	// Use this for initialization
	void Start() {
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = startVoice;

		FadeInOutUtil.Instance.FadeOut(changeSceneInterval, Color.white);
		this.UpdateAsObservable().Where(x => Input.anyKeyDown).Subscribe(_ => {
			audioSource.Play();
			audioSource.ObserveEveryValueChanged(x => x.isPlaying)
				.Where(x => !x)
				.Subscribe(x => Application.LoadLevel("Tutorial"));
		});
	}

}
