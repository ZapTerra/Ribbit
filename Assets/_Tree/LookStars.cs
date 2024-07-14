using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookStars : MonoBehaviour {
	public GameObject starCamera;
	private Camera mainCamera;
	// Start is called before the first frame update
	void Start() {
		mainCamera = Camera.main;
	}

	// Update is called once per frame
	void Update() {

	}

	public void Switch() {
		starCamera.gameObject.SetActive(mainCamera.enabled);
		mainCamera.enabled = !mainCamera.enabled;
	}
}
