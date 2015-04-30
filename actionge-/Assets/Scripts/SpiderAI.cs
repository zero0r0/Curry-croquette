using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour {

	public GameObject player;

	//アニメーション関連の変数
	private Animator anim;
	private float speed;
	private float jumpHeight;

	//前に進むときのフラグ。bool型
	private bool forward;
	//スタートのポジション
	private Vector3 startPos;
	//うごく範囲の指定
	[SerializeField]
	public float moveX;

	private int rand;

	Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		forward = true;
		startPos = this.gameObject.transform.position;
		anim = GetComponent<Animator> ();
		speed = anim.GetFloat ("Speed");
		jumpHeight = anim.GetFloat ("JumpHeight");
	}
	
	
	// Update is called once per frame
	void Update () {
		Move ();
		rand = Random.Range (0,300);

		if (rand == 1)
			jump ();
		//レイキャストがプレイヤーにヒットしたかの判定
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, Vector3.forward, out hit, 5)) {
			if (hit.collider.tag == "Player") 
				Chase();
		}
	}


	//移動関数
	void Move(){
		if (this.transform.position.x > startPos.x + moveX) {
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
			forward = false;
		}
		if (this.transform.position.x < startPos.x - moveX) {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
			forward = true;
		}

		if(forward)
			this.transform.localPosition += transform.forward * speed * Time.deltaTime;
		else 
			this.transform.localPosition += transform.forward * speed * Time.deltaTime;
	}

	void jump(){
		rigidbody.AddForce (transform.up * jumpHeight, ForceMode.Impulse);
		anim.SetBool("Jump",true);
	}

	//発見次第追ってくる
	void Chase(){
		transform.LookAt (player.transform.position); //ターゲットの方に向かせる
		transform.position += transform.forward * speed * Time.deltaTime; //前方に移動速度分動かす
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerScript>().HP--;
			Debug.Log(col.gameObject.GetComponent<PlayerScript>().HP);
		}
	}
}