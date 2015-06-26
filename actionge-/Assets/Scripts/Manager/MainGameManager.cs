using UnityEngine;
using Utils;
using UniRx;
using UniRx.Triggers;
using System;

namespace Managers {
	/// <summary>
	/// MainGame1Scene管理クラス
	/// </summary>
	public class MainGameManager : SingletonMonoBehaviour<MainGameManager> {

		private Transform objectPool;
		private Transform player;

		// シーン遷移するまでのインターバルタイム
		public float changeSceneInterval = 3f;

		// 最後に通過したチェックポイント
		private Transform latestCheckPoint;

		// ゴール時のエフェクト
		public GameObject firework;

		void Start() {
			// オブジェクトプールのTransformを取得する
			objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
			player = GameObject.FindGameObjectWithTag("Player").transform;

			this.UpdateAsObservable()
				.Where(x => Input.GetKeyDown(KeyCode.Q))
				.Subscribe(x => {
					TouchGoal();
				});
		}

		/// <summary>
		/// Endingシーンへ遷移する
		/// </summary>
		public void TouchGoal() {
			
			Observable.Timer(TimeSpan.FromSeconds(0f), TimeSpan.FromSeconds(0.3f))
				.Take(10)
				.Subscribe(_ => {
					AudioManager.Instance.PlaySound(AudioManager.SoundId.Firework);
					ParticleSystem firework
						= (Instantiate(this.firework, player.transform.position, transform.rotation) as GameObject)
						.GetComponent<ParticleSystem>();
					firework.ObserveEveryValueChanged(x => x.isPlaying)
						.Where(x => !x)
						.Subscribe(x => { }, () => { AudioManager.Instance.PlaySound(AudioManager.SoundId.Firework); });
				}).AddTo(gameObject);
			Observable.Timer(TimeSpan.FromSeconds(0f), TimeSpan.FromSeconds(1f))
				.Buffer(8)
				.Subscribe(x => Application.LoadLevel("Result")).AddTo(gameObject);
			enabled = false;
		}

		/// <summary>
		/// チェックポイント通過処理
		/// </summary>
		/// <param name="checkPoint">通過したチェックポイントのTransform</param>
		public void TouchCheckPoint(Transform checkPoint) {
			latestCheckPoint = checkPoint;
		}

		/// <summary>
		/// ゲームオーバー処理
		/// </summary>
		public void ToGameOver() {
			FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.black, () => {
				PlayerRespawn();
			});
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.GameOver);
			AudioManager.Instance.FadeOutBGM(changeSceneInterval);
		}

		/// <summary>
		/// プレイヤーの復活処理
		/// </summary>
		private void PlayerRespawn() {
			FadeInOutUtil.Instance.FadeOut(changeSceneInterval, Color.black);
			AudioManager.Instance.PlayBGM();

			// プレイヤーを最後に到達したチェックポイントに配置する
			player.transform.position = latestCheckPoint.position;
			// PlayerScriptのenabledがfalseのとき取得するためにはこのように書く必要がある
			(player.GetComponent(typeof(PlayerScript)) as PlayerScript).enabled = true;
			player.GetComponent<PlayerScript>().Respawn();
		}

		/// <summary>
		/// オブジェクトをオブジェクトプールへセットする
		/// </summary>
		/// <param name="obj">オブジェクトプールにセットするGameObject</param>
		public void SetObjectToObjectPool(GameObject obj) {
			obj.transform.SetParent(objectPool.transform, false);
		}

	}

}