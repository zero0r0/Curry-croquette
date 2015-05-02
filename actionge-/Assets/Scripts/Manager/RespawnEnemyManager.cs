using UnityEngine;
using System.Collections;

public class RespawnEnemyManager : MonoBehaviour {

	GameObject obj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespanEnemy(GameObject enemy){
		obj = enemy;
	}

	void OnBecameInvisible() {
		obj.transform.position = this.transform.position;
	}
}
