using UnityEngine;
using System.Collections;

public class NewMoveFloor2 : MonoBehaviour {
	
	//うごく範囲の指定
	[SerializeField]
	private float moveX;
	[SerializeField]
	private float moveZ;
	[SerializeField]
	private float moveY;
	//speed設定
	[SerializeField]
	private float speed;
	
	private Vector3 startPos;

	bool touch;

	
	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
		touch = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(touch)
			Move ();
	}
	
	void Move(){
		if (moveZ > 0) {
			if (moveZ + startPos.z > this.transform.position.z)
				this.transform.position += transform.forward * Time.deltaTime * speed;
		}
		if (moveZ < 0) {
			if (moveZ + startPos.z < this.transform.position.z)
				this.transform.position -= transform.forward * Time.deltaTime * speed;
		}
	}
	
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.transform.parent = this.transform;
			touch = true;
		}
	}
	
	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Player") {
			col.transform.parent = null;
		}
	}
}
