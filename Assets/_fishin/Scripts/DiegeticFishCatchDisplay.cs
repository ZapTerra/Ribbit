using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiegeticFishCatchDisplay : MonoBehaviour {
	public string fisheName;
	public int rarity;
	[SerializeField]
	public Color mythic;
	public Color legendary;
	public Color epic;
	public Color rare;
	public Color uncommon;
	public Color common;

	public float timeToFadeout;
	public float timeLeftActive;
	public float fadeoutTime;
	public GameObject twinkly;
	public CanvasGroup canvasGroup;
	public UnityEngine.UI.Image fisheSprite;
	public TMPro.TextMeshProUGUI fishInfo;
	public GameObject mythicBG;
	public GameObject legendaryBG;
	public GameObject epicBG;
	public GameObject rareBG;
	private bool twinkleSpawned;
	private bool startFade;
	private ParticleSystemRenderer[] currentBackgroundMaterials;
	private List<float> alphaList;
	private Color textColorForRarity;
	void Start() {
		timeLeftActive = timeToFadeout;
		if (Resources.Load<Sprite>(fisheName.ToString()) != null) {
			fisheSprite.sprite = Resources.Load<Sprite>(fisheName.ToString());
		} else {
			fisheSprite = null;
		}
		if (rarity == 6) {
			EnableThings(mythic, mythicBG);
		}
		if (rarity == 5) {
			EnableThings(legendary, legendaryBG);
		}
		if (rarity == 4) {
			EnableThings(epic, epicBG);
		}
		if (rarity == 3) {
			EnableThings(rare, rareBG);
		}
		if (rarity == 2) {
			EnableThings(uncommon, null);
		}
		if (rarity == 1) {
			EnableThings(common, null);
		}
	}

	void Update() {
		timeLeftActive -= Time.deltaTime / timeToFadeout;
		if (timeLeftActive <= 0 && Input.GetMouseButtonDown(0)) {
			startFade = true;
		}
		if (timeLeftActive <= 0 && startFade) {
			if (!twinkleSpawned) {
				var spawned = Instantiate(twinkly);
				spawned.transform.position = transform.position;
				spawned.GetComponent<SpriteRenderer>().color = textColorForRarity;
				twinkleSpawned = true;
			}
			canvasGroup.alpha -= Time.deltaTime / fadeoutTime;
			if (currentBackgroundMaterials != null) {
				foreach (ParticleSystemRenderer renderer in currentBackgroundMaterials) {
					var tempColor = renderer.material.GetColor("_TintColor");
					tempColor.a -= Time.deltaTime / fadeoutTime * .5f;
					renderer.material.SetColor("_TintColor", tempColor);
				}
			}
			if (canvasGroup.alpha == 0) {
				Destroy(gameObject);
			}
		}
	}

	public void EnableThings(Color textColor, GameObject background) {
		if (textColor != null) {
			fishInfo.color = textColor;
			textColorForRarity = textColor;
		}
		if (background != null) {
			background.SetActive(true);
			currentBackgroundMaterials = GetComponentsInChildren<ParticleSystemRenderer>();
		}
	}
}
