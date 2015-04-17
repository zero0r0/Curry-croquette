using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float forwardSpeed;
	public float force;

	private Animator anim;

	private Rigidbody rigidbody;

	// キャラクターの移動量
	private Vector3 velocity;

	/*//左右のキーの押されているか判定の設定
	private bool rightBool;
	private bool leftBool;
	*/
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");
		velocity = new Vector3 (h, 0, 0);			// 上下のキー入力からx軸方向の移動量を取得

		/*------方向キーの入力を受け付け次第、走るアニメーション再生-------*/
		if (Mathf.Abs(h) > 0.1 && !anim.GetBool("Jump")) 
			anim.SetBool ("Run", true);
		else
			anim.SetBool ("Run",false);
		/*-----------------------------------------------------------*/

		if (Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Jump", true);
			rigidbody.AddForce(transform.up * force, ForceMode.Impulse);
		}
		else
			anim.SetBool ("Jump", false);
		

		//RUN再生中はキャラを動かす
		if (anim.GetBool("Run"))
			move (h);
	}

	/*移動用関数*/
	void move(float h){
		velocity *= forwardSpeed;	// 移動速度を掛ける
		transform.localPosition += velocity * Time.fixedDeltaTime;
	}
}
