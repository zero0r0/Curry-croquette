using UnityEngine;
using System.Collections;

public class KinokoAI : MonoBehaviour {
		
	// キャラクターの移動量
	public Vector3 velocity;
	
	//アニメーション関連の変数
	private Animator anim;
	private float speed;
	
	//方向転換のtimer
	public float time;
	
	// Use this for initialization
	void Start () {
		time = 0;
		anim = GetComponent<Animator> ();
		speed = anim.GetFloat ("Speed");
	}
	
	
	// Update is called once per frame
	void Update () {
		Move ();
	}


	//移動関数
	void Move(){
		time += Time.deltaTime;
		this.transform.Translate (velocity * speed * Time.deltaTime);
		ChangeDirection (5.0f);
	}

	//向きを変える関数
	void ChangeDirection(float t){
		if (t < time) {
			transform.Rotate (new Vector3 (0f, 180f, 0f));
			time = 0;
		}
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponent<PlayerScript>().ApplyDamage(1);
		}
	}

}
