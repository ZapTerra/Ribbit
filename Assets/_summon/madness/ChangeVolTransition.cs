using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVolTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ChangeVol(float vol)
    {
        float newVol = AudioListener.volume;
        newVol = vol;
        AudioListener.volume = newVol;

    }
}
