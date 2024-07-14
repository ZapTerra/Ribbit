using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class water : MonoBehaviour {
	public float jumpDistance = 1;
	public GameObject daCamera;
	void Start() {

	}
	void Update() {
		transform.position = new Vector2(
			Mathf.RoundToInt(daCamera.transform.position.x * .5f) * jumpDistance,
			Mathf.RoundToInt(daCamera.transform.position.y * .5f) * jumpDistance
		);
	}
}
