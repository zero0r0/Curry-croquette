using UnityEngine;
using System.Collections;

public class KinokoAI : MonoBehaviour {

	
	//アニメーション関連の変数
	private Animator anim;
	private float speed;
	[SerializeField]
	private int speedLevel;
	
	//方向転換のtimer
	//public float time;

	//前に進むときのフラグ。bool型
	private bool forward;

	private Vector3 startPos;
	//うごく範囲の指定
	[SerializeField]
	public float moveX;
	[SerializeField]
	public float moveZ;

	// Use this for initialization
	void Start () {
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
		if (moveX != 0) {
			if (this.transform.position.x > startPos.x + moveX) {
				transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
				forward = false;
			}
			if (this.transform.position.x < startPos.x - moveX) {
				transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
				forward = true;
			}
		} 
		else {
			if (this.transform.position.z > startPos.z + moveZ) {
				transform.rotation = Quaternion.AngleAxis (-180f, Vector3.up);
				forward = false;
			}
			if (this.transform.position.z < startPos.z - moveZ) {
				transform.rotation = Quaternion.AngleAxis (0, Vector3.up);
				forward = true;
			}
		}

		if(forward)
			this.transform.localPosition += transform.forward * speed * Time.deltaTime * speedLevel;
		else 
			this.transform.localPosition += transform.forward * speed * Time.deltaTime * speedLevel;
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerScript>().ApplyDamage(1);
		}
	}

}
