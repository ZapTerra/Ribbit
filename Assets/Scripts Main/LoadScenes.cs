using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class LoadScenes : MonoBehaviour {
	public GameObject transitionLiliePade;
	public GameObject canvas;
	public float duration = 0.1f;
	public int liliesPerFrame;
	private float transitionTime = 1f;
	public static bool sceneIsTransitioning;

	//create set of random coordinates to populate the screen with
	void Start() {
		sceneIsTransitioning = false;
		DOTween.SetTweensCapacity(1000, 125);
	}

	public void DoSceneTransition(string scene) {
		Debug.Log("Loading scene : \"" + scene + "\"");
		if(!sceneIsTransitioning){
			sceneIsTransitioning = true;
			StartCoroutine(AnimateTransition(scene));
			DontDestroyOnLoad(gameObject);
		}else{
			Debug.Log("Already loading a scene.");
		}
	}

	public void DoResetLogHoppingAds() {
		FrogeMove.adCounter = 0;
		FrogeMove.timeOfLastAd = Time.time;
	}

	//populate screen with lilypads and load scene
	IEnumerator AnimateTransition(string scene) {
		liliesPerFrame = (int)((Screen.width * Screen.height / 2500) / ((1 - duration * 2) / Time.deltaTime) / transitionTime * 2);
		liliesPerFrame = liliesPerFrame == 0 ? 1 : liliesPerFrame;
		if(liliesPerFrame < 30){
			liliesPerFrame = 30;
		}
		List<Vector2> padCoords = new List<Vector2>();
		const int margin = 50;
		int xProgression = -margin;
		int yProgression = -margin;
		while (yProgression < Screen.height + margin) {
			xProgression = 0;
			while (xProgression < Screen.width + margin) {
				padCoords.Insert(Random.Range(0, padCoords.Count), new Vector2(xProgression + Random.Range(-25, 26), yProgression + Random.Range(-25, 26)));
				xProgression += 50;
			}

			yProgression += 50;
		}

		List<Tween> tweens = new List<Tween>();
		for (int i = 0; i < padCoords.Count; i++) {
			if (i % liliesPerFrame == 0) {
				yield return 0;
			}

			var padCoord = padCoords[i];
			var currentPade = Instantiate(transitionLiliePade, (Vector3)padCoord, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
			currentPade.transform.SetParent(canvas.transform, false);
			tweens.Add(currentPade.transform.DOScale(Random.Range(0.4f, .6f), duration).From(0));
		}
		yield return new WaitForSeconds(duration);
		var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
		while (!async.isDone || tweens.Any((tween) => tween.active)) {
			yield return new WaitForEndOfFrame();
		}
		sceneIsTransitioning = false;
		Debug.Log("Hi! I'm killing ALL Tweens here!");
		StartCoroutine(Younglings(tweens));
	}

	IEnumerator Younglings(List<Tween> tweens) {
		liliesPerFrame = (int)((Screen.width * Screen.height / 2500) / ((1 - duration * 2) / Time.deltaTime) / transitionTime * 2);
		liliesPerFrame = liliesPerFrame == 0 ? 1 : liliesPerFrame;
		if(liliesPerFrame < 30){
			liliesPerFrame = 30;
		}
		var younglings = new List<GameObject>();
		foreach (Transform t in transform) {
			younglings.Insert(Random.Range(0, younglings.Count), t.gameObject);
		}

		for (int i = 0; i < younglings.Count; i++) {
			if (i % liliesPerFrame == 0) {
				yield return new WaitForEndOfFrame();
			}
			var youngling = younglings[i];
			youngling.transform.DOScale(0f, duration).OnKill(() => Destroy(youngling));
		}
		yield return new WaitForSeconds(duration);
		DOTween.KillAll();
		Destroy(gameObject);
	}
}

//attempt at consistency
//breaks in build

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using DG.Tweening;
// using System.Linq;

// public class LoadScenes : MonoBehaviour {
// 	public GameObject transitionLiliePade;
// 	public GameObject canvas;
// 	public float duration = 0.1f;
// 	public int liliesPerFrame;
// 	private float transitionTime = 1.6f;
// 	public static bool sceneIsTransitioning;
// 	private float odds;

// 	//create set of random coordinates to populate the screen with
// 	void Start() {
// 		sceneIsTransitioning = false;
// 		DOTween.SetTweensCapacity(1000, 125);
// 	}

// 	public void DoSceneTransition(string scene) {
// 		Debug.Log("Loading scene : \"" + scene + "\"");
// 		if(!sceneIsTransitioning){
// 			sceneIsTransitioning = true;
// 			StartCoroutine(AnimateTransition(scene));
// 			DontDestroyOnLoad(gameObject);
// 		}else{
// 			Debug.Log("Already loading a scene.");
// 		}
// 	}

// 	public void DoResetLogHoppingAds() {
// 		FrogeMove.adCounter = 0;
// 		FrogeMove.timeOfLastAd = Time.time;
// 	}

// 	//populate screen with lilypads and load scene
// 	IEnumerator AnimateTransition(string scene) {
// 		calculatePerFrame();
// 		List<Vector2> padCoords = new List<Vector2>();
// 		const int margin = 50;
// 		int xProgression = -margin;
// 		int yProgression = -margin;
// 		while (yProgression < Screen.height + margin) {
// 			xProgression = 0;
// 			while (xProgression < Screen.width + margin) {
// 				padCoords.Insert(Random.Range(0, padCoords.Count), new Vector2(xProgression + Random.Range(-25, 26), yProgression + Random.Range(-25, 26)));
// 				xProgression += 50;
// 			}

// 			yProgression += 50;
// 		}

// 		List<Tween> tweens = new List<Tween>();
// 		for (int i = 0; i < padCoords.Count; i++) {
// 			if (i % liliesPerFrame == 0) {
// 				yield return 0;
// 			}
// 			var padCoord = padCoords[i];
// 			var currentPade = Instantiate(transitionLiliePade, (Vector3)padCoord, Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360))));
// 			currentPade.transform.SetParent(canvas.transform, false);
// 			tweens.Add(currentPade.transform.DOScale(Random.Range(0.4f, .6f), duration).From(0));
// 		}
// 		yield return new WaitForSeconds(duration);
// 		var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
// 		while (!async.isDone || tweens.Any((tween) => tween.active)) {
// 			yield return new WaitForEndOfFrame();
// 		}
// 		sceneIsTransitioning = false;
// 		Debug.Log("Hi! I'm killing ALL Tweens here!");
// 		StartCoroutine(Younglings(tweens));
// 	}

// 	IEnumerator Younglings(List<Tween> tweens) {
// 		// Randomly shuffled list of children.
// 		var younglings = new List<GameObject>();
// 		foreach (Transform t in transform) {
// 			younglings.Insert(Random.Range(0, younglings.Count), t.gameObject);
// 		}
// 		for (int i = 0; i < younglings.Count; i++) {
// 			if(odds < 1 && Random.RandomRange(0f, 1f) > odds){
// 				yield return new WaitForEndOfFrame();
// 			}
// 			if (i % liliesPerFrame == 0) {
// 				yield return new WaitForEndOfFrame();
// 			}
// 			calculatePerFrame();
// 			var youngling = younglings[i];
// 			youngling.transform.DOScale(0f, duration).OnKill(() => Destroy(youngling));
// 		}
// 		yield return new WaitForSeconds(duration);
// 		DOTween.KillAll();
// 		Destroy(gameObject);
// 	}

// 	private void calculatePerFrame(){
// 		odds = (Screen.width * Screen.height / 2500) / (((1 - duration * 2) / Time.deltaTime) / transitionTime * 2);
// 		liliesPerFrame = (int)odds;
// 		liliesPerFrame = liliesPerFrame == 0 ? 1 : liliesPerFrame;
// 	}
// }
