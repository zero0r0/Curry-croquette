using UnityEngine;
using System.Collections;

public class DeadLineBehaviour : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			MainGameManager.Instance.ToGameOver();
			this.enabled = false;
		}
	}
}
