using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySetGo : MonoBehaviour {

    public float offset;
    float timer;

    public float size;
    float testval;
    Vector3 finalsize;
    bool stoprot;
    public bool justgrow;
    SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        timer = -offset;
        finalsize = Vector3.one * size;
        sr = GetComponent<SpriteRenderer>();
        if (justgrow)
        {
            sr.color = Color.white;

        }
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer<=1)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, finalsize, 1 - Mathf.Pow(Mathf.Clamp01(1 - timer), 4));

        }
        if (timer>2f && timer<3f)
        {
            sr.color = Color.white;
        }

        if (timer>1.5f)
        {

            float intensity = Time.deltaTime * 0.2f * Mathf.Pow((timer-0.5f), 4);
            if (transform.position.y>-1)
                transform.position -= Vector3.up * intensity;
            if (stoprot==false && justgrow == false)
            {
                transform.localEulerAngles -= Vector3.one * intensity;
                testval += intensity;
            }

            if (testval > 180 && justgrow == false)
            {
                transform.localEulerAngles = Vector3.right*90;
                stoprot = true;
            }

            if (testval>100 && justgrow == false)
            {

                transform.localScale -= Vector3.one * intensity/60;
            }

            if (transform.localScale.x<=0)
            {
                gameObject.SetActive(false);
            }
        }
	}
}
