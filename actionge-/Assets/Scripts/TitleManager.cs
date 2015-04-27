using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルシーン用マネージャクラス
/// </summary>
public class TitleManager : MonoBehaviour {

    // 入力を促すテキスト
    public GameObject inputText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown) {
            Application.LoadLevel("mainGame");
        }
	}

    /// <summary>
    /// 入力を促す文字を半透明にする
    /// </summary>
    private void FadeInOut() {

    }
}
