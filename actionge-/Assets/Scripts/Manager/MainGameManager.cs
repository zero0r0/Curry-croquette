using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// MainGame1Scene管理クラス
/// </summary>
public class MainGameManager : SingletonMonoBehaviour<MainGameManager> {

	// オブジェクトプールTransform
	private Transform objectPool;

	// 落下時のゲームオーバー判定
	public GameObject deadLine;
	public float deadLineHeight;
	private GameObject player;
	private Vector3 deadLinePos;

	// シーン遷移するまでのインターバルタイム
	public float changeSceneInterval = 3f;

	// 最後に通過したチェックポイント
	private Transform latestCheckPoint;

    new void Awake() {
		base.Awake();
    }

	void Start() {
		// プレイヤーを取得し、落下死亡判定オブジェクトをプレイヤーに追従させるよう設定する
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		deadLinePos = player.transform.position;
		deadLinePos.y -= deadLineHeight;
		deadLine = Instantiate(deadLine, deadLinePos, Quaternion.Euler(Vector3.zero)) as GameObject;

		// オブジェクトプールのTransformを取得する
		objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
	}

	void Update() {
		deadLinePos.x = player.transform.position.x;
		deadLine.transform.position = deadLinePos;
	}

    /// <summary>
    /// Endingシーンへ遷移する
    /// </summary>
    public void TouchGoal() {
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.blue, () => {
			ItemManager.Instance.gameObject.transform.SetParent(null, false);
			Application.LoadLevel("Ending");
		});
		AudioManager.Instance.FadeOutBGM(changeSceneInterval);
    }

	public void TouchCheckPoint(Transform checkPoint) {
		latestCheckPoint = checkPoint;
	}

	// ゲームオーバー処理
	public void ToGameOver() {
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.black, () => {
			PlayerRespawn();
		});
		EffectManager.Instance.InstantEffect(EffectManager.EffectId.GameOver);
		AudioManager.Instance.FadeOutBGM(changeSceneInterval);
	}

	private void PlayerRespawn() {
		FadeInOutUtil.Instance.FadeOut(changeSceneInterval, Color.black);
		AudioManager.Instance.PlayBGM();

		// プレイヤーを最後に到達したチェックポイントに配置する
		player.transform.position = latestCheckPoint.position;
		// PlayerScriptのenabledがfalseのとき取得するためにはこのように書く必要がある
		(player.GetComponent(typeof(PlayerScript)) as PlayerScript).enabled = true;
		player.GetComponent<PlayerScript>().Respawn();
	}
		
	public void SetObjectToObjectPool(GameObject obj) {
		obj.transform.parent = objectPool.transform;
		obj.transform.localPosition = Vector3.zero;
	}

}
