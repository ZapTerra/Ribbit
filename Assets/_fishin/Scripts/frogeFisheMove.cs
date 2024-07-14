using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogeFisheMove : MonoBehaviour {
	public AudioManager audioManager;
	public Animator animator;
	public GameObject startingCenterLilie;
	public Transform targetLilie;
	public int frogeState = 0;
	public int jumpX;
	public int jumpY;
	public float jumpHeight = 1;
	public float jumpSpeed = 1;
	private float jumpTravel;
	public float distanceLeft;
	private Rigidbody rb;
	// Start is called before the first frame update
	void Start() {
		animator.SetInteger("frogeState", 0);
		frogeState = 0;
	}

	// Update is called once per frame
	void Update() {
		if (!panelOpener.paused) {
			if (Input.GetMouseButtonDown(0) && frogeState == 0) {
				Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
				if (hit != null && hit.gameObject.tag == "liliePade") {
					audioManager.Play("FrogLeap");
					animator.SetInteger("frogeState", 1);
					frogeState = 1;
					var collisionScript = hit.gameObject.GetComponent<liliePade>();
					targetLilie.position = hit.transform.position;
					jumpX = collisionScript.arrayPosX;
					jumpY = collisionScript.arrayPosY;
					distanceLeft = Vector2.Distance(targetLilie.position, transform.position);
					transform.up = targetLilie.position - transform.position;
				}
			}
			if (distanceLeft > 0) {
				jumpTravel = Mathf.Clamp(Time.deltaTime * jumpSpeed, 0, distanceLeft);
				distanceLeft -= jumpTravel;
				transform.position = transform.position + transform.up * jumpTravel;
				if (distanceLeft < 2) {
					animator.SetInteger("frogeState", 0);
				}
				if (distanceLeft <= 0) {
					audioManager.Play("FrogLand");
					frogeState = 0;
				}
			}
		}
	}

	private void LateUpdate() {
		transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
	}
}
