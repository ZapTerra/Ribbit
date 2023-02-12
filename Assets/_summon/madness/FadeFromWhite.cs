using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFromWhite : MonoBehaviour {

	SpriteRenderer sr;
	Color white2;
	float timer;
	// Use this for initialization
	void Start() {
		sr = GetComponent<SpriteRenderer>();
		white2 = new Color(1, 1, 1, 0);
	}

	private void OnEnable() {
		timer = -0.2f;
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime / 2;
		sr.color = Color.Lerp(Color.white, white2, timer);
		if (timer > 1) {
			//gameObject.SetActive(false);
		}
	}
}
