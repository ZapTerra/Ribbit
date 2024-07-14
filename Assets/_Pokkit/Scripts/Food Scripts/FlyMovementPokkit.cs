using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FlyMovementPokkit : MonoBehaviour {

	public PathCreator entryPath;
	public PathCreator flightPath;
	private float entryDistance;
	private float pathDistance;
	public float flySpeed;
	private bool operationCondorIsAGo;
	void Start() {
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
	}

	void Update() {
		if (entryDistance > 1) {
			pathDistance += Time.deltaTime * flySpeed;
			while (pathDistance > 1) {
				pathDistance--;
			}
			transform.position = flightPath.path.GetPointAtTime(pathDistance);
		} else {
			entryDistance += Time.deltaTime * flySpeed;
			transform.position = entryPath.path.GetPointAtTime(entryDistance);
			if (entryDistance > 1) {
				transform.position = flightPath.path.GetPointAtTime(0);
			}
		}
		if (!operationCondorIsAGo) {
			GetComponent<SpriteRenderer>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = true;
			operationCondorIsAGo = true;
		}

	}

}
