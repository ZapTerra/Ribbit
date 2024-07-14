using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFrenzyMeter : MonoBehaviour
{
    public GameObject meterHolder;
    public List<SpriteRenderer> meterSprites;
    // Start is called before the first frame update
    void Start()
    {
        foreach(SpriteRenderer sr in meterSprites){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        }
        //meterHolder.transform.localScale = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        float meterFill = fisheEatBobe.feedingFrenzy / 4800;
        meterHolder.transform.localScale = new Vector3(1, meterFill, 1);
        foreach(SpriteRenderer sr in meterSprites){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, Mathf.Clamp(meterFill * 4, 0, 1), Time.deltaTime));
        }
    }
}
