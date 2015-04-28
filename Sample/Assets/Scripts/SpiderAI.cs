using UnityEngine;
using System.Collections;

public class SpiderAI : MonoBehaviour {

	public GameObject player;

	// キャラクターの移動量
	public Vector3 velocity;
	public int jumpHeight;

	private Rigidbody rigidbody;

	//アニメーション関連の変数
	private Animator anim;
	private float speed;
	
	//方向転換のtimer
	public float time;

	private int random;

	// Use this for initialization
	void Start () {
		time = 0;
		anim = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
		speed = anim.GetFloat ("Speed");
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move(){
		random = Random.Range (0,200);
		time += Time.deltaTime;
		this.transform.Translate (velocity * speed * Time.deltaTime);

		//一定確率でジャンプしたり等ランダムにうごく
		if (random == 1)
			jump ();

		//レイキャストがプレイヤーにヒットしたかの判定
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, Vector3.forward, out hit, 5)) {
			if (hit.collider.tag == "Player") 
				ChangeDirection (true, 6.0f);
			else
				ChangeDirection (false, 6.0f);
		}
	}

	void jump(){
		rigidbody.AddForce (transform.up * jumpHeight, ForceMode.Impulse);
		anim.SetBool("Jump",true);
	}

	//spiderの場合は引数ふたつ。bool型はtrueの場合はplayerを発見したとき
	//発見次第、レイキャストの届く範囲以内ならおってくる.
	void ChangeDirection(bool discovery ,float t){
		if (!discovery) {
			if (t < time) {
				transform.Rotate (new Vector3 (0f, 180f, 0f));
				time = 0;
			}
		} else { 
			transform.LookAt (player.transform.position); //ターゲットの方に向かせる
			transform.position += transform.forward * speed * Time.deltaTime; //前方に移動速度分動かす
		}
	}

	//プレイヤーにあたったときの判定
	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.GetComponent<PlayerScript>().HP--;
			//col.rigidbody.AddForce();
			Debug.Log(col.gameObject.GetComponent<PlayerScript>().HP);
		}
	}
}