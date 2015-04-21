using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public float forwardSpeed;
	public float force;

	private Animator anim;
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

	private Rigidbody rigidbody;
	// キャラクターの移動量
	private Vector3 velocity;

	//アニメーター各ステートへの参照
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash ("Base Layer.Jump");

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");
		velocity = new Vector3 (h, 0, 0);	// 左右のキー入力からx軸方向の移動量を取得
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
		//anim.SetFloat("Speed", h);
		
		//方向キーの入力を受け付け次第、走るアニメーション再生
		if (Mathf.Abs (h) != 0 && !anim.GetBool ("Jump")) {
			anim.SetBool ("Run", true);
			move(h);
		}
		else 
			anim.SetBool ("Run", false);

		//アニメーションのステートがIdleの最中
		if (currentBaseState.nameHash == idleState) { 
			jumpAnimation();
		}
		//アニメーションのステートがLocomotionの最中
		if (currentBaseState.nameHash == locoState) {
			jumpAnimation();
		}
		if (currentBaseState.nameHash == jumpState) {
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Jump", false);
			}
		}
	}

	/*移動用関数*/
	void move(float h){
		if (h < 0) {
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
			velocity = new Vector3 (-h, 0, 0);
			velocity *= forwardSpeed;						// 移動速度を掛ける
			transform.localPosition -= velocity * Time.fixedDeltaTime;
		}
		else if (h > 0) {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
			velocity *= forwardSpeed;						// 移動速度を掛ける
			transform.localPosition += velocity * Time.fixedDeltaTime;
		}
	}

	/*ジャンプ動作の関数 key判定条件も含む*/
	void jumpAnimation(){
		if(Input.GetKeyDown(KeyCode.Space)){
			if(!anim.IsInTransition(0)){
				anim.SetBool("Jump", true);
				rigidbody.AddForce(transform.up * force, ForceMode.Impulse);
			}
	    }
	}
}