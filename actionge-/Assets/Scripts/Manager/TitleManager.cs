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
	[SerializeField]
	private AudioClip logoVoice;

	private AudioSource audioSource;

	public float changeSceneInterval = 1f;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = logoVoice;
		StartCoroutine(StartSplash());
	}

	private IEnumerator StartSplash() {
		FadeInOutUtil.Instance.FadeOut(changeSceneInterval, Color.black);
		yield return new WaitForSeconds(changeSceneInterval);
		audioSource.Play();
		while (true) {
			yield return new WaitForSeconds(0.01f);
			if  (!audioSource.isPlaying)
				break;
		}
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.black);
		yield return new WaitForSeconds(changeSceneInterval);
		FadeInOutUtil.Instance.FadeOut(1f, Color.black);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKeyDown) {
			audioSource.PlayOneShot(startVoice);
			StartCoroutine("GameStart");
        }
	}

	private IEnumerator GameStart(){
		FadeInOutUtil.Instance.FadeIn(changeSceneInterval, Color.white);
		yield return new WaitForSeconds(changeSceneInterval);
		Application.LoadLevel("mainGame");
	}

}
