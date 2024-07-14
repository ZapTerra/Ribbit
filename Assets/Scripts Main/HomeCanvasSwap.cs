using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCanvasSwap : MonoBehaviour
{
    public GameObject homeWorld;
    public GameObject homeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swapCanvasAndHomeWorld(){
        homeCanvas.SetActive(!homeCanvas.activeInHierarchy);
        homeWorld.SetActive(!homeCanvas.activeInHierarchy);
    }
}
