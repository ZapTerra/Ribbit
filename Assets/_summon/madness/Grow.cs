using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += Vector3.one * Time.deltaTime/3;
        transform.Rotate(Vector3.up, Time.deltaTime * 30);
	}
}
