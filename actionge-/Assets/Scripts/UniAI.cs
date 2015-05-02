using UnityEngine;
using System.Collections;

public class UniAI : MonoBehaviour {

	public GameObject[] asset;

	float time;
	int rand;


	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		rand = Random.Range(0,4);
		if (time > 5) {
			Instantiate(asset[rand],this.transform.position + new Vector3 (0,2.5f,0),this.transform.rotation);
			time = 0;
		}
	}
}
