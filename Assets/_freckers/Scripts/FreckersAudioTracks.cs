using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreckersAudioTracks : MonoBehaviour
{
    public AudioSource allaTurca;
    public List<AudioSource> riffs;
    private AudioSource currentRiff;
    private int riffIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwapRiff()
    {
        if(!allaTurca.mute)
        {
            allaTurca.mute = true;
            currentRiff = riffs[riffIndex];
            riffIndex++;
            if(riffIndex > riffs.Count - 1){
                riffIndex = 0;
            }
            currentRiff.enabled = true;
        }
    }

    public void SwapClassical(){
        if(allaTurca.mute)
        {
            allaTurca.mute = false;
            if(currentRiff != null){
                currentRiff.enabled = false;
            }
        }
    }
}
