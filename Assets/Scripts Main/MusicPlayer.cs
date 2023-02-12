using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

	public static MusicPlayer Instance { get; private set; }
	private string sceneMusicIsFor;

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene().name != sceneMusicIsFor) {
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
