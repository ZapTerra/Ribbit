using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpFade : MonoBehaviour {


    public float offset;
    float timer;
    Color col2;
    public bool hghbvvv;
    SpriteRenderer sr;
    Vector3 homepos;
	// Use this for initialization
	void Start () {
        timer = (-offset);
        sr = GetComponent<SpriteRenderer>();
        col2 = new Color(1, 1, 1, 0);
        sr.color = col2;
        homepos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer>0)
        {
            transform.position += Vector3.up * Time.deltaTime;
            sr.color = Color.Lerp(Color.white, col2, timer);
        }

	}
}
