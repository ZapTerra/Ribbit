using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpin : MonoBehaviour {


    RectTransform rt;
    float timer;
    Vector3 angles;
    Vector3 homescale;
	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
        homescale = rt.localScale;
        rt.localScale = Vector3.zero;
        timer = -10.9f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime*1.5f;
        if (timer>0)
        {
            float val = Mathf.Sin(Mathf.Min(timer,1));
            rt.localScale = homescale * val;
            rt.localEulerAngles += Vector3.forward * Time.deltaTime * 470 * (1.1f - val);
        }

	}
}
