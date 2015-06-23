using UnityEngine;

public class RespawnEnemyManager : MonoBehaviour {

	private GameObject obj;

	public void RespanEnemy(GameObject enemy){
		obj = enemy;
	}

	void OnBecameInvisible() {
		obj.transform.position = transform.position;
		Destroy (gameObject);
	}
}
