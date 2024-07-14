using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontChange : MonoBehaviour {

    Text txt;
    [SerializeField]
    bool randomize;
    [SerializeField]
    string[] strings;
    float timer;
    bool done;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
		if (randomize)
        {
            txt.text = strings[Random.Range(0, strings.Length)];
        } else
        {
            txt.text = strings[0];
        }
	}

    private void OnEnable()
    {
        txt = GetComponent<Text>();
        if (randomize)
        {
            txt.text = strings[Random.Range(0, strings.Length)];
        }
        else
        {
            //txt.text = strings[0];
            //timer = 0;
            //done = false;
        }
    }

    // Update is called once per frame
    void Update () {
		if (!randomize && !done)
        {
            timer += Time.deltaTime;
            if (timer>0.9f && timer<1.5f)
            {
                txt.text = strings[1];
            }
            if (timer>2.6f)
            {

                txt.text = strings[2];
                done = true;
                //randomize = true;
            }
        }
            
    }
}
