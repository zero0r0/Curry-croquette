using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
        CheckInstance();
    }

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		deadLinePos = player.transform.position;
		deadLinePos.y -= deadLineHeight;
		deadLine = Instantiate(deadLine, deadLinePos, Quaternion.Euler(Vector3.zero)) as GameObject;
		objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
	}

	void Update() {
		deadLinePos.x = player.transform.position.x;
		deadLine.transform.position = deadLinePos;
	}

    /// <summary>
    /// Endingシーンへ遷移する
    /// </summary>
    public void TouthGoal() {
		StartCoroutine(TransitionToEndingScene());
    }

	private IEnumerator TransitionToEndingScene() {
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.blue);
		AudioManager.Instance.FadeOutBGM(changeSceneInterval);
		yield return new WaitForSeconds(changeSceneInterval);
		ItemManager.Instance.gameObject.transform.parent = null;
		Application.LoadLevel("Ending");
	}

	public void TouchCheckPoint(Transform checkPoint) {
		latestCheckPoint = checkPoint;
	}

	public void ToGameOver() {
		StartCoroutine(TransitionToGameOver());
	}
	
	private IEnumerator TransitionToGameOver() {
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.black);

		EffectManager.Instance.InstantEffect(EffectManager.EffectId.GameOver);
		//AudioManager.Instance.PlaySound(AudioManager.SoundId.GameOver);
		AudioManager.Instance.FadeOutBGM(changeSceneInterval);

		yield return new WaitForSeconds(changeSceneInterval);

		FadeInOutUtil.Instance.FadeOut(changeSceneInterval, Color.black);
		AudioManager.Instance.PlayBGM();

		player.transform.position = latestCheckPoint.position;
		(player.GetComponent(typeof(PlayerScript)) as PlayerScript).enabled = true;
		player.GetComponent<PlayerScript>().Respawn();
	}

	public void SetObjectToObjectPool(GameObject obj) {
		obj.transform.parent = objectPool.transform;
		obj.transform.localPosition = Vector3.zero;
	}

}
