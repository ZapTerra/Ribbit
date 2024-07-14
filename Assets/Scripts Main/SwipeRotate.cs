using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotate : MonoBehaviour
{
    public float rotationSpeed = .5f;
    public float rotationDamp = .1f;
    public float momentumDecay = .025f;
    private Vector2 tapPosition;
    private Vector2 movement;
    private bool fingerDown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            tapPosition = Input.mousePosition;
            fingerDown = true;
        }
        //To be doubly sure in case of a wacky input source
        if(Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0)){
            fingerDown = false;
        }
        if(fingerDown){
            movement += new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height) - new Vector2(tapPosition.x / Screen.width, tapPosition.y / Screen.height);
            tapPosition = Input.mousePosition;
            movement *= 1 - rotationDamp;
        }else{
            movement *= 1 -momentumDecay;
        }
        //I combined two lines. It's very long now and I don't know what best practice is anymore.
        //I am aware the lerp will never stop/reach zero.
        transform.rotation = 
            Quaternion.Lerp(
                transform.rotation,
                Quaternion.AngleAxis(
                    movement.magnitude * rotationSpeed,
                    new Vector3(movement.y, -movement.x)
                ) * transform.rotation,
                Time.deltaTime * 6
            );
        //transform.rotation= Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(movement.y * rotationSpeed, Vector3.right) * transform.rotation, Time.deltaTime * 6);
    }
}
