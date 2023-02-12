using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustScale : MonoBehaviour {

	public int rarity;
	float timer;
	[SerializeField]
	GameObject sacrifice1;
	[SerializeField]
	GameObject sacrifice2;
	[SerializeField]
	GameObject sacrifice3;
	[SerializeField]
	GameObject sacrifice4;
	[SerializeField]
	GameObject sacrifice5;
	[SerializeField]
	bool sacrificed;
	[SerializeField]
	Color gold;
	[SerializeField]
	Color silver;
	[SerializeField]
	Color bronze;
	[SerializeField]
	Color forestgreen;
	[SerializeField]
	Material mat;
	[SerializeField]
	Material mat2;

	// Use this for initialization
	void Start() {
		mat.SetColor("_Color", gold);
		mat2.SetColor("_RimColor", gold);
		rarity = TritterGacha.mostRecentPull[0].rarity;
	}

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;
		if (timer > 0f && !sacrificed) {
			if (rarity < 5) {
				sacrifice3.SetActive(false);
				mat.SetColor("_Color", silver);
				mat2.SetColor("_RimColor", silver);
			}
			if (rarity < 4) {
				sacrifice2.SetActive(false);
				sacrifice4.SetActive(false);
				sacrifice3.SetActive(true);
				mat.SetColor("_Color", bronze);
				mat2.SetColor("_RimColor", bronze);
			}
			if (rarity < 3) {
				sacrifice1.SetActive(false);
				sacrifice3.SetActive(false);
				sacrifice5.SetActive(false);
				sacrifice2.SetActive(true);
				sacrifice4.SetActive(true);
				transform.position += Vector3.up;
				mat.SetColor("_Color", forestgreen);
				mat2.SetColor("_RimColor", forestgreen);
			}
			if (rarity < 2) {
				sacrifice1.SetActive(false);
				sacrifice2.SetActive(false);
				sacrifice4.SetActive(false);
				sacrifice5.SetActive(false);
				sacrifice3.SetActive(true);
				transform.position -= Vector3.up;
				mat.SetColor("_Color", Color.black);
				mat2.SetColor("_RimColor", gold);
			}
			sacrificed = true;
		}
		// transform.localScale = Vector3.one * (Camera.main.orthographicSize / 5);

	}
}
