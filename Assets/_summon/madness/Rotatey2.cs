using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotatey2 : MonoBehaviour {

	public bool starty;
	public bool endy;
	public AudioSource hitEffect;
	float timer;
	public float offset;
	public float hitDelay;
	float holdy;
	bool left;
	float timer2;
	float timer3;
	Vector3 home;
	private bool havePlayedHitEffect;
	// Use this for initialization
	void Start() {
		home = transform.position;
		timer = 0;
		//GetComponent<Renderer>().sortingOrder = 10000;

	}

	// Update is called once per frame
	void Update() {
		timer3 += Time.deltaTime;
		if(timer3 > hitDelay)
		{
			if(!havePlayedHitEffect)
			{
				hitEffect.Play();
				Debug.Log("Vibrate, star hit");
				Vibration.VibratePop();
				havePlayedHitEffect = true;
			}
		}
		if (timer3 >= 4.5f) {
			timer += Time.deltaTime;
			if (timer3 > 4.5f + offset) {
				transform.position = home + Vector3.up * (Mathf.Max(0, Mathf.Sin(-timer * 4 + offset * 10 + 3.1f) / 4)) + (transform.parent.gameObject.GetComponent<AdjustScale>().rarity == 2 ? 1 : 0) * transform.parent.localPosition.y * Vector3.up * .4f;
			}

			if (!left) {
				if (starty == true) {
					if (timer > 2 && timer < 2.6f) {
						transform.Rotate(Vector3.forward, Time.deltaTime * 120 * (0.1f + Inout((timer - 2) * 2)));
					}

				}

				if (timer > 2.6f + offset) {
					if (holdy < 640) {
						transform.Rotate(Vector3.forward, Time.deltaTime * (-900 + holdy / 8));
						holdy += Time.deltaTime * (900 - holdy / 8);
					}


					if (holdy > 640) {
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.left * (90 + 360)), Time.deltaTime * 6);
						timer2 += Time.deltaTime;
						if (timer2 > 0.45) {
							timer -= Mathf.PI * 2;
							timer2 = 0;
							holdy = 0;
						}
					}
				}
			}

			if (left) {
				if (endy == true) {
					if (timer > 2 && timer < 2.6f) {
						transform.Rotate(Vector3.forward, Time.deltaTime * -120 * (0.1f + Inout((timer - 2) * 2)));
					}

				}

				if (timer > 2.6f + 0.4 - offset) {
					if (holdy < 640) {
						transform.Rotate(Vector3.forward, Time.deltaTime * (900));
						holdy += Time.deltaTime * (900 - holdy / 8);
					}


					if (holdy > 640) {
						transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.left * 90), Time.deltaTime * 6);
						timer2 += Time.deltaTime;
						if (timer2 > 0.45 - (0.4 - offset)) {
							timer = 2;
							timer2 = 0;
							holdy = 0;
							left = false;
						}
					}
				}
			}
		}
		//transform.Rotate(Vector3.forward, Time.deltaTime * 70);
	}

	private float Inout(float x) {
		return Mathf.Max(0, -Mathf.Pow((x * 2 - 1), 2) + 1);
	}
}
