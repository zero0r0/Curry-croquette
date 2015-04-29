using UnityEngine;
using System.Collections;

public class MainGameManager : SingletonMonoBehaviour<MainGameManager> {

    new void Awake() {
        CheckInstance();
    }

    /// <summary>
    /// Endingシーンへ遷移する
    /// </summary>
    public void TouthGoal() {
		StartCoroutine(TransitionToEndingScene());
		ItemManager.Instance.ShowItemInfo();
    }

	private IEnumerator TransitionToEndingScene() {
		yield return new WaitForSeconds(3f);
		ItemManager.Instance.gameObject.transform.parent = null;
		Application.LoadLevel("Ending");
	}

	public void SetObjectToObjectPool(GameObject obj) {
		obj.transform.parent = this.transform;
		obj.transform.localPosition = Vector3.zero;
	}

}
