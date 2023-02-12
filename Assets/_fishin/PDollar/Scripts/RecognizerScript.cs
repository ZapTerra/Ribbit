using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using PDollarGestureRecognizer;

public class RecognizerScript : MonoBehaviour {
	public AudioManager audioManager;

	public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Vector3 lastScreenPos;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	public string type;
	private bool recognized;

	private string newGestureName = "";

	public delegate void ShapeDrawnDelegate(string type);
	public static event ShapeDrawnDelegate OnShapeDrawn;

	private bool enterDrawArea;

	void OnEnable() {
		Debug.Log("If you're looking for the fish bite sound effect trigger, it's here!");
		audioManager.Play("Bite");
	}

	void Start() {
		platform = Application.platform;
		drawArea = new Rect(0, 0, 2 * (UnityEngine.Screen.width - UnityEngine.Screen.width / 3), 2 * UnityEngine.Screen.height);

		//Load user custom gestures
		TextAsset[] gestureAssets = Resources.LoadAll<TextAsset>("GestureSet");
		foreach (TextAsset gestureAsset in gestureAssets) {
			trainingSet.Add(GestureIO.ReadGesture(new XmlTextReader(new StringReader(gestureAsset.text))));
		}
	}

	void Update() {
		if (Time.timeScale != 0) {
			try {
				if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
					if (Input.touchCount > 0) {
						virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
					} else {
						Destroy(currentGestureLineRenderer);
					}
				} else {
					if (Input.GetMouseButton(0)) {
						virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
					} else {
						Destroy(currentGestureLineRenderer);
					}
				}

				if (drawArea.Contains(virtualKeyPosition)) {

					if (Input.GetMouseButtonDown(0) || enterDrawArea == false) {
						enterDrawArea = true;
						if (recognized) {

							recognized = false;
							strokeId = -1;

							points.Clear();

							gestureLinesRenderer.Clear();
						}

						++strokeId;

						Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
						currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

						gestureLinesRenderer.Add(currentGestureLineRenderer);

						vertexCount = 0;
					}
					if (Input.GetMouseButton(0) && (virtualKeyPosition - lastScreenPos).magnitude > 0) {
						points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

						currentGestureLineRenderer.SetVertexCount(++vertexCount);
						currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
					}
				}
				if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)) {
					GetSymbol();
				}
				lastScreenPos = virtualKeyPosition;
			} catch {

			}
		}
	}

	public void GetSymbol() {
		if (enterDrawArea) {
			if(points.Count > 2)
			{
				recognized = true;
				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
				message = gestureResult.GestureClass + " " + gestureResult.Score;
				Debug.Log(message);
				Debug.Log("^This is a symbol and confidence level");
				type = gestureResult.GestureClass;
				OnShapeDrawn?.Invoke(type);

				foreach (LineRenderer lineRenderer in gestureLinesRenderer) {
					//Not sure why the null check is necessary, but it is.
					if(lineRenderer != null){
						lineRenderer.SetVertexCount(0);
						Destroy(lineRenderer.gameObject);
					}
				}

				enterDrawArea = false;
			}
		}
	}

	// public void WriteGesture(String type) {
	// 	//SAVE USER GESTURE ATTEMPT TO TRAINING DATASET
	// 	if (points.Count > 0) {
	// 		string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, type, DateTime.Now.ToFileTime());
	// 		GestureIO.WriteGesture(points.ToArray(), type, fileName);
	// 		Debug.Log(Application.persistentDataPath);
	// 	}
	// }

	private void OnDisable() {
		recognized = false;
		strokeId = -1;

		points.Clear();

		gestureLinesRenderer.Clear();
		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {
			lineRenderer.SetVertexCount(0);
			Destroy(lineRenderer.gameObject);
		}
		Destroy(currentGestureLineRenderer);
	}
}
