using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rowMaker : MonoBehaviour {
	public int rowsAhead = 10;
	public int rows;
	public float logeMoveSpeed = 5f;
	public float logeMoveSpeedVariance = 2.5f;
	public float rowSpacing = 2;
	public GameObject loge;
	public GameObject froge;

	private int rowsSpawned = 0;
	private GameObject newLoge;
	// Start is called before the first frame update
	void Start() {
		rows = rowsAhead;
		spawnRows();
	}

	// Update is called once per frame
	void Update() {
		rows = rowsAhead + Mathf.FloorToInt(froge.transform.position.y / rowSpacing);
		spawnRows();
	}

	void spawnRows() {
		while (rowsSpawned < rows) {
			rowsSpawned++;

			newLoge = Instantiate(loge);
			newLoge.transform.position = new Vector3(froge.transform.position.x + Random.Range(-3, 3), rowsSpawned * rowSpacing, 0);
			LogeMove[] createdLogeChildScripts = newLoge.GetComponentsInChildren<LogeMove>();
			float logeRowVariance = Random.Range(-logeMoveSpeedVariance, logeMoveSpeedVariance);
			foreach (LogeMove l in createdLogeChildScripts) {
				l.moveSpeed = logeMoveSpeed + logeRowVariance;
			}
		}
	}
}
