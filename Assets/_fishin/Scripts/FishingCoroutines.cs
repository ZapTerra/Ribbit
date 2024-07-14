using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingCoroutines : MonoBehaviour
{
    public static FishingCoroutines instance;
    void Start()
    {
        instance = this;
    }

    public IEnumerator ClearTextOnDelay(TMPro.TextMeshProUGUI text, float delay){
        yield return new WaitForSeconds(delay);
        text.text = "";
    }
}
