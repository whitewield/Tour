using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Map : MonoBehaviour {

	private static CS_Map instance = null;
	public static CS_Map Instance { get { return instance; } }

	[SerializeField] Transform myBackGroundTransform;
	public Transform BackGroundTransform { get { return myBackGroundTransform; } }

	private void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}
	}
	
	void Start () {
		
	}
	
	void Update () {
		
	}
}
