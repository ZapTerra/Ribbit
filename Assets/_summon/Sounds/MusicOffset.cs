using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOffset : MonoBehaviour {

	int rarity;
	public float offset;
	float timer;
	bool done;
	public GameObject starsHolder;
	// Use this for initialization
	void Start() {
		timer = -offset;
		rarity = starsHolder.GetComponent<AdjustScale>().rarity;
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;
		if (!done && timer > 0) {
			if (rarity >= 5) {
				GetComponents<AudioSource>()[0].Play();
				//Debug.Log("yehw");
			} else if (rarity >= 4) {
				GetComponents<AudioSource>()[2].Play();
				//Debug.Log("ok");
			} else {
				GetComponents<AudioSource>()[1].Play();
				//Debug.Log("well");
			}
			//GetComponent<AudioSource>().Play();
			done = true;
		}

	}
}
