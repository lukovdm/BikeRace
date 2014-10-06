using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	public float speed;

	void FixedUpdate () {
		if (!networkView.isMine)
			return;
		rigidbody2D.AddForce (new Vector2((Input.GetKey(KeyCode.RightArrow) ? 1 : 0) * speed, Input.GetAxis ("Vertical") * speed));
	}
}