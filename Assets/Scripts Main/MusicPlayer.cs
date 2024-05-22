using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour {

	public static MusicPlayer Instance { get; private set; }
	public List<GameObject> dontDestroyTheseOnLoad;
	private string sceneMusicIsFor;

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene().name != sceneMusicIsFor) {
			foreach(GameObject g in dontDestroyTheseOnLoad){
				g.transform.parent = null;
			}
			Instance = null;
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	void Start() {
		if (Instance != null) {
			Destroy(gameObject);
		} else {
			Instance = this;
		}
	}
	void Awake() {
		sceneMusicIsFor = SceneManager.GetActiveScene().name;
	}
}
