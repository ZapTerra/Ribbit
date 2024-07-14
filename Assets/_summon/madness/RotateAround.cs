using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour {

    public Vector3 rota;
    public bool rando;
    public bool inout;
    Vector3 hom;
    bool reverse;
    float timer;
    float offset;
    float reversetimer;
	// Use this for initialization
	void Start () {
		if (rando == true)
        {
            transform.rotation = Random.rotation;
            offset = transform.parent.transform.parent.transform.localEulerAngles.y / 360 * Mathf.PI * 2;

        }
        hom = transform.localPosition;
	}
	

    public void Reverse()
    {
        reverse = true;
    }
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += rota*Time.deltaTime*Mathf.Max(-10,(1-reversetimer)*Mathf.Abs(Mathf.Pow(1-reversetimer,3)));
        if (inout==true)
        {
            timer += Time.deltaTime;
            transform.localPosition = hom + Vector3.back * (1+Mathf.Sin(timer))/4;
        }
        if (rando==true)
        {
            timer += Time.deltaTime;
            transform.localPosition = hom + Vector3.up * (1 + Mathf.Sin(timer+offset)) / 4;

        }
        if (reverse)
        {
            reversetimer += Time.deltaTime * 0.3f;
            timer -= Time.deltaTime*reversetimer;
        }
	}
}
