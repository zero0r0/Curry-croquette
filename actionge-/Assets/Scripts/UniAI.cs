using UnityEngine;

public class UniAI : MonoBehaviour {

	public GameObject[] asset;
	public GameObject Player;

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
			rand = Random.Range (0, asset.Length);
			if (time > 4) {
				Instantiate (asset [rand], this.transform.position + new Vector3 (0, 2.5f, 0), this.transform.rotation);
				time = 0;
			}
		}
		if (Vector3.Distance (Player.transform.position, transform.position) <= 10f) 
			action = true;
		else
			action = false;
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerScript>().ApplyDamage(1);
		}
	}
}
