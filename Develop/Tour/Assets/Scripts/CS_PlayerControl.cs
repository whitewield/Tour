using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerControl : MonoBehaviour {

	[SerializeField] float mySpeed = 2;

	// Use this for initialization
	void Start () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			Input.multiTouchEnabled = true;
		}
	}

	void Update () {

		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			//Debug.Log ("IPhonePlayer");

			// get the touches
			Touch[] t_touches = Input.touches;

			// record the center of all touches
			Vector2 t_touchCenter = Vector2.zero;

			if (t_touches.Length != 0) {
				// get average
				for (int i = 0; i < t_touches.Length; i++) {
					t_touchCenter += t_touches[i].position;
					Debug.Log ("t_touches:" + i + " " + t_touchCenter);
				}
				t_touchCenter /= t_touches.Length;

				Debug.Log ("t_touchCenter:" + t_touchCenter);

				ScreenPointMove (t_touchCenter);
			}
		} else if (Input.GetMouseButton (0)) {
			//Debug.Log ("Mouse");
			ScreenPointMove (Input.mousePosition);
		}
	}

	private void ScreenPointMove (Vector2 g_screenPoint) {

		//Debug.Log ("Screen:" + g_screenPoint);

		// get the target world position
		Vector3 t_targetPosition =
			Camera.main.ScreenToWorldPoint (new Vector3 (g_screenPoint.x, g_screenPoint.y, Camera.main.farClipPlane));

		//Debug.Log ("ScreenToWorld:" + t_targetPosition);

		t_targetPosition = Vector3.ProjectOnPlane (t_targetPosition - Camera.main.transform.position, Camera.main.transform.forward);
		t_targetPosition = Vector3.ProjectOnPlane (t_targetPosition, Vector3.up);

		//Debug.Log ("World:" + t_targetPosition);

		//Debug.Log ("Nor:" + t_targetPosition.normalized);

		// move the player
		if (!Mathf.Approximately (t_targetPosition.sqrMagnitude, 0)) {
			this.transform.position = this.transform.position + t_targetPosition.normalized * mySpeed * Time.deltaTime;
		}
	}
}
