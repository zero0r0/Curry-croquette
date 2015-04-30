using UnityEngine;
using System.Collections;

/// <summary>
/// MainGame1Scene管理クラス
/// </summary>
public class MainGameManager : SingletonMonoBehaviour<MainGameManager> {

	// オブジェクトプールTransform
	private Transform objectPool;

	// シーン遷移するまでのインターバルタイム
	public float changeSceneInterval = 3f;

    new void Awake() {
        CheckInstance();
		objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
    }

    /// <summary>
    /// Endingシーンへ遷移する
    /// </summary>
    public void TouthGoal() {
		StartCoroutine(TransitionToEndingScene());
		ItemManager.Instance.ShowItemInfo();
    }

	private IEnumerator TransitionToEndingScene() {
		yield return new WaitForSeconds(changeSceneInterval);
		ItemManager.Instance.gameObject.transform.parent = null;
		Application.LoadLevel("Ending");
	}

	public void SetObjectToObjectPool(GameObject obj) {
		obj.transform.parent = objectPool.transform;
		obj.transform.localPosition = Vector3.zero;
	}

}
