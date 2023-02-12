using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashGameInteractions : MonoBehaviour {
	public bool bowlFilled = false;
	public bool frogInBowl = false;
	public bool stage1 = true;
	public Animator frogAnimator;
	public Animator progressBarAnimator;
	private float startTime = Mathf.Infinity;
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButtonDown(0) && !bowlFilled) {
			frogAnimator.SetBool("Start Pouring", true);
		}
		if (Time.time - startTime >= 7 && bowlFilled) {
			Debug.Log("Slow");
			frogAnimator.speed -= Time.deltaTime * .33f;
		}
		if (Time.time - startTime >= 10 && bowlFilled) {
			frogAnimator.speed = 1;
			frogAnimator.SetBool("End Wash", true);
		}
	}
	void DoneFilling() {
		bowlFilled = true;
	}

	void DoneClimbing() {
		progressBarAnimator.SetBool("Start Wash", true);
		startTime = Time.time;
	}
}
