using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCount : MonoBehaviour {
	public GameObject froge;
	public int scoreNumber;
	public TMPro.TextMeshProUGUI score;
	public TMPro.TextMeshProUGUI hiScore;
	private bool playerRewarded;
	// Start is called before the first frame update
	void Start() {
		hiScore.text = (PlayerPrefs.GetInt("hiScore")).ToString();
		PlayerPrefs.Save();
	}

	// Update is called once per frame
	void Update() {
		if (FrogeMove.keepingCount) {
			scoreNumber = Mathf.FloorToInt(froge.transform.position.y / 3);

			if(scoreNumber % 5 == 0 && scoreNumber != 0 && !playerRewarded){
				PlayerMoney.MONEY++;
				PlayerMoney.saveMoney();
				playerRewarded = true;
			}else if(scoreNumber % 5 != 0 && playerRewarded){
				playerRewarded = false;
			}

			score.text = scoreNumber.ToString();
			if (scoreNumber > PlayerPrefs.GetInt("hiScore")) {
				PlayerPrefs.SetInt("hiScore", scoreNumber);
				PlayerPrefs.Save();
			}
		}
	}
}
