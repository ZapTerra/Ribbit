using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour {
	public List<Vector3> bezierPoints;
	public float fadeInTime;
	public float opacityCap;
	public float rotationSpeed;
	public float rotationVariance;
	private float bezierTime;
	public float bezierLengthInSeconds;
	public int curvePoints = 4;
	public float chaos = 250;
	public float cornerDistanceX = 100;
	public float cornerDistanceY = 50;

	void Start() {
		//alter rotation speed randomly within variance set and randomly flip the direction of rotation
		rotationSpeed += Random.Range(-rotationVariance / 2, rotationVariance / 2);
		rotationSpeed *= Random.Range(0, 2) * 2 - 1;

		//make invisible
		var tempColor = GetComponent<SpriteRenderer>().color;
		tempColor.a = 0;
		GetComponent<SpriteRenderer>().color = tempColor;

		//Bezier starts at current position
		bezierPoints.Add(transform.position);

		//This is very long, but I'm going to pretend that's okay. :)
		//Add a bunch of random points on the screen to the list for the Bezier movement.
		for (int i = 0; i < curvePoints; i++) {
			newPoint();
		}

		//Bezier ends at lower left corner of screen.
		bezierPoints.Add(new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(cornerDistanceX, cornerDistanceY)).x, Camera.main.ScreenToWorldPoint(new Vector2(cornerDistanceX, cornerDistanceY)).y, transform.position.z));
	}

	void Update() {
		//spin
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);

		//fadein
		if (GetComponent<SpriteRenderer>().color.a < opacityCap) {
			var tempColor = GetComponent<SpriteRenderer>().color;
			tempColor.a += Time.deltaTime / fadeInTime;
			GetComponent<SpriteRenderer>().color = tempColor;
		}

		//progress along bezier curve according to time in seconds defined
		bezierTime += Time.deltaTime / bezierLengthInSeconds;

		//Bezier ends at lower left corner of screen.
		bezierPoints[bezierPoints.Count - 1] = new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(cornerDistanceX, cornerDistanceY)).x, Camera.main.ScreenToWorldPoint(new Vector2(cornerDistanceX, cornerDistanceY)).y, transform.position.z);

		//clamp to screen
		var x = Mathf.Clamp(BezierCurve.Point3(bezierTime, bezierPoints).x, Camera.main.ScreenToWorldPoint(Vector3.zero).x, Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x);
		var y = Mathf.Clamp(BezierCurve.Point3(bezierTime, bezierPoints).y, Camera.main.ScreenToWorldPoint(Vector3.zero).y, Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y);
		transform.position = new Vector3(x, y, transform.position.z);

		//if at end of path, destroy
		if (bezierTime >= 1) {
			Destroy(gameObject);
		}
	}

	private void newPoint() {
		bezierPoints.Add(
			 Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(-chaos, Screen.width + chaos), Random.Range(-chaos, Screen.height + chaos), Camera.main.farClipPlane / 2))
		);
	}
}
