using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour {

    [SerializeField]
    GameObject slider;
    [SerializeField]
    GameObject audfriend;
    AudioController ac;
    Slider sliderslider;

    private void Start()
    {
        sliderslider = slider.GetComponent<Slider>();
        ac = audfriend.GetComponent<AudioController>();
    }

    public void ChangeVol()
    {
        if (sliderslider == null)
        {
            sliderslider = slider.GetComponent<Slider>();
        }
        if (ac == null)
        {
            ac = audfriend.GetComponent<AudioController>();
        }
        float newVol = AudioListener.volume;
        newVol = sliderslider.value;
        AudioListener.volume = newVol;
        ac.SetGlobalVol(newVol);
    }
}

