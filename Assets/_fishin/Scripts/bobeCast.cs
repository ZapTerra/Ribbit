using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobeCast : MonoBehaviour {
	public AudioManager audioManager;
	public GameObject parentObject;
	public GameObject frogeObject;
	public GameObject splashFX;
	public float castDist = 0;
	public float castHeight = 5;
	public float bobeSpeed = 8;
	public float minBobeSpeed = .2f;
	public bool goMeLaddie = false;
	public bool returnToFather = false;
	public bool physicsThisFrame = false;
	public bool doSplash = false;
	public string state;

	private Vector2 defaultPos;
	private float bobeTravel;
	private float bobeSpeedUsed;
	private float distanceLeft;
	// Start is called before the first frame update
	void Start() {
		doSplash = false;
		defaultPos = transform.localPosition;
		bobeSpeedUsed = bobeSpeed;
	}

	// Update is called once per frame
	void Update() {

		if (frogeObject.GetComponent<frogeFisheMove>().frogeState == 1 && !returnToFather) {
			returnToFather = true;
		}

		if (goMeLaddie) {
			transform.rotation = frogeObject.transform.rotation;
			transform.parent = null;
			state = "casting";
			distanceLeft = castDist;
			goMeLaddie = false;
			Camera.main.GetComponent<smoothCamera>().target2 = transform;
		}

		if (doSplash && physicsThisFrame) {
			audioManager.Play("BobPlop");
			Instantiate(splashFX, transform.position, Quaternion.identity);
			gameObject.tag = "bobe";
			doSplash = false;
		}

		if (returnToFather) {
			gameObject.tag = "Untagged";
			gameObject.GetComponent<Collider2D>().enabled = false;
			transform.parent = parentObject.transform;
			transform.localPosition = defaultPos;
			distanceLeft = 0;
			bobeSpeedUsed = bobeSpeed;
			returnToFather = false;
		}

		if (distanceLeft > 0) {
			bobeTravel = Time.deltaTime * bobeSpeedUsed;
			bobeSpeedUsed = bobeSpeed - ((bobeSpeed - minBobeSpeed) * (1 - distanceLeft / castDist));
			distanceLeft -= bobeTravel;
			transform.position += transform.up * bobeTravel;
			if (distanceLeft <= 0) {
				gameObject.GetComponent<Collider2D>().enabled = true;
				doSplash = true;
			}
		}

		physicsThisFrame = false;
	}

	private void FixedUpdate() {
		physicsThisFrame = true;
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		physicsThisFrame = true;
		if (collider.gameObject.CompareTag("liliePade")) {
			audioManager.Play("LaughTrack");
			doSplash = false;
			returnToFather = true;
		}
	}
}
