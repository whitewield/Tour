using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Camera : MonoBehaviour {

	private static CS_Camera instance = null;
	public static CS_Camera Instance { get { return instance; } }

	private CS_PlayerControl myPlayer;
	private Camera myCamera;

	private float myHeight = 10f;

	[SerializeField] float myPlayerForwardRatio = 1.26f;
	[SerializeField] Vector3 myTargetPosition;
	[SerializeField] float myMoveLerpSpeed = 1.8f;

	[SerializeField] bool doFollowPlayer = true;
	[SerializeField] float myTargetSize = 5;
	private float myFullSize;
	[SerializeField] float mySizeLerpSpeed = 1;
	[SerializeField] float mySizeFullRatio = 1.2f;


	private void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		myCamera = this.GetComponent<Camera> ();

		// get the default height of camera
		myHeight = this.transform.position.y;
	}
	
	void Start () {
		// find the player instance
		myPlayer = CS_PlayerControl.Instance;

		// calculate the full size according to map
		myFullSize = CalculateFullSize ();
	}

	// calculate the full size according to map
	private float CalculateFullSize () {
		// get back ground local scale
		Vector3 t_mapScale = CS_Map.Instance.BackGroundTransform.localScale;
		// calculate the map world scale to screen scale
		t_mapScale.y *= 0.5f;

		// map aspect
		float t_mapAspect = t_mapScale.x / t_mapScale.y;

		// if the camera aspect is larger than map aspect
		if (myCamera.aspect > t_mapAspect) {
			return t_mapScale.y * 0.5f * mySizeFullRatio;
		}

		// if the camera aspect is smaller than maps aspect
		float t_targetHeight = 1 / myCamera.aspect * t_mapScale.x;
		return t_targetHeight * 0.5f * mySizeFullRatio;
	}
	
	void Update () {
		if (doFollowPlayer) {
			Update_FollowPlayer ();
		} else {
			Update_FullView ();
		}

		// update the position
		this.transform.position = Vector3.Lerp (this.transform.position, myTargetPosition, Time.deltaTime * myMoveLerpSpeed);
		// update the size
		myCamera.orthographicSize = Mathf.Lerp (myCamera.orthographicSize, myTargetSize, Time.deltaTime * mySizeLerpSpeed);
	}

	private void Update_FullView () {
		myTargetPosition = new Vector3 (0, myHeight, -myHeight * 1.73f);

		myTargetSize = CalculateFullSize ();
	}

	private void Update_FollowPlayer () {
		// calculate target position according to player's move direction
		myTargetPosition =
			myPlayer.transform.position +
					myPlayer.MyDirection * myPlayerForwardRatio * myCamera.orthographicSize +
					new Vector3 (0, myHeight, -myHeight * 1.73f);
	}
}
