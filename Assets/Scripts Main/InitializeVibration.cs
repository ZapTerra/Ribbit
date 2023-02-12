using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeVibration : MonoBehaviour
{
    void Start()
    {
        Vibration.Init();
        PlayerPrefs.SetInt("OGDOWNLOAD", 1);
    }
}
