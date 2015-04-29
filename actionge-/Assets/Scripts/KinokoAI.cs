using UnityEngine;
using System.Collections;

public class KinokoAI : MonoBehaviour {

	
	//アニメーション関連の変数
	private Animator anim;
	private float speed;
	
	//方向転換のtimer
	//public float time;

	//前に進むときのフラグ。bool型
	[SerializeField]
	private bool forward;

	[SerializeField]
	private Vector3 startPos;
	//うごく範囲の指定
	[SerializeField]
	public float moveX;

	// Use this for initialization
	void Start () {
		//time = 0;
		forward = true;
		startPos = this.gameObject.transform.position;
		anim = GetComponent<Animator> ();
		speed = anim.GetFloat ("Speed");
	}
	
	
	// Update is called once per frame
	void Update () {
		Move ();
	}


	//移動関数
	void Move(){
		if (this.transform.position.x > startPos.x + moveX) {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
			forward = false;
		}
		if (this.transform.position.x < startPos.x - moveX) {
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
			forward = true;
		}

		if(forward)
			this.transform.localPosition += transform.forward * speed * Time.deltaTime;
		else 
			this.transform.localPosition -= transform.forward * speed * Time.deltaTime;


		//ChangeDirection (5.0f);
	}
	/*
	//向きを変える関数
	void ChangeDirection(float t){
		if (t < time) {
			transform.Rotate (new Vector3 (0f, 180f, 0f));
			time = 0;
		}
	}
*/
	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerScript>().ApplyDamage(1);
		}
	}

}
