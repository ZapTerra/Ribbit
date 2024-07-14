using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

	public AudioSource bloop;
	[SerializeField]
	Material mat;
	[SerializeField]
	GameObject homeButton;
	public GameObject currencyDisplay;
	public GameObject costDisplay;
	bool clicked;
	float timer;
	ParticleSystem ps;
	bool played;
	float timer2;
	float timer3;
	Vector3 homescale;
	[SerializeField]
	GameObject growsphere;
	[SerializeField]
	GameObject ringsrings;
	[SerializeField]
	GameObject rotayrotay;
	[SerializeField]
	Material thismat;
	[SerializeField]
	GameObject rotatefriend;
	[SerializeField]
	GameObject musicfriend;
	RotateAround[] rotarounds;
	public TMPro.TextMeshProUGUI summonCostText;
	private int summonCost;
	// Use this for initialization
	void Start() {
		mat.SetFloat("_Iterate", 0);
		ps = GetComponent<ParticleSystem>();
		played = false;
		thismat.SetFloat("_Translate", 0);
		homescale = transform.localScale;

		Debug.Log("Managing Summon Cost Here");
		//If no existing value for summon time, create value as yesterday
		if(!PlayerPrefs.HasKey("Summon Date")){
			Debug.Log("x");
			PlayerPrefs.SetFloat("SummonCostMultiplier", 1);
			PlayerPrefs.SetString("Summon Date", System.DateTime.Now.AddDays(-1).ToBinary().ToString());
		}
		long temp = System.Convert.ToInt64(PlayerPrefs.GetString("Summon Date"));
		System.DateTime oldDate = System.DateTime.FromBinary(temp);
		if(System.DateTime.Now.Subtract(oldDate).Hours > 14){
			Debug.Log(1);
			PlayerPrefs.SetString("Summon Date", System.DateTime.Now.ToBinary().ToString());
			PlayerPrefs.SetFloat("SummonCostMultiplier", 1);
			summonCost = 10;
		}else{
			Debug.Log("regular");
			Debug.Log(PlayerPrefs.GetFloat("SummonCostMultiplier"));
			summonCost = (int)(10 * PlayerPrefs.GetFloat("SummonCostMultiplier"));
		}
		if(!PlayerPrefs.HasKey("HasSummonedBefore")){
			Debug.Log(3);
			summonCost = 0;
		}
		summonCostText.text = summonCost.ToString();
	}

	private void LateUpdate() {
		if (timer2 > 130) {
			Camera.main.transform.position += Vector3.forward * Time.deltaTime / 4 * Mathf.Clamp01((timer2 - 130) / 30);
			rotatefriend.transform.Rotate(Vector3.left, Time.deltaTime * 14 * Mathf.Clamp01((timer2 - 130) / 5));
		}
	}

	// Update is called once per frame
	void Update() {
		if (timer2 > 130) {
			transform.position += Vector3.back * Time.deltaTime * Mathf.Clamp01((timer2 - 130) / 30);

		}

		if (clicked == true) {
			timer += Time.deltaTime * 34;
			timer2 += Time.deltaTime * 34;

		}
		mat.SetFloat("_Iterate", timer);
		if (timer2 > 18 && played == false) {
			homeButton.SetActive(false);
			currencyDisplay.SetActive(false);
			costDisplay.SetActive(false);
			played = true;
			Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ScreenOverlay>().SetGo(true);
			ringsrings.SetActive(true);
			rotarounds = rotayrotay.GetComponentsInChildren<RotateAround>();
			foreach (RotateAround ra in rotarounds)
				ra.Reverse();

		}
		if (timer > 45) {
			GetComponent<MeshRenderer>().enabled = false;
		}
		if (timer2 > 60) {
			thismat.SetFloat("_Translate", Mathf.Pow((60 - timer2) / 30, 2));
		}
		if (timer3 >= 0) {
			timer3 += Time.deltaTime;
			if (timer3 > 2f) {
				growsphere.transform.localScale *= 1.05f;
				timer3 = -1;
			}
		}
	}

	void OnMouseDown() {
		if(PlayerMoney.MONEY >= summonCost){
			PlayerPrefs.SetInt("HasSummonedBefore", 1);
			if(PlayerPrefs.GetFloat("SummonCostMultiplier") < 4){
				PlayerPrefs.SetFloat("SummonCostMultiplier", PlayerPrefs.GetFloat("SummonCostMultiplier") * 2);
			}
			PlayerMoney.MONEY -= summonCost;
			PlayerMoney.saveMoney();
			TritterGacha.TritterPull(1);

			clicked = true;
			transform.localScale = homescale;
			GetComponent<MeshCollider>().enabled = false;
			timer = 0;
			if (timer2 > 12) {
				timer2 = 0;
			}
			played = false;

			bloop.Play();
			musicfriend.GetComponent<AudioController>().StartAudio();
		}
	}

	private void OnMouseEnter() {
		transform.localScale = homescale * 1.1f;
	}

	private void OnMouseExit() {
		transform.localScale = homescale;
	}
}
