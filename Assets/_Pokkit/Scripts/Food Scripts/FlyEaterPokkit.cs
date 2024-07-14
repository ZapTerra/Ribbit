using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEaterPokkit : MonoBehaviour {
	public CircleCollider2D tongueHitbox;
	public Animator animator;
	void Start() {

	}

	void Update() {
		animator.SetBool("TongueGrab", false);
		if (Input.GetMouseButtonDown(0) || Input.GetKeyDown("space")) {
			animator.SetBool("TongueGrab", true);
		}
	}
}
