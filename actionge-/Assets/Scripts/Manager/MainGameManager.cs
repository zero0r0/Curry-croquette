using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// MainGame1Scene管理クラス
/// </summary>
public class MainGameManager : SingletonMonoBehaviour<MainGameManager> {

	// オブジェクトプールTransform
	private Transform objectPool;

	// シーン遷移するまでのインターバルタイム
	public float changeSceneInterval = 3f;

	public GameObject deadLine;

	private GameObject player;
	private Vector3 deadLinePos;
	public float deadLineHeight;

    new void Awake() {
        CheckInstance();
		objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
    }

	void Start() {
		player = GameObject.FindGameObjectWithTag("Player") as GameObject;
		deadLinePos = player.transform.position;
		deadLinePos.y -= deadLineHeight;
		deadLine = Instantiate(deadLine, deadLinePos, Quaternion.Euler(Vector3.zero)) as GameObject;
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
		FadeInOut.Instance.FadeIn(changeSceneInterval, Color.white);
		yield return new WaitForSeconds(changeSceneInterval);
		ItemManager.Instance.gameObject.transform.parent = null;
		Application.LoadLevel("Ending");
	}

	public void SetObjectToObjectPool(GameObject obj) {
		obj.transform.parent = objectPool.transform;
		obj.transform.localPosition = Vector3.zero;
	}

	public void TransitionToGameOverScene() {
		AudioManager.Instance.PlaySound(AudioManager.SoundId.GameOver);
	}

}
