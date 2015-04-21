using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col){
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Leg") {
			Destroy (this.gameObject, 0.5f);
		}
	}
}
