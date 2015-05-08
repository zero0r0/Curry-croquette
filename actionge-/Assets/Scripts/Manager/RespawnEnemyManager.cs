using UnityEngine;
using System.Collections;

public class RespawnEnemyManager : MonoBehaviour {

	private GameObject obj;

	public void RespanEnemy(GameObject enemy){
		obj = enemy;
	}

	void OnBecameInvisible() {
		obj.transform.position = this.transform.position;
		Destroy (this.gameObject);
	}
}
