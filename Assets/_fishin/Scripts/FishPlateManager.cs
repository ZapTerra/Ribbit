using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishPlateManager : MonoBehaviour {
	public string fishType;
	public bool haveCaught;
	public Image fishImage;
	public TextMeshProUGUI display;
	public TextMeshProUGUI caught;
	public bool hasSubspecies;
	private bool youCanKillMeNow;
	void Start() {
		fishImage.sprite = Resources.Load<Sprite>(fishType.ToString());
		if(hasSubspecies){
			fishType += "(s)";
		}
		display.text = fishType;
		if(!haveCaught){
			fishImage.color = new Color(0, 0, 0, 1);
			display.color = new Color(0, .5f, 0, 1);
			display.text = "?????";
			caught.color = Color.white;
		}
	}
	void Update() {
		//lol
		youCanKillMeNow = true;
	}
	void OnDisable(){
		if(youCanKillMeNow){
		Destroy(gameObject.transform.parent.gameObject);
		}
	}

}
