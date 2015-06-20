using UnityEngine;
using System.Collections;

public class UniAI : MonoBehaviour {

	public GameObject[] asset;

	float time;
	int rand;

	bool action;

	// Use this for initialization
	void Start () {
		action = false;
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (action) {
			time += Time.deltaTime;
			rand = Random.Range (0, 3);
			if (time > 5) {
				Instantiate (asset [rand], this.transform.position + new Vector3 (0, 2.5f, 0), this.transform.rotation);
				time = 0;
			}
		}
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerScript>().ApplyDamage(1);
		}
	}
	
	void OnBecameVisible (){		
		action = true;
	}

	void OnBecameInvisible (){
		action = false;
	}

}
