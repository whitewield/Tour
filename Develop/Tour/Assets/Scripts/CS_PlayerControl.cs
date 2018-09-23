using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_PlayerControl : MonoBehaviour {

	private static CS_PlayerControl instance = null;
	public static CS_PlayerControl Instance { get { return instance; } }

	private Rigidbody myRigidbody;

	// store my camera
	private Camera myCamera;
	private CS_Camera myCameraScript;

	// for movement
	[SerializeField] float myAcceleration = 10;
	[SerializeField] float myFrictionRatio = 0.9f;

	// my moving direction
	private Vector3 myDirection = Vector3.zero;
	public Vector3 MyDirection { get { return myDirection; } }

	private void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		// find my rigidBody
		myRigidbody = this.GetComponent<Rigidbody> ();
	}

	void Start () {
		// enable multi-touch
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			Input.multiTouchEnabled = true;
		}

		// find my camera
		myCameraScript = CS_Camera.Instance;
		myCamera = myCameraScript.GetComponent<Camera> ();
	}

	void Update () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			// if its on iphone, use multi-touch calculation

			// get the touches
			Touch[] t_touches = Input.touches;

			// record the center of all touches
			Vector2 t_touchCenter = Vector2.zero;

			if (t_touches.Length != 0) {
				// get average
				for (int i = 0; i < t_touches.Length; i++) {
					t_touchCenter += t_touches[i].position;
				}
				t_touchCenter /= t_touches.Length;

				// set direction
				myDirection = ScreenPointToDirection (t_touchCenter);
			}
		} else if (Input.GetMouseButton (0)) {
			// if its using a mouse, do mouse calculation
			myDirection = ScreenPointToDirection (Input.mousePosition);
		} else {
			// otherwise, set direction to zero
			myDirection = Vector3.zero;
		}
	}

	private void FixedUpdate () {
		// get current velocity
		Vector3 t_currentVelocity = myRigidbody.velocity;

		// apply acceleration
		t_currentVelocity += myDirection * myAcceleration * Time.fixedDeltaTime;

		// apply friction
		t_currentVelocity = t_currentVelocity * myFrictionRatio;

		// set velocity
		myRigidbody.velocity = t_currentVelocity;
	}

	private Vector3 ScreenPointToDirection (Vector2 g_screenPoint) {
		// get a point on the ray
		Vector3 t_pointOnLine =
			myCamera.ScreenToWorldPoint (new Vector3 (g_screenPoint.x, g_screenPoint.y, myCamera.farClipPlane));

		// get the target world position
		Vector3 t_targetPosition = 
			LinePlaneIntersection (t_pointOnLine, myCamera.transform.forward, Vector3.up, Vector3.zero);

		// calculate the direction
		t_targetPosition = t_targetPosition - this.transform.position;

		// if the direction is longer than zero, normalize
		if (Mathf.Approximately (t_targetPosition.sqrMagnitude, 0)) {
			return Vector3.zero;
		} else {
			return t_targetPosition.normalized;
		}
	}

	/// <summary>
	/// Gets the intersection point with line and plane.
	/// reference: https://en.wikipedia.org/wiki/Line%E2%80%93plane_intersection
	/// </summary>
	/// <returns>The intersection point</returns>
	/// <param name="linePoint">a point on the line.</param>
	/// <param name="lineDirection">line direction.</param>
	/// <param name="planeNormal">plane's normal.</param>
	/// <param name="planePoint">a point on the plane.</param>
	public static Vector3 LinePlaneIntersection (Vector3 linePoint, Vector3 lineDirection, Vector3 planeNormal, Vector3 planePoint) {
		float d = Vector3.Dot (planePoint - linePoint, planeNormal) / Vector3.Dot (lineDirection, planeNormal);
		return d * lineDirection + linePoint;
	}
}
