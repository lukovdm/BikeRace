using UnityEngine;
using System.Collections;

public class Grounded : MonoBehaviour {
	public float timeTillAir = 0.5f;

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("enter");
		GetComponentInParent<Ride> ().grounded = true;
		StopAllCoroutines ();
	}

	void OnCollisionExit2D(Collision2D coll) {
		Debug.Log ("left");
		StartCoroutine ("leaveGround");
	}

	IEnumerator leaveGround(){
		yield return new WaitForSeconds (timeTillAir);
		Debug.Log ("not Grounded");
		GetComponentInParent<Ride>().grounded = false;
	}
}
