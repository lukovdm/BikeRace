using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeGround : MonoBehaviour {
	public Transform gPlayer;
	private Vector3 prePos;
	public List<Vector2> points = new List<Vector2>();
	private LineRenderer line;
	private EdgeCollider2D col;
	private int verts = 2;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
		col = gameObject.GetComponent<EdgeCollider2D> ();
		points.Add(new Vector2 ((float) gPlayer.position.x, (float) gPlayer.position.y));
	}

	void FixedUpdate () {
		if (prePos == gPlayer.position)
			return;

		line.SetVertexCount(verts + 1);
		points.Add(new Vector2 ((float) gPlayer.position.x, (float) gPlayer.position.y));
		line.SetPosition(verts, new Vector3((float) gPlayer.position.x, (float) gPlayer.position.y, -0.1f));
		col.points = points.ToArray();
		verts++;
		prePos = gPlayer.position;
	}

	public void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		List<Vector2> nPoints = new List<Vector2> ();
		int count = 0;
		if (stream.isWriting) {
			nPoints = points;
			count = nPoints.Count;
			stream.Serialize(ref count);
			for(int i = 0; i < nPoints.Count; i++){
				Vector3 nPoint = new Vector3(nPoints[i].x, nPoints[i].y, 0);
				stream.Serialize(ref nPoint);
			}
		} else {
			stream.Serialize(ref count);
			for(int i = 0; i < count; i++){
				Vector3 nPoint = Vector3.zero;
				stream.Serialize(ref nPoint);
				nPoints.Add(new Vector2(nPoint.x, nPoint.y));
			}
			points = nPoints;
		}
	}
}
