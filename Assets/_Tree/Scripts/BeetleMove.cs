using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleMove : MonoBehaviour {
	public AudioManager audioManager;
	public float timeToPickUp = .5f;
	public float maxMouseMoveToPickup = 60f;
	public float moveSpeed;
	public float oddsToNotWait = 6;
	public float maxWaitTime = 5;
	public float maxJourneyRotate = 360;
	public float maxJourneyDistance = 5;
	public float fallSpeed = 1;
	public float treeHeight = 40;
	public float groundTrunkKeepawayRadius = 3;
	public float groundTrunkMaxRadius = 6;
	public float groundPlotSize = 16;
	public float trunkXPos;
	public float trunkYPos;
	public float rotation;
	public bool crawlOnTree;
	public static bool someoneIsHold;
	public Transform rotationParent;
	public GameObject pickupPlane;
	public GameObject shadow;
	public Tritter tritterData;
	private float trunkRadiusForCrawlDistance = -2.25f;
	private float initRotation;
	private float journeyDistanceTravelled;
	private float journeyDistance;
	private float timeSinceLastSave;
	private bool freeFalling;
	// Start is called before the first frame update
	void Start() {
		audioManager = FindObjectOfType<AudioManager>();
		StartCoroutine(Shmoovin(Random.Range(-maxJourneyRotate, maxJourneyRotate), Random.Range(0, maxJourneyDistance)));
		shadow.SetActive(false);
		someoneIsHold = false;
		crawlOnTree = tritterData.crawlOnTree;
        rotation = tritterData.rotation;
        trunkXPos = tritterData.trunkXPos;
        trunkYPos = tritterData.trunkYPos;
		PosUpdate();
	}

	// Update is called once per frame
	void Update() {
		//If the camera is above the middle of the tree and the beetle is behind the tree relative to the camera, render it behind the tree.
		if(((Mathf.Abs(Mathf.DeltaAngle(rotationParent.eulerAngles.y, Camera.main.transform.parent.eulerAngles.y)) <= 80) || Camera.main.transform.parent.position.y < 20)){
			if(gameObject.layer == LayerMask.NameToLayer("Default")){
				SetGraphicsLayers("Overlay");
			}
		}else if(Mathf.Abs(Mathf.DeltaAngle(rotationParent.eulerAngles.y, Camera.main.transform.parent.eulerAngles.y)) > 80){
			if(gameObject.layer == LayerMask.NameToLayer("Overlay")){
				SetGraphicsLayers("Default");
			}
		}

		timeSinceLastSave += Time.deltaTime;
        if(timeSinceLastSave >= 1){
            tritterData.rotation = rotation;
            tritterData.crawlOnTree = crawlOnTree;
            tritterData.trunkXPos = trunkXPos;
            tritterData.trunkYPos = trunkYPos;
        }
	}
	
	void SetGraphicsLayers(string layer) {
		foreach (Transform child in transform){
			child.gameObject.layer = LayerMask.NameToLayer(layer);
			foreach (Transform grandchild in child.transform)
				grandchild.gameObject.layer = LayerMask.NameToLayer(layer);
		}
		gameObject.layer = LayerMask.NameToLayer(layer);
	}

	void PosUpdate() {
		trunkXPos += Mathf.Cos(rotation * Mathf.Deg2Rad) * moveSpeed * Time.deltaTime;
		trunkYPos += Mathf.Sin(rotation * Mathf.Deg2Rad) * moveSpeed * Time.deltaTime;
		if (crawlOnTree) {
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rotation - 90);
			rotationParent.eulerAngles = new Vector3(rotationParent.eulerAngles.x, (trunkXPos / 7.2f) * -Mathf.PI * Mathf.Rad2Deg, rotationParent.eulerAngles.z);
			transform.position = new Vector3(transform.position.x, Mathf.Clamp(trunkYPos, -treeHeight + 1, treeHeight - transform.localScale.y / 2), transform.position.z);
		} else {
			//move the beetle to xy ground positon, clamping it

			//HEY YOU
			//MAKE IT SO THIS WILL CLAMP TO YOUR CURRENT X/Y DISTANCE FROM THE CENTER INSTEAD IF YOU ARE CURRENTLY OUTSIDE OF THE RADIUS SO YOU CAN'T GET SUDDENTLY [sic] TELEPORTED


			trunkXPos = Mathf.Clamp(Mathf.Abs(trunkXPos), Mathf.Abs((new Vector2(trunkXPos, trunkYPos).normalized * groundTrunkKeepawayRadius).x), Mathf.Abs((new Vector2(trunkXPos, trunkYPos).normalized * groundTrunkMaxRadius).x)) * Mathf.Sign(trunkXPos);
			trunkYPos = Mathf.Clamp(Mathf.Abs(trunkYPos), Mathf.Abs((new Vector2(trunkXPos, trunkYPos).normalized * groundTrunkKeepawayRadius).y), Mathf.Abs((new Vector2(trunkXPos, trunkYPos).normalized * groundTrunkMaxRadius).y)) * Mathf.Sign(trunkYPos);
			transform.position = new Vector3(trunkXPos, -treeHeight + .1f, trunkYPos);
			transform.eulerAngles = new Vector3(90f, 0f, rotation - 90);
		}
	}

	void OnMouseDown() {
		StartCoroutine(MouseDown());
	}

	IEnumerator MouseDown() {
		var mouseDownPos = Input.mousePosition;
		yield return StartCoroutine(Wait(timeToPickUp));
		if (Input.GetMouseButton(0) && (Input.mousePosition - mouseDownPos).magnitude < maxMouseMoveToPickup && !someoneIsHold && !freeFalling) {
			StopAllCoroutines();
			StartCoroutine(FollowMouse());
		}
		yield return null;
	}

	IEnumerator Wait(float time) {
		yield return new WaitForSeconds(time);
	}

	IEnumerator Shmoovin(float rotationGoal, float distanceGoal) {
		yield return StartCoroutine(Wait(Random.Range(0, oddsToNotWait) < 1 ? 0 : Random.Range(0, maxWaitTime)));
		initRotation = rotation;
		journeyDistance = distanceGoal;
		journeyDistanceTravelled = 0;
		while (journeyDistanceTravelled < journeyDistance) {
			rotation = initRotation + rotationGoal * (journeyDistanceTravelled / distanceGoal);
			journeyDistanceTravelled += moveSpeed * Time.deltaTime;
			PosUpdate();
			yield return null;
		}
		StartCoroutine(Shmoovin(Random.Range(-360, 360), Random.Range(0, 5)));
	}

	IEnumerator FollowMouse() {

		someoneIsHold = true;

		shadow.SetActive(true);
		transform.parent = Camera.main.gameObject.transform;
		Camera.main.GetComponent<TreeScroll>().holdingBeetle = true;
		Camera.main.GetComponent<TreeScroll>().DisplayName(tritterData.species);

		Ray ray;
		RaycastHit hit;
		while (Input.GetMouseButton(0)) {
			yield return null;
			rotationParent.eulerAngles = new Vector3(rotationParent.eulerAngles.x, transform.parent.eulerAngles.y, rotationParent.eulerAngles.z);
			rotationParent.position = new Vector3(rotationParent.position.x, transform.parent.position.y - ((transform.parent.eulerAngles.x / transform.parent.gameObject.GetComponent<TreeScroll>().groundAngleAmount)) * ((treeHeight - transform.localScale.y / 2) + transform.parent.gameObject.GetComponent<TreeScroll>().minVertCoord), rotationParent.position.z);
			//The minimum vertical coordinate for the camera is added instead of subtracted becasue [sic] it is expected to be negative
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (pickupPlane.GetComponent<Collider>().Raycast(ray, out hit, 100f)) {
				transform.position = hit.point;
				Debug.Log(hit.distance);
				transform.rotation = transform.parent.rotation;
			}
		}

		someoneIsHold = false;

		Camera.main.GetComponent<TreeScroll>().holdingBeetle = false;
		Camera.main.GetComponent<TreeScroll>().StopDisplayingName();

		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (shadow.GetComponent<Collider>().Raycast(ray, out hit, 100f)) {
			Debug.Log("Vibrate, tritter placed");
			Vibration.VibratePop();
			shadow.SetActive(false);
			transform.parent = rotationParent;
			transform.localRotation = Quaternion.identity;
			transform.localPosition = new Vector3(0, 0, trunkRadiusForCrawlDistance);
			trunkXPos = ((rotationParent.eulerAngles.y / Mathf.Rad2Deg) / -Mathf.PI) * 7.2f;
			trunkYPos = rotationParent.position.y;
			rotation = 0;
			crawlOnTree = true;
		} else {
			shadow.SetActive(false);
			transform.parent = null;
			crawlOnTree = false;
			trunkXPos = transform.position.x;
			trunkYPos = transform.position.z;

			//THIS BUGGY IF BEETLE HASNT MOVEd
			//THIS BUGGY IF BEETLE HASNT MOVEd[sic[
				//I don't know what this means, looks fine to me
			//falls until it hits the ground once dropped
			freeFalling = true;
			audioManager.Play("Fall");
			while (transform.position.y > -treeHeight + .1f) {
				transform.position = Vector3.up * -fallSpeed * Time.deltaTime + transform.position;
				transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -39.9f, Mathf.Infinity), transform.position.z);
				yield return null;
			}
			audioManager.Play("Thud");
			freeFalling = false;
			transform.eulerAngles = new Vector3(90f, 0f, rotation + 180);
		}

		StartCoroutine(Shmoovin(Random.Range(-maxJourneyRotate, maxJourneyRotate), Random.Range(0, maxJourneyDistance)));

		yield return null;
	}
}
