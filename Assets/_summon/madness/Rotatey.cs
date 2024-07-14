using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotatey : MonoBehaviour, IPointerClickHandler
{

    [SerializeField]
    bool ccw;
    Vector3 loc;
    Vector3 locpos;
    float timer;
    float fval;
    Vector3 locscal;
	// Use this for initialization
	void Start () {
        
        loc = transform.localEulerAngles;
        locpos = transform.localPosition;
        locscal = transform.localScale;
        if (ccw)
        {
            fval = -1;
            timer = Mathf.PI / 4;
        } else
        {
            fval = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime*fval;
        transform.localEulerAngles = loc + Mathf.Sin(-timer) * Vector3.forward*30;
        transform.localPosition = locpos + Mathf.Sin(-timer) * Vector3.up/4;
        if (transform.localScale!=locscal)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, locscal, Time.deltaTime*3);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.localScale = transform.localScale * 1.1f;
    }
}
