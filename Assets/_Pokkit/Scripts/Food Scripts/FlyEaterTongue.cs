using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEaterTongue : MonoBehaviour {
	public GameObject flyPrefab;
	private GameObject snaggedFly;
	public Animator animator;
	void Start() {

	}

	void Update() {
		if (transform.localScale.x == 0 && snaggedFly != null) {
			Destroy(snaggedFly);
			Instantiate(flyPrefab);
		}
		if (snaggedFly == null) {
			animator.SetBool("Nom", false);
		}
	}
	void OnCollisionStay2D(Collision2D collision) {
		Debug.Log("Collision");
		snaggedFly = collision.gameObject;
		if (snaggedFly.tag == "Pokkit Fly") {
			animator.SetBool("Nom", true);
			snaggedFly.GetComponent<FlyMovementPokkit>().enabled = false;
			snaggedFly.transform.parent = transform;
		}
	}
}
