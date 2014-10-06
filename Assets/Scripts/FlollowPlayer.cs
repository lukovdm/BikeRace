using UnityEngine;
using System.Collections;

public class FlollowPlayer : MonoBehaviour {
	public Transform playerTrans;

	// Update is called once per frame
	void Update () {
		if (playerTrans != null) {
			gameObject.transform.position = new Vector3 ((float)playerTrans.position.x, transform.position.y, -10f);
			if (playerTrans.position.y < camera.ScreenToWorldPoint (new Vector3 (0, 0, camera.nearClipPlane)).y) {
				transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, -10f);
			} else if (playerTrans.position.y > camera.ScreenToWorldPoint (new Vector3 (camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane)).y) {
				transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, -10f);
			}	
		} else {
			gameObject.transform.position = Vector3.zero;
		}
	}
}