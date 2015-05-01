using UnityEngine;
using System.Collections;

public class ZipMonster : MonoBehaviour {
	
	public GameObject player;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float jumpHeight;
	//前に進むときのフラグ。bool型
	[SerializeField]
	private bool forward;
	//スタートのポジション
	[SerializeField]
	private Vector3 startPos;
	//うごく範囲の指定
	[SerializeField]
	public float moveX;
	private int rand;
	Rigidbody rigidbody;

	private Animator anim;

	void Start(){
		forward = true;
		startPos = this.gameObject.transform.position;
		anim = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
	}

	void Updata(){
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
	void Move(){
		if (this.transform.position.x > startPos.x + moveX) {
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
			forward = false;
		}
		if (this.transform.position.x < startPos.x - moveX) {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
			forward = true;
		}
		
		if (forward)
			this.transform.localPosition += transform.forward * speed * Time.deltaTime;
		else 
			this.transform.localPosition += transform.forward * speed * Time.deltaTime;
	}
	
	void jump(){
		rigidbody.AddForce (transform.up * jumpHeight, ForceMode.Impulse);
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