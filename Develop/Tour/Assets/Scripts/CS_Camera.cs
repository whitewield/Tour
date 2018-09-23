using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Camera : MonoBehaviour {

	private static CS_Camera instance = null;
	public static CS_Camera Instance { get { return instance; } }

	private CS_PlayerControl myPlayer;
	private Camera myCamera;

	private float myHeight = 10f;
	[SerializeField] float myPlayerForwardRatio;
	[SerializeField] float myLerpSpeed = 1;

	private void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		myCamera = this.GetComponent<Camera> ();
	}

	// Use this for initialization
	void Start () {
		myPlayer = CS_PlayerControl.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 t_targetPosition =
			myPlayer.transform.position +
					myPlayer.MyDirection * myPlayerForwardRatio * myCamera.orthographicSize +
					new Vector3 (0, myHeight, -myHeight * 1.73f);

		this.transform.position = Vector3.Lerp (this.transform.position, t_targetPosition, Time.deltaTime * myLerpSpeed);
	}
}
