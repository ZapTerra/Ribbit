using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCount : MonoBehaviour {

    Text txt;
    [SerializeField]
    GameObject textobjct;
    InputField txt2;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
        txt2 = textobjct.GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateCountFunc()
    {
        if (txt == null)
        {
            txt = GetComponent<Text>();

            txt2 = textobjct.GetComponent<InputField>();
        }


        txt.text = txt2.text.Length + "/240";
    }
}
