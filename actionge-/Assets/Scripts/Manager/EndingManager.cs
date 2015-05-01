using UnityEngine;
using System.Collections;

public class EndingManager : SingletonMonoBehaviour<EndingManager> {

	// Use this for initialization
	void Start () {
		FadeInOutUtil.Instance.FadeOut(1f, Color.blue);
		if (ItemManager.Instance.CheckCollectAllItmes()) {
			Debug.Log("Collect All Items");
		} else {
			Debug.Log("まだアイテムは残っているよ");
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
