using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioController : MonoBehaviour {

	public List<AudioSource> auds;
	float timer;
	bool starting;
	bool sound1;
	bool sound2;
	bool sound3;
	bool sound4;
	bool sound5;
	bool destroynext;
	float globalvol;
	bool hasbeenset;
	// Use this for initialization
	void Start() {
		DontDestroyOnLoad(this.gameObject);
		SceneManager.activeSceneChanged += ChangedActiveScene;
	}

	private void ChangedActiveScene(Scene current, Scene next) {
		if(this){
			name = next.name;
			//name != "SummonScene" && 
			if (name != "BeetleSummoned") {
				SceneManager.activeSceneChanged -= ChangedActiveScene;
				Destroy(gameObject);
			}
		}
	}

	public void SetGlobalVol(float f) {
		//Old method from other project I don't think I want it messing with the audio
		//globalvol = f;
		hasbeenset = true;
	}
	// Update is called once per frame
	void Update() {
		//no clue
		// if (destroynext == true && SceneManager.GetActiveScene().name == "SummonedScene") {
		// 	destroynext = true;
		// }
		if(starting){
			if(TritterGacha.mostRecentPull[0].rarity == 1){
				auds[0].PlayDelayed(1);
			}
			if(TritterGacha.mostRecentPull[0].rarity == 2){
				auds[1].PlayDelayed(1);
			}
			if (TritterGacha.mostRecentPull[0].rarity == 3){
				auds[2].PlayDelayed(1);
			}
			if(TritterGacha.mostRecentPull[0].rarity == 4){
				auds[3].PlayDelayed(1);
			}
			if(TritterGacha.mostRecentPull[0].rarity == 5){
				auds[4].PlayDelayed(1);
			}
			starting = false;
		}
	}

	public void StartAudio() {
		timer = 0;
		starting = true;
		Destroy(FindObjectOfType<MusicPlayer>().gameObject);
	}
}
