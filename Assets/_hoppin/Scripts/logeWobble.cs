using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logeWobble : MonoBehaviour {
	public Animator animator;
	public float rotationSpeed = 10;
	public float rotationMagnitude = 6;
	public float startOnSecondsLeft = 1;
	public float timeOnLoge = 3;
	public float timeToZeroRotation = .1f;
	private Transform frogge;
	// Start is called before the first frame update
	void Start() {
		animator.SetBool("IsSinking", false);
	}

	// Update is called once per frame
	void Update() {
		frogge = GameObject.Find("frogeNew").GetComponent<Transform>();
		if (transform.parent.GetComponentInChildren<FrogeMove>() != null) {
			timeOnLoge -= Time.deltaTime;
			if (timeOnLoge <= startOnSecondsLeft) {
				transform.eulerAngles = Vector3.forward * Mathf.Sin((timeOnLoge - startOnSecondsLeft) * rotationSpeed) * rotationMagnitude;
				if(!animator.GetBool("IsSinking")){
					Debug.Log("Vibrate, loge wobble");
					Vibration.VibratePeek();
				}
				animator.SetBool("IsSinking", true);
			}
			if (timeOnLoge <= 0) {
				frogge.parent = null;
				Destroy(transform.parent.gameObject);
			}
		} else {
			StartCoroutine(AnimateRotationTowards(this.transform, Quaternion.identity, timeToZeroRotation));
		}
	}
	private System.Collections.IEnumerator AnimateRotationTowards(Transform target, Quaternion rot, float dur) {
		float t = 0f;
		Quaternion start = target.rotation;
		while (t < dur) {
			target.rotation = Quaternion.Slerp(start, rot, t / dur);
			yield return null;
			t += Time.deltaTime;
		}
		target.rotation = rot;
	}

}
