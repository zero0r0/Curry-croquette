using UnityEngine;
using System.Collections;

/// <summary>
/// タイトルシーン用マネージャクラス
/// </summary>
public class TitleManager : MonoBehaviour {

    // 入力を促すテキスト
    public GameObject inputText;
	[SerializeField]
	private AudioClip startVoice;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown) {
			audioSource.clip = startVoice;
			audioSource.Play();
			StartCoroutine("GameStart");
        }
	}

	private IEnumerator GameStart(){
		yield return new WaitForSeconds(1f);
		Application.LoadLevel("mainGame");
	}

    /// <summary>
    /// 入力を促す文字を半透明にする
    /// </summary>
    private void FadeInOut() {

    }
}
