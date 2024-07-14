using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrogeMove : MonoBehaviour {
	public Animator animator;
	public static int adCounter;
	public static float timeOfLastAd;
	public float jumpDist = 1;
	public float regHeight = -.2f;
	public float jumpHeight = 1;
	public float jumpSpeed = 1;
	public float distanceLeft;
	public string state;
	public bool isGoingRight = true;
	public GameObject leSplashe;
	public static bool keepingCount = true;
	private bool doReset = false;
	private bool loggedReasonForNoReset = false;
	private float jumpTravel;
	private Rigidbody rb;


	void Start() {
		keepingCount = true;
		rb = GetComponent<Rigidbody>();
		leSplashe.SetActive(false);
	}

	void Update() {

		if (Time.timeScale == 1 && doReset) {
			if(!LoadScenes.sceneIsTransitioning){
				Debug.Log("Resetting because timescale is 1, flagging scene as no longer transitioning.");
				SceneManager.LoadScene("River", LoadSceneMode.Single);
			}else{
				Debug.Log("Scene not transitioned because the home scene is being loaded");
			}
		} else if (Time.timeScale != 1 && !loggedReasonForNoReset) {
			loggedReasonForNoReset = true;
			Debug.Log("Initially not resetting because timescale is " + Time.timeScale);
		}

		transform.eulerAngles = Vector4.zero;
		try {
			isGoingRight = GetComponentInParent<LogeMove>().goRight;
		} catch {

		}

		if ((Input.GetMouseButton(0) || Input.GetKeyDown("space")) && state == "grounded") {
			transform.parent = null;
			state = "jumping";
			animator.SetBool("Jumping", true);
			distanceLeft = jumpDist;
			if(keepingCount){
				transform.position = transform.position + Vector3.back * jumpHeight;
			}
		}

		if (distanceLeft > 0) {
			//disable collider while jumping so frog can go over the alligators
			//GetComponent<BoxCollider>().enabled = false;
			//if the frog isn't underwater, render it over the alligators
			// if (gameObject.GetComponent<SpriteRenderer>().sortingOrder != -2) {
			// 	GetComponent<SpriteRenderer>().sortingOrder = 6;
			// }
			jumpTravel = Time.deltaTime * jumpSpeed;
			distanceLeft -= jumpTravel;
			if (distanceLeft <= 0 && keepingCount) {
				transform.position = new Vector3(transform.position.x, Mathf.Round(transform.position.y), Mathf.Clamp(transform.position.z, -Mathf.Infinity, -.21f));


				GetComponent<BoxCollider>().enabled = true;
				//if the frog isn't underwater, put its render layer back under the alligator's top jaw
				// if (gameObject.GetComponent<SpriteRenderer>().sortingOrder != -2) {
				// 	GetComponent<SpriteRenderer>().sortingOrder = 4;
				// }


			} else {
				transform.position += Vector3.up * jumpTravel;
			}

			if (distanceLeft < .5f) {
				animator.SetBool("Jumping", false);
			}
		}
	}

	private void OnCollisionStay(Collision collision) {
		if (collision.collider.tag == "Splishe") {
			keepingCount = false;
			gameObject.GetComponent<SpriteRenderer>().sortingOrder = -2;
			Destroy(gameObject.GetComponent<Rigidbody>());
			gameObject.GetComponent<BoxCollider>().enabled = false;
			leSplashe.SetActive(true);
			StartCoroutine(WaitReset(1f));
			Debug.Log("Reset for Splishe");
		}
		//to remove double check to not kill frog, comment out && distanceLeft <= 0) {
		if (collision.collider.tag == "Die"){//} && distanceLeft <= 0) {
			Debug.Log(collision.gameObject.name);
			Debug.Log("Reset for Die");
			Reset();
		}
		if (collision.collider.tag == "Untagged") {
			transform.SetParent(collision.transform);
		}
		if (leSplashe.activeInHierarchy) {
			transform.parent = null;
		}
		state = "grounded";
	}
	
	private IEnumerator WaitReset(float time) {
		Debug.Log("Flagging scene as transitioning.");
		Debug.Log("Waiting");
		yield return new WaitForSeconds(time);
		Reset();
	}

	private void Reset() {
		adCounter++;
		Debug.Log(adCounter);
		if ((adCounter >= 5 && Time.time - timeOfLastAd >= 180) || Time.time - timeOfLastAd >= 300) {
			//FindObjectOfType<FullscreenAd>().MakeMoneyEverywhere();
			timeOfLastAd = Time.time;
			adCounter = 0;
		}
		Debug.Log("Giving go ahead for reset");
		doReset = true;
	}
}
