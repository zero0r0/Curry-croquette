using UnityEngine;
using System.Collections;

public class NewMoveFloor1 : MonoBehaviour {

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
	bool forward;
	bool up;
	bool right;

	// Use this for initialization
	void Start () {
		startPos = this.transform.position;
		forward = false;
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move(){
		if (moveX != 0) {
			if (this.transform.position.x > startPos.x + moveX)
				right = false;
			if (this.transform.position.x < startPos.x - moveX)
				right = true;

			if(right)
				this.transform.position += transform.right * Time.deltaTime * speed;
			else 
				this.transform.position -= transform.right * Time.deltaTime * speed;
		}
		
		if (moveY != 0) {
			if (this.transform.position.y > startPos.y + moveY)
				up = false;
			if (this.transform.position.y < startPos.y - moveY)
				up = true;

			if(up)
				this.transform.position += transform.up * Time.deltaTime * speed;
			else 
				this.transform.position -= transform.up * Time.deltaTime * speed;
		}
		
		if (moveZ != 0) {
			if (this.transform.position.z > startPos.z + moveZ)
				forward = false;
			if (this.transform.position.z < startPos.z - moveZ)
				forward = true;

			if(forward)
				this.transform.position += transform.forward * Time.deltaTime * speed;
			else 
				this.transform.position -= transform.forward * Time.deltaTime * speed;
		}
	}
	
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.transform.parent = this.transform;
		}
	}

	void OnCollisionExit(Collision col){
		if (col.gameObject.tag == "Player") {
			col.transform.parent = null;
		}
	}
}
