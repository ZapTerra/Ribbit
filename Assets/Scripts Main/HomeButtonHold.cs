using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButtonHold : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float holdTime;
    public Color pressColor;
    public UnityEvent buttonEvent;
    public UnityEngine.UI.Image homeButtonImage;
    public Button buttonReference;
    private bool holding;
    private float holdStartTime;
    private bool cursorIsOverButton;
    // Start is called before the first frame update
    void Start()
    {
        homeButtonImage.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(cursorIsOverButton && !holding){
            BeginHold();
        }
    }

    public void BeginHold()
    {
        if(buttonReference.interactable){
            holding = true;
            holdStartTime = Time.time;
            StartCoroutine(HoldButton());
        }
    }

    public IEnumerator HoldButton(){
        float percent = 0;
        while(true){
            percent = (Time.time - holdStartTime) / holdTime;
            homeButtonImage.fillAmount = percent;
            if(percent >= 1 || !cursorIsOverButton || Input.GetMouseButtonUp(0)){
                break;
                holding = false;
            }
            yield return null;
        }
        Debug.Log("Attempting hold button trigger, checking if finger was held down for duration: " + (percent >= 1));
        if(percent >= 1){
            homeButtonImage.color = pressColor;
            Debug.Log("Triggering hold press button event");
            buttonEvent.Invoke();
        }else{
            homeButtonImage.fillAmount = 0;
        }
        holding = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        cursorIsOverButton = true;
        Debug.Log("entered");
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        cursorIsOverButton = false;
    }
}
