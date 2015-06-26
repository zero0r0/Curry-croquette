using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	[SerializeField]
	private float colY;
	[SerializeField]
	private float jumpOffset;

	//キャラクターのスピード・ジャンプ力・HP
	public float jumpHeight;
	public float forwardSpeed;
	public int HP;

	//rigidbody
	private Rigidbody rigidbody;

	// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
	private float orgColHight;
	private Vector3 orgVectColCenter;

	// キャラクターコントローラ（カプセルコライダ）の参照
	private CapsuleCollider col;
	private Rigidbody rb;

	//アニメーション関連
	private Animator anim;
	private AnimatorStateInfo currentBaseState;// base layerで使われる、アニメーターの現在の状態の参照

	//アニメーター各ステートへの参照
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int damaState = Animator.StringToHash("Base Layer.Damage");

	//jumpフラグ
	bool jumpFlag;

	//ボイスID
	public enum VoiceId { Jump, Get, Damage };

	//Voice SE
	[SerializeField]
	private AudioClip jumpVoice;
	[SerializeField]
	private AudioClip getVoice;
	[SerializeField]
	private AudioClip[] damageVoice;
	//PlayerのAudioSource
	private AudioSource audioSource;

	public GameObject enemyRespawn;
	public GameObject goalEffect;

	// Use this for initialization
	void Start() {
		jumpFlag = false;
		HP = 3;

		/*----------コンポーネントの習得-------------------------------*/
		audioSource = GetComponent<AudioSource>();
		rigidbody = GetComponent<Rigidbody>(); // rb = GetComponent<Rigidbody>()と同じことをしているからどちらかを消したほうがいい
		anim = GetComponent<Animator>();
		// CapsuleColliderコンポーネントを取得する
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();

		orgColHight = col.height;
		orgVectColCenter = col.center;

		// 最初のチェックポイントの生成
		// ここでやるべきではないかも
		GameObject check = Instantiate(new GameObject("First Check Point"), transform.position, transform.rotation) as GameObject;
		check.tag = "Check Point";
		SphereCollider collider = check.AddComponent<SphereCollider>();
		collider.isTrigger = true;
	}

	// Update is called once per frame
	void Update() {

		float h = Input.GetAxisRaw("Horizontal");
		jumpHeight = anim.GetFloat("JumpHeight");
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する

		//方向キーの入力を受け付け次第、走るアニメーション再生
		if (Mathf.Abs(h) != 0) {
			move(h);
		} else
			anim.SetBool("Run", false);

		// currentBaseState.fullPathHash == locoState
		// nameHashは古い書き方だそうです
		if (currentBaseState.nameHash == locoState || currentBaseState.nameHash == idleState) {
			if((Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetButtonDown("Jump")) && jumpFlag){
			//if (Input.GetButtonDown("Jump") ) {
				jumpAnimation();
			}
		}

		/*		//アニメーションのステートがJumpの最中
				if (currentBaseState.nameHash == jumpState) {
					if(!anim.IsInTransition(0))
					{	
						if(Mathf.Abs(h)!=0 && jumpFlag){
							anim.SetBool("Run",true);
						}
					}
				}
		*/
		// レイキャストをキャラクターのセンターから落とす
		//キャラがジャンプ可能かどうかを判別
		/*RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.up, -Vector3.up, out hit, 1)) {
			Debug.Log(hit.collider.gameObject.tag);
			if(hit.collider.tag == "Floor") 
				jumpFlag = true;
			else 
				jumpFlag = false;
		}
		*/
	}

	/*移動用関数*/
	void move(float h) {
		if (currentBaseState.nameHash == idleState || currentBaseState.nameHash == locoState) {
			anim.SetBool("Run", true);
		}
		if (h < 0) {
			transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up);
			this.transform.position += new Vector3(h, 0, 0) * forwardSpeed * Time.deltaTime;
		} else if (h > 0) {
			transform.rotation = Quaternion.AngleAxis(90f, Vector3.up);
			this.transform.position += new Vector3(h, 0, 0) * forwardSpeed * Time.deltaTime;
		}
		// this.transform.position += new Vector3(h, 0, 0) * forwardSpeed * Time.deltaTime
		// 同じことだからifの外に出してもいい
		// transform.position += transform.forward * forwardSpeed * Time.deltaTimeでもいいかも
	}

	/*ジャンプ動作の関数 key判定条件も含む*/
	void jumpAnimation() {
		anim.SetBool("Jump", true);
		rigidbody.velocity = (transform.up * jumpHeight);
		jumpFlag = false;
	}

	//ジャンプ中のあたり判定の操作・アニメーションeventから呼び出す
	void JumpCol() {
		Voice(VoiceId.Jump);
		//Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
		//RaycastHit hitInfo = new RaycastHit();

		// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
		col.height = orgColHight - (jumpHeight * 0.01f);            // 調整されたコライダーの高さ
		float adjCenterY = orgVectColCenter.y + jumpHeight * 0.01f * colY;
		col.center = new Vector3(0, adjCenterY, 0);     // 調整されたコライダーのセンター
														//	}
		StartCoroutine("resetCollider");
	}

	// キャラクターのコライダーサイズのリセット関数
	private IEnumerator resetCollider() {
		yield return new WaitForSeconds(0.8f);
		// コンポーネントのHeight、Centerの初期値を戻す
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}
	// コルーチン使わずRx使ったほうが楽に書けるかも

	//敵を倒すためのトリガー関数
	//地面についているかの判定も行う
	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Enemy") {
			jumpFlag = false;
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.KillEnemy, col.gameObject.transform.position);
			rigidbody.velocity = (transform.up * jumpHeight);

			// 敵復活マーカーオブジェクトを生成する
			GameObject Respawn = Instantiate(enemyRespawn, col.transform.position, col.transform.rotation) as GameObject;
			RespawnEnemyManager rem = Respawn.GetComponent<RespawnEnemyManager>();
			rem.RespanEnemy(col.gameObject);
			MainGameManager.Instance.SetObjectToObjectPool(col.gameObject);
		}
		// タグがItemの場合、ItemManagerに取得したアイテムを送る
		else if (col.tag == "Item") {
			ItemManager.Instance.SendItem(col.gameObject.GetComponent<ItemBehaviour>().ItemId);
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.GetItem, col.gameObject.transform.position);
			MainGameManager.Instance.SetObjectToObjectPool(col.gameObject);
			Voice(VoiceId.Get);
		}
		// タグがGoalの場合、MainGameManagerにエンディング遷移処理を行うよう、指示を出す
		else if (col.tag == "Goal") {
			MainGameManager.Instance.TouchGoal();
			for(int i = 0; i < 3; i++)
				Instantiate(goalEffect,new Vector3(col.transform.position.x + Random.Range(0,2f), -0.6f, col.transform.position.z), goalEffect.transform.rotation);
		} else if (col.tag == "Check Point") {
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.CheckPoint, transform.position);
			MainGameManager.Instance.TouchCheckPoint(col.transform);
		} else if (col.tag == "Adjust") {
			this.transform.position = new Vector3(this.transform.position.x , this.transform.position.y ,col.transform.position.z);
		}

	}

	void OnCollisionStay(Collision col) {
		//RaycastHit hit;
		//if (Physics.Raycast (transform.position + Vector3.up, -Vector3.up, out hit, 1.2f)) {
		if (col.gameObject.tag == "Floor")
			jumpFlag = true;
		else
			jumpFlag = false;
	}


	void OnCollisionEnter(Collision col) {
		RaycastHit hit;
		if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, 0.5f)) {
			if (col.gameObject.tag == "Floor")
				jumpFlag = true;
			else
				jumpFlag = false;
		}
		if (col.gameObject.tag == "Enemy")
			jumpFlag = true;
		else
			jumpFlag = false;

	}

	void OnCollisionExit(Collision col) {
		if (col.gameObject.tag == "Floor" || col.gameObject.tag == "Enemy")
			jumpFlag = false;
	}


	/// <summary>
	/// ダメージを受け付ける
	/// </summary>
	/// <param name="damange">ダメージ量</param>
	public void ApplyDamage(int damange = 1) {
		// ダメージモーション中は無敵時間
		if (!anim.GetBool("Damage") && HP > 0) {
			HP -= damange;
			UIManager.Instance.DecreasePlayerHP();
			Voice(VoiceId.Damage);
			anim.SetBool("Damage", true);
			// 体力が0以下になった時、ゲームオーバー処理を行う
			if (HP <= 0) {
				MainGameManager.Instance.ToGameOver();
				anim.SetTrigger("Die");
				enabled = false;
			}
		}
	}

	public void Voice(VoiceId v) {
		if (v == VoiceId.Get) {
			audioSource.clip = getVoice;
			audioSource.Play();
		}
		if (v == VoiceId.Jump) {
			audioSource.clip = jumpVoice;
			audioSource.Play();
		}
		if (v == VoiceId.Damage) {
			int n;
			n = Random.Range(0, 4); // Random.Range(0, damageVoice.Length) と書くと安全
			audioSource.clip = damageVoice[n];
			audioSource.Play();
		}

	}

	// プレイヤーリスポーン
	public void Respawn() {
		HP = 3;
		anim.SetTrigger("Respawn");
		UIManager.Instance.PlayerRespawn();
	}
}
