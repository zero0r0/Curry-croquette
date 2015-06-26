using UnityEngine;
using UniRx;
using Utils;

namespace Managers {
	/// <summary>
	/// スプラッシュシーン用管理クラス
	/// </summary>
	public class SplashManager : SingletonMonoBehaviour<SplashManager> {

		// ライセンスを読み上げるボイス
		public AudioClip logoVoice;
		private AudioSource audioSource;

		// フェードイン・アウトに要する時間
		public float fadeTime = 1f;

		new void Awake() {
			base.Awake();
			// マウスカーソルを非表示にする
			Cursor.visible = false;
		}

		void Start() {
			// AudioSourceを取得し、ライセンスを読み上げるボイスを設定する
			audioSource = GetComponent<AudioSource>();
			GetComponent<AudioSource>().clip = logoVoice;

			// フェードイン・アウトのイベント登録
			// フェードアウトが終了したら、ライセンスを読み上げるボイスを再生し、再生が終了したらフェードインを開始する
			FadeInOutUtil.Instance.FadeOut(fadeTime, Color.white, () => {
				audioSource.Play();
				audioSource.ObserveEveryValueChanged(x => x.isPlaying)
					.First()
					.Where(x => !x)
					.Subscribe(_ => FadeInOutUtil.Instance.FadeIn(fadeTime, Color.white, () => Application.LoadLevel("Title"))
				);
			});

		}
	}

}
