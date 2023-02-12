using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDisplayMusicPlayer : MonoBehaviour
{
    public List<AudioSource> tracks;
    private bool starting;
    // Start is called before the first frame update
    void Start()
    {
        if(TritterGacha.mostRecentPull[0].rarity == 1){
            tracks[0].Play();
        }
        if(TritterGacha.mostRecentPull[0].rarity == 2){
            tracks[1].Play();
        }
        if (TritterGacha.mostRecentPull[0].rarity == 3){
            tracks[2].Play();
        }
        if(TritterGacha.mostRecentPull[0].rarity == 4){
            tracks[3].Play();
        }
        if(TritterGacha.mostRecentPull[0].rarity == 5){
            tracks[4].Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
