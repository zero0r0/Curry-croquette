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

	// キャラクターの移動量
	private Vector3 velocity;
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
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int locoState = Animator.StringToHash ("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash ("Base Layer.Jump");
	static int damaState = Animator.StringToHash ("Base Layer.Damage");

	//jumpフラグ
	bool touchFloor;

	//ボイスID
	public enum VoiceId {Jump, Get, Damage};

	//Voice SE
	[SerializeField]
	private AudioClip jumpVoice;
	[SerializeField]
	private AudioClip getVoice;
	[SerializeField]
	private AudioClip[] damageVoice; 
	//PlayerのAudioSource
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		HP = 3;
//		Time.captureFramerate = 60;
		/*----------コンポーネントの習得-------------------------------*/
		audioSource = GetComponent<AudioSource> ();
		rigidbody = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator>();
		// CapsuleColliderコンポーネントを取得する
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();

		orgColHight = col.height;
		orgVectColCenter = col.center;

		GameObject check = new GameObject("First Check");
		check = Instantiate(check, transform.position, transform.rotation) as GameObject;
		check.gameObject.tag = "Check Point";
		SphereCollider collider = check.AddComponent<SphereCollider>();
		collider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxisRaw ("Horizontal");
		jumpHeight = anim.GetFloat("JumpHeight");
		velocity = new Vector3 (h, 0, 0);	// 左右のキー入力からx軸方向の移動量を取得
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// 参照用のステート変数にBase Layer (0)の現在のステートを設定する
		
		//方向キーの入力を受け付け次第、走るアニメーション再生
		if (Mathf.Abs (h) != 0 && !anim.GetBool ("Jump")) {
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
		//アニメーションのステートがJumpの最中
		if (currentBaseState.nameHash == jumpState) {
			touchFloor = false;
			if(!anim.IsInTransition(0))
			{	
				anim.SetBool("Jump", false);
				if(Mathf.Abs(h)!=0 && touchFloor){
					anim.SetBool("Run",true);
				}
			} 
		}
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
	void move(float h){
		if (currentBaseState.nameHash == idleState && touchFloor) {
			anim.SetBool ("Run", true);
		}
		if (h < 0) {
			//anim.SetBool ("Run", true);
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
			velocity = new Vector3 (-h, 0, 0);
			velocity *= forwardSpeed;					// 移動速度を掛ける
			transform.localPosition -= velocity * Time.fixedDeltaTime;
		}
		else if (h > 0) {
			//anim.SetBool ("Run", true);
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
			velocity *= forwardSpeed;						// 移動速度を掛ける
			transform.localPosition += velocity * Time.fixedDeltaTime;
		}
	}

	/*ジャンプ動作の関数 key判定条件も含む*/
	void jumpAnimation(){
		if(Input.GetKeyDown(KeyCode.Space) && touchFloor){
			if(!anim.IsInTransition(0)){
				anim.SetBool("Jump", true);
				touchFloor = false;
				//rigidbody.useGravity = false;
				//rigidbody.AddForce(transform.up * jumpHeight , ForceMode.Impulse);
				rigidbody.velocity = (transform.up * jumpHeight);
			}
	    }
	}

	//ジャンプ中のあたり判定の操作・アニメーションeventから呼び出す
	void JumpCol(){
		Voice (VoiceId.Jump);
		Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
		RaycastHit hitInfo = new RaycastHit();
		// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
			col.height = orgColHight - (jumpHeight * 0.01f);			// 調整されたコライダーの高さ
			float adjCenterY = orgVectColCenter.y + jumpHeight*0.01f*colY;
			col.center = new Vector3(0, adjCenterY, 0);		// 調整されたコライダーのセンター
	//	}
		StartCoroutine ("resetCollider");
	}

	// キャラクターのコライダーサイズのリセット関数
	private IEnumerator resetCollider()
	{
		yield return new WaitForSeconds (0.8f);
		// コンポーネントのHeight、Centerの初期値を戻す
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}

	//敵を倒すためのトリガー関数
	//地面についているかの判定も行う
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
			//EffectManager.Instance.InstantEffect(EffectManager.EffectId.KillEnemy, col.gameObject.transform.position);
			//rigidbody.AddForce(transform.up * jumpHeight * jumpOffset, ForceMode.Impulse);
			rigidbody.velocity = (transform.up * jumpHeight);
			touchFloor = false;
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.KillEnemy, col.transform.position);
			col.transform.position = new Vector3 (-100f,-100f,-100f);
		}
        // タグがItemの場合、ItemManagerに取得したアイテムを送る
        else if (col.tag == "Item") {
            ItemManager.Instance.SendItem(col.gameObject.GetComponent<ItemBehaviour>().ItemId);
			EffectManager.Instance.InstantEffect(EffectManager.EffectId.GetItem, col.gameObject.transform.position);
            MainGameManager.Instance.SetObjectToObjectPool(col.gameObject);
			Voice(VoiceId.Get);
        }
        // タグがGoalの場合、MainGameManagerにエンディング遷移処理を行うよう、指示を出す
		else if (col.tag == "Goal" && ItemManager.Instance.CheckCollectNecessaryItems()) {
            MainGameManager.Instance.TouthGoal();
        }
		else if (col.tag == "Check Point") {
			MainGameManager.Instance.TouchCheckPoint(col.transform);
		}

	}

	void OnCollisionStay(Collision col){
		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.up, -Vector3.up, out hit, 1.2f)) {
			if (col.gameObject.tag == "Floor")
				touchFloor = true;
			else 
				touchFloor = false;
		}
	}



    /// <summary>
    /// ダメージを受け付ける
    /// </summary>
    /// <param name="damange">ダメージ量</param>
    public void ApplyDamage(int damange) {
		if (!anim.GetBool("Damage") && HP > 0) {
			HP -= damange;
			UIManager.Instance.IncreasePlayerHP ();
			Voice(VoiceId.Damage);
			anim.SetBool ("Damage", true);
			// 体力が0以下になった時、ゲームオーバー処理を行う
			if (HP <= 0) {
				MainGameManager.Instance.ToGameOver();
				anim.SetTrigger("Die");
				this.enabled = false;
			}
		}
    }

	public void Voice(VoiceId v){
		if (v == VoiceId.Get) {
			this.audioSource.clip = getVoice;
			audioSource.Play();
		}
		if (v == VoiceId.Jump) {
			this.audioSource.clip = jumpVoice;
			audioSource.Play();
		}
		if (v == VoiceId.Damage) {
			int n;
			n = Random.Range(0,4);
			this.audioSource.clip = damageVoice[n];
			audioSource.Play();
		}

	}

	public void Respawn() {
		this.HP = 3;
		anim.SetTrigger("Respawn");
		UIManager.Instance.PlayerRespawn();
	}

}
