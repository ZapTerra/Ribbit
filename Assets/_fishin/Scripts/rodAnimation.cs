using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class rodAnimation : MonoBehaviour {
	public AudioManager audioManager;
	public GameObject castAreaObject;
	public GameObject bobeObject;
	public GameObject frogeObject;
	public GameObject dragGraphic1;
	public GameObject dragGraphic2;
	public GameObject dragGraphicCenter;
	public Vector2 clickPos1;
	public Vector2 clickPos2;
	public Vector2 castDir;
	public float maxRodRotate = 90;
	public float maxCastDragDistance = 3.5f;
	public float rotationSpeed = 4;
	public float rotationSpeedBack = 8;
	public float distToNoticeCast = .5f;
	public float castDistMultiplier = 1f;
	private float cursorAngle;
	private float targetRotation;
	private float myAngle;
	private float targetRotationDifference;
	private int rotationOverload = 0;
	public float clickDistanceForCast;
	public float rodRotation;
	private bool hasStartedRotation;
	public bool canHasCastOnRelease = false;

	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {
		if (!panelOpener.paused) {
			if (frogeObject.GetComponent<frogeFisheMove>().frogeState == 0) {
				if (Input.GetMouseButtonDown(0)) {
					Collider2D hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
					if (hit != null && hit.transform == castAreaObject.transform) {
						clickPos1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						dragGraphic1.SetActive(true);
						dragGraphic1.transform.position = clickPos1;
						canHasCastOnRelease = true;
						hasStartedRotation = false;
						bobeObject.GetComponent<bobeCast>().returnToFather = true;
					}
				}

				if (canHasCastOnRelease) {
					clickPos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					clickDistanceForCast = Mathf.Clamp(Vector2.Distance(clickPos1, clickPos2), 0, maxCastDragDistance);

					if (clickDistanceForCast > distToNoticeCast) {
						Camera.main.GetComponent<smoothCamera>().target2 = dragGraphic2.transform;
						castDir = (clickPos2 - clickPos1).normalized;
						cursorAngle = Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg + 90f;

						if (!hasStartedRotation) {
							myAngle = frogeObject.transform.eulerAngles.z;
							targetRotation = Mathf.DeltaAngle(myAngle, cursorAngle);
							hasStartedRotation = true;
						}

						//add the difference between last frame's target rotation and this frame's target rotation,
						//and subtract how much the frog has moved.
						//This maintains the current direction of rotation while also maintaining the amount of needed rotation.
						var frogChange = Mathf.DeltaAngle(myAngle, frogeObject.transform.eulerAngles.z);

						myAngle += frogChange;
						targetRotation += Mathf.DeltaAngle(myAngle + targetRotation, cursorAngle) - frogChange;

						//if you don't want to tell the frog it should rotate by more than 360 degrees.
						//while (Mathf.Abs(targetRotation) > 360) {
						//targetRotation -= Mathf.Sign(targetRotation) * 360;
						//}
						//while (Mathf.Abs(myAngle) > 360) {
						//myAngle -= Mathf.Sign(targetRotation) * 360;
						//}

						var newRotation = Mathf.Lerp(myAngle, myAngle + targetRotation, Time.deltaTime * rotationSpeed);
						frogeObject.transform.rotation = Quaternion.Euler(newRotation * Vector3.forward);
					}

					dragGraphic2.SetActive(true);
					dragGraphicCenter.SetActive(true);

					dragGraphic2.transform.position = clickPos1 + Vector2.ClampMagnitude((clickPos2 - clickPos1), maxCastDragDistance);
					dragGraphicCenter.transform.position = (dragGraphic1.transform.position + dragGraphic2.transform.position) / 2;
					dragGraphicCenter.transform.localScale = new Vector3(dragGraphicCenter.transform.localScale.x, Vector3.Distance(dragGraphic1.transform.position, dragGraphic2.transform.position), dragGraphicCenter.transform.localScale.z);
					dragGraphicCenter.transform.rotation = Quaternion.FromToRotation(Vector3.up, dragGraphic1.transform.position - dragGraphic2.transform.position);

					rodRotation = clickDistanceForCast * (maxRodRotate / maxCastDragDistance) * -1;
				} else if (dragGraphic1.activeInHierarchy == true) {
					dragGraphic1.SetActive(false);
					dragGraphic2.SetActive(false);
					dragGraphicCenter.SetActive(false);
				}

				if (Input.GetMouseButtonUp(0)) {
					if (canHasCastOnRelease && clickDistanceForCast > distToNoticeCast) {
						audioManager.Play("BobWhoosh" + Random.Range(1, 3));
						var bobeCastScript = bobeObject.GetComponent<bobeCast>();
						bobeCastScript.castDist = clickDistanceForCast * castDistMultiplier;
						bobeCastScript.goMeLaddie = true;
					}
					rodRotation = 0;
					clickDistanceForCast = 0;
					canHasCastOnRelease = false;
				}
				transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(new Vector3(0, 0, rodRotation)), Time.deltaTime * (rodRotation != 0 ? rotationSpeed : rotationSpeedBack));
			} else {
				targetRotation = frogeObject.transform.eulerAngles.z;
			}
		}
	}
}