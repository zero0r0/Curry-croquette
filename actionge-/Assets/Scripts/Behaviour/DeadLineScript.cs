using UnityEngine;
using System.Collections;

public class DeadLineScript : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Player") {
			MainGameManager.Instance.TransitionToGameOverScene();
		}
	}
}
