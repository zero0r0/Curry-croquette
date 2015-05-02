using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

	private Canvas canvas;
	private bool isTutorial = false;
	private GameObject objectPool;

	public float changeSceneInterval = 0;

	// Use this for initialization
	void Start () {
		objectPool = GameObject.FindGameObjectWithTag("Object Pool");
		canvas = GameObject.FindObjectOfType<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && canvas.enabled) {
			Camera.main.cullingMask = 1;
			canvas.enabled = false;
			Camera.main.clearFlags = CameraClearFlags.Skybox;
		}
		if (objectPool.GetComponentsInChildren<Transform>().Length >= 3 && !isTutorial) {
			StartCoroutine(TransitionToMainGame());
			isTutorial = true;
		}
	}

	IEnumerator TransitionToMainGame() {
		yield return new WaitForSeconds(changeSceneInterval);
		Application.LoadLevel("Stage");
	}
}
