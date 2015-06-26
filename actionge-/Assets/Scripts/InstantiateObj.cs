using UnityEngine;

public class InstantiateObj : MonoBehaviour {

	new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		rigidbody.AddForce (Vector3.left*5, ForceMode.Impulse);
		Destroy (this.gameObject,2);
	}

}
