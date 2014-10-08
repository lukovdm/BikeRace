using UnityEngine;
using System.Collections;

public class FlollowPlayer : MonoBehaviour {
	public Transform playerTrans;

	// Update is called once per frame
	void Update () {
		if (playerTrans != null) {
			gameObject.transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y, -10f);
		} else {
			gameObject.transform.position = Vector3.zero;
		}
	}
}