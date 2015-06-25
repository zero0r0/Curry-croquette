using UnityEngine;
using Managers;

/// <summary>
/// プレイヤー落下死亡判定クラス
/// </summary>
public class DeadLineBehaviour : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			MainGameManager.Instance.ToGameOver();
			enabled = false;
		}
	}
}
