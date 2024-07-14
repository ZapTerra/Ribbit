using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocMove : MonoBehaviour {
	public float centerDiff = 0;
	public float crocGap = 5;
	public float crocNumber = 3;
	public float maxRearDist = 10;
	public float jumpSpeed = 5;
	public float randomOffset;
	public GameObject[] models;
	public Animator animator;
	public Transform frogePos;
	public FrogeMove frogeScript;

	private bool canJump = true;
	private bool randBool;
	private bool goBack = false;
	private float distToGo = 0;
	private float diff;
	private float respawnDist;
	// Start is called before the first frame update
	void Start() {
		transform.parent = null;
		IsKillYou();
		frogePos = GameObject.Find("frogeNew").GetComponent<Transform>();
		frogeScript = GameObject.Find("frogeNew").GetComponent<FrogeMove>();
		respawnDist = crocGap * crocNumber;
	}

	// Update is called once per frame
	void Update() {
		if (transform.position.y < frogePos.position.y - maxRearDist || transform.position.x < frogePos.position.x - respawnDist) {
			Destroy(gameObject);
		}
		if (distToGo > 0) {
			distToGo -= jumpSpeed * Time.deltaTime;
			transform.position = new Vector2(transform.position.x, transform.position.y + (goBack ? jumpSpeed : -jumpSpeed) * Time.deltaTime);
			if (distToGo <= 0 && !goBack) {
				animator.SetBool("Lunge", true);
				distToGo = 2;
				goBack = true;
			}
		} else {
			animator.SetBool("Lunge", false);
			animator.SetBool("CloseEyes", frogePos.position.y < transform.position.y ? false : true);
			animator.SetBool("LookingL", frogePos.position.x < transform.position.x ? true : false);
		}
	}
	void OnTriggerEnter(Collider otherObject) {
		if (otherObject.gameObject.CompareTag("Player")) {
			if (canJump == true) {
				Debug.Log("Vibrate, croc lunge");
				Vibration.VibratePop();
				animator.SetBool("Lunge", true);
				distToGo = 2;
				canJump = false;
			}
		}
	}

	void IsKillYou() {
		randBool = Random.Range(1, 11) > 8;
		goBack = false;
		foreach (var r in models) {
			r.SetActive(randBool);
		}
		BoxCollider[] boxColliders = gameObject.GetComponents<BoxCollider>();
		foreach (var b in boxColliders) {
			b.enabled = randBool;
		}

		randomOffset = Random.Range(-1.5f, 1.5f);
		transform.position += Vector3.right * randomOffset;
	}
}
