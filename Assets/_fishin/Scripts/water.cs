using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour {
	public GameObject daCamera;
	void Update() {
		transform.position = new Vector2(Mathf.RoundToInt(daCamera.transform.position.x / 2) * 2, Mathf.RoundToInt(daCamera.transform.position.y / 2) * 2);
	}
}
