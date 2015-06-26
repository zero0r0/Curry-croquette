using UnityEngine;
using System.Collections;

public class FireWorkScript : MonoBehaviour {
	
	// 花火の打ち上げにかかる時間が２秒なのでエフェクトの時間を2.2～3秒ぐらいにする
	void Start () {
		Destroy (this.gameObject, Random.Range(2.2f, 3.2f));
	}
}
