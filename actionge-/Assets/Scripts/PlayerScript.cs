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

	//jumpフラグ
	bool jumpFlag;

    // アイテム管理クラス
    private ItemManager itemManager;

    // UI管理クラス
    private UIManager uiManager;

    // ステージ管理クラス
    private StageManager stageManager;

    // オブジェクトプールTransform
    private Transform objectPool;

	// Use this for initialization
	void Start () {
		HP = 3;
		rigidbody = GetComponent<Rigidbody> ();
		// Animatorコンポーネントを取得する
		anim = GetComponent<Animator>();

		// CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
		col = GetComponent<CapsuleCollider>();
		rb = GetComponent<Rigidbody>();
		orgColHight = col.height;
		orgVectColCenter = col.center;

        // 各Managerクラスの取得
        itemManager = GameObject.FindGameObjectWithTag("Item Manager").GetComponent<ItemManager>();
        uiManager = GameObject.FindGameObjectWithTag("UI Manager").GetComponent<UIManager>();
        stageManager = GameObject.FindGameObjectWithTag("Stage Manager").GetComponent<StageManager>();

        // オブジェクトプールのTransformの取得
        objectPool = GameObject.FindGameObjectWithTag("Object Pool").transform;
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
			if(!anim.IsInTransition(0))
			{	
				anim.SetBool("Jump", false);
				if(Mathf.Abs(h)!=0){
					anim.SetBool("Run",true);
				}
			}
		}
	}

	/*移動用関数*/
	void move(float h){
		if (currentBaseState.nameHash == idleState) {
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
		// レイキャストをキャラクターのセンターから落とす
		RaycastHit hit;
		if (Physics.Raycast (transform.position + Vector3.up, -Vector3.up, out hit, 100)) {
			if(hit.collider.tag == "Floor") 
				jumpFlag = true;
			else 
				jumpFlag = false;
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			if(!anim.IsInTransition(0)){
				anim.SetBool("Jump", true);
				//rigidbody.useGravity = false;
				rigidbody.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
			}
	    }
	}

	//ジャンプ中のあたり判定の操作・アニメーションeventから呼び出す
	void JumpCol(){
		Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
		RaycastHit hitInfo = new RaycastHit();
		// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
		//if (Physics.Raycast(ray, out hitInfo))
		//{
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
		//rigidbody.useGravity = true;
		col.height = orgColHight;
		col.center = orgVectColCenter;
	}

	//敵を倒すためのトリガー関数
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Enemy") {
			rigidbody.AddForce(transform.up * jumpHeight * jumpOffset, ForceMode.Impulse);
			//JumpCol();
			col.transform.position = new Vector3 (-100f,-100f,-100f);
		}
        // タグがItemの場合、ItemManagerに取得したアイテムを送る
        else if (col.tag == "Item") {
            itemManager.SendItem(col.gameObject.GetComponent<ItemBehaviour>().ItemId);
            SetToObjectPool(col.gameObject);
        }
        // タグがGoalの場合、StageManagerにエンディング遷移処理を行うよう、指示を出す
        else if (col.tag == "Goal") {
            stageManager.TransitionToEndingScene();
        }
	}

    /// <summary>
    /// ダメージを受け付ける
    /// </summary>
    /// <param name="damange">ダメージ量</param>
    public void ApplyDamage(int damange) {
        HP -= damange;
        uiManager.IncreasePlayerHP();
        anim.SetBool("Damage", true);
    }

    /// <summary>
    /// GameObjectをObjectPoolにセットする
    /// Destroyの代わりに使用する
    /// </summary>
    /// <param name="obj">ObjectPoolに移動させるGameObject</param>
    private void SetToObjectPool(GameObject obj) {
        obj.transform.parent = objectPool;
        obj.transform.position = objectPool.position;
    }

}