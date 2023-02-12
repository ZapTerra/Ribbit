using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabJaundice : MonoBehaviour {
	public GameObject loge;
	// Start is called before the first frame update
	void Start() {
		loge = (GameObject)Resources.Load("Assets/hoppin/Prefabs/logeSingular.prefab");
	}
}
