using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuButton : MonoBehaviour
{
    public string scene;
    public LoadScenes sceneLoader;
    private Vector2 clickPosition;
    private bool haveClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && haveClicked && ((Vector2)Input.mousePosition - clickPosition).magnitude < (Screen.width + Screen.height) / 20)
        {
            sceneLoader.DoSceneTransition(scene);
        }
    }

    //I don't want to figure out how to apply an editor GUI drop in event here T_T
    void OnMouseDown(){
        clickPosition = Input.mousePosition;
        haveClicked = true;
    }
}
