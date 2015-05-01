using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScriptTess : MonoBehaviour {

	public float moveSpeed = 10f;
	private Vector3 moveDirection = Vector3.zero;
	private float gravity = 1f;
	private bool isGrounded = false;
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw("Horizontal");
		if (!isGrounded) {
			moveDirection.y -= gravity;
		} else if (Input.GetKeyDown(KeyCode.Space)) {
			moveDirection.y = moveSpeed;
			isGrounded = false;
		}
		moveDirection.z = moveSpeed * horizontal;
		rigidbody.velocity = moveDirection;
	}

	void OnCollisionEnter(Collision other) {
		if (other.transform.tag == "Floor") {
			isGrounded = true;
			moveDirection.y = 0;
		}
	}
}
