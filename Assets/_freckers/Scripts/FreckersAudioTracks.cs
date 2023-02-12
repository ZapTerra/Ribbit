using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreckersAudioTracks : MonoBehaviour
{
    public AudioSource allaTurca;
    public List<AudioSource> riffs;
    private AudioSource currentRiff;

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
            currentRiff = riffs[Random.Range(0, riffs.Count)];
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
