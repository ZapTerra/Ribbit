using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOver : MonoBehaviour {

    bool mousetime;
    [SerializeField]
    GameObject uihover;

    void OnMouseDown()
    {
        mousetime = true;
        uihover.GetComponent<UIManager>().MoveUp();
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }


}

