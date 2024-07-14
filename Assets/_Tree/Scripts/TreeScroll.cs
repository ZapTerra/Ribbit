using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;

public class TreeScroll : MonoBehaviour {
	public bool holdingBeetle = false;
	public float camRotationSpeed = .1f;
	public float camVertSpeed = .1f;
	public float vertMoveAtGroundResistance = 4;
	public float rotationMomentumDecay = .01f;
	public float vertMomentumDecay = .01f;
	public float minVertCoord = -30;
	public float maxVertCoord = 40;
	public float groundAngleStart = -30;
	public float groundAngleEnd = -40;
	public float groundAngleAmount = 45;
	public TMPro.TextMeshProUGUI nameDisplay;
	public Transform cameraParentLad;
	private Vector3 initState;
	private Vector3 SwipePos;
	private Vector3 keyframeSwipePos;
	public float cameraRotationMomentum;
	public float cameraVertMomentum;
	private bool objectLayerIsDefault = true;
	public GameObject canopy;
	public GameObject treeBody;
	public GameObject terrain;
	private int scrollMomentumKeyframe = 0;
	// Start is called before the first frame update
	void Start() {
		nameDisplay.text = "";
		Debug.Log("Build problems? Maybe try uncommenting header");
	}

	void Update() {
		if (cameraParentLad.transform.position.y > 20 && !objectLayerIsDefault) {
			SetGraphicsLayers("Default");
			objectLayerIsDefault = true;
		} else if (cameraParentLad.transform.position.y < 20 && objectLayerIsDefault) {
			SetGraphicsLayers("Overlay");
			objectLayerIsDefault = false;
		}
		if (!holdingBeetle) {
			if (Input.GetMouseButtonDown(0)) {
				SwipePos = Input.mousePosition;
				initState = cameraParentLad.eulerAngles;
			}

			if (Input.GetMouseButton(0)) {
				cameraRotationMomentum = (SwipePos.x - Input.mousePosition.x) / Screen.width * 400f * camRotationSpeed;
				cameraVertMomentum = (SwipePos.y - Input.mousePosition.y) / Screen.height * 400f * camVertSpeed;
				// if (Mathf.Abs(cameraVertMomentum) < Mathf.Abs(cameraRotationMomentum) * (camVertSpeed / camRotationSpeed * .1)) {
				// 	cameraVertMomentum = 0;
				// }
				// if (Mathf.Abs(cameraRotationMomentum) < Mathf.Abs(cameraVertMomentum) * (camRotationSpeed / camVertSpeed * .35)) {
				// 	cameraRotationMomentum = 0;
				// }
				SwipePos = Input.mousePosition;
				if (scrollMomentumKeyframe % 6 == 0) {
					scrollMomentumKeyframe = 0;
					keyframeSwipePos = SwipePos;
				}
				scrollMomentumKeyframe++;
			}

			if (Input.GetMouseButtonUp(0)) {
				scrollMomentumKeyframe = 0;
				cameraRotationMomentum = ((keyframeSwipePos.x - Input.mousePosition.x) / Screen.width * 100f) * camRotationSpeed;
				cameraVertMomentum = ((keyframeSwipePos.y - Input.mousePosition.y) / Screen.height * 100f) * camVertSpeed;
				// if (Mathf.Abs(cameraVertMomentum) < Mathf.Abs(cameraRotationMomentum) * (camVertSpeed / camRotationSpeed * .1)) {
				// 	cameraVertMomentum = 0;
				// }
				// if (Mathf.Abs(cameraRotationMomentum) < Mathf.Abs(cameraVertMomentum) * (camRotationSpeed / camVertSpeed * .35)) {
				// 	cameraRotationMomentum = 0;
				// }
			} else {
				cameraRotationMomentum = Mathf.Lerp(cameraRotationMomentum, 0, rotationMomentumDecay);
				cameraVertMomentum = Mathf.Lerp(cameraVertMomentum, 0, vertMomentumDecay);
			}
		} else {
			cameraRotationMomentum = Input.mousePosition.x < Screen.width * .2f ? -camRotationSpeed * 2.5f : Input.mousePosition.x > Screen.width * .8f ? camRotationSpeed * 2.5f : 0;
			cameraRotationMomentum *= Input.mousePosition.x < Screen.width * .1f || Input.mousePosition.x > Screen.width * .9f ? 4 : 1;

			cameraVertMomentum = Input.mousePosition.y < Screen.height * .2f ? -camVertSpeed * 2.5f : Input.mousePosition.y > Screen.height * .8f ? camVertSpeed * 2.5f : 0;
			cameraVertMomentum *= Input.mousePosition.y < Screen.height * .1f || Input.mousePosition.y > Screen.height * .9f ? 4 : 1;
			cameraVertMomentum *= Input.mousePosition.y < Screen.height * .05f || Input.mousePosition.y > Screen.height * .95f ? 4 : 1;
			Debug.Log("SPIN");
		}

		cameraParentLad.eulerAngles = new Vector3(cameraParentLad.eulerAngles.x, cameraParentLad.eulerAngles.y - cameraRotationMomentum, cameraParentLad.eulerAngles.z);

		cameraParentLad.transform.position = new Vector3(cameraParentLad.transform.position.x, Mathf.Clamp(cameraParentLad.transform.position.y + cameraVertMomentum, minVertCoord, maxVertCoord), cameraParentLad.transform.position.z);

		transform.localEulerAngles = new Vector3((transform.position.y < groundAngleStart ? ((groundAngleStart - transform.position.y) / (groundAngleStart - groundAngleEnd)) * groundAngleAmount : 0), 0, 0);
	}

	void SetGraphicsLayers(string layer) {
		foreach (Transform child in canopy.transform)
			foreach (Transform grandchild in child.transform)
				grandchild.gameObject.layer = LayerMask.NameToLayer(layer);
		treeBody.layer = LayerMask.NameToLayer(layer);
		terrain.layer = LayerMask.NameToLayer(layer);
	}

	public void DisplayName(string name){
		nameDisplay.text = "~ " + name + " ~";
	}

	public void StopDisplayingName(){
		nameDisplay.text = "";
	}
}
