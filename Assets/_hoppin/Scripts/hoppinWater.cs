using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hoppinWater : MonoBehaviour {
	public float jumpDistance = 1;
	public GameObject daCamera;
	public GameObject polyWater;
	private bool rats = true;
	void Start() {
		//needed
		//don't know why
		gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
	void Update() {
		if(rats){
			//needed
			//don't know why
			polyWater.SetActive(false);
			polyWater.SetActive(true);
			rats = false;
		}
		transform.position = new Vector3(
			Mathf.RoundToInt(daCamera.transform.position.x * 0.5f) * jumpDistance,
			Mathf.RoundToInt(daCamera.transform.position.y * 0.5f) * jumpDistance,
			.98f
		);
	}
}
