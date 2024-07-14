using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class fisheEatBobe : MonoBehaviour {
	public int adsFishRatio = 7;
	public float adDelay = 2f;
	public GameObject diegeticDisplay;
	public GameObject timeMeter;
	public GameObject timeBar;
	public Image symbolToDrawDisplay;
	public static TMPro.TextMeshProUGUI infoDisplay;
	public float timeToCatch = 2;
	public float timeLeft = 2;
	public float totalTime;
	public List<string> fisheSymbols;
	public string rarityName;
	public string fisheName;
	public float frenzyBonus = 400;
	public float frenzyDecay = 8;
	public float frenzyTimeDecay = .0003f;
	public float fisheLength = 10f;
	[SerializeField]
	public static float feedingFrenzy;
	public static int adCounter;
	public static int rarityOfLastCaughtFish;

	private TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
	private int rarity;
	private int rollRange = 4096;
	private bool amBite = false;
	private bool catchSuccess;
	private bool exitDrawMode = false;

	public static List<int> fisheCount = new List<int>
	{
		0,
		0,
		0,
		0,
		0,
		0
	};

	//this is to make the empty list for everything the player has caught :)
	public static List<List<Fishe>> caugghtFisheList = new List<List<Fishe>>
	{
		new List<Fishe>{

		},
		new List<Fishe>
		{

		},
		new List<Fishe>
		{

		},
		new List<Fishe>
		{

		},
		new List<Fishe>
		{

		},
		new List<Fishe>
		{

		}
	};
	public static List<List<string>> symbolPool = new List<List<string>>
	{

		//I am aware that this code does not work as intended. However, I have also become endeared to the way it dishes out symbols to draw <3
		new List<string>{
			"exclamation mark", "line", "caret", "v", "crescent", "loop"
		},
		new List<string>
		{
			"triangle"
		},
		new List<string>
		{
			"heart", "s", "z"
		},
		new List<string>
		{
			"pentagram"
		},
		new List<string>
		{
			"swoopy"
		},
		new List<string>
		{
			"z"
		},
		// new List<string>{
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
		// new List<string>
		// {
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
		// new List<string>
		// {
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
		// new List<string>
		// {
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
		// new List<string>
		// {
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
		// new List<string>
		// {
		// 	"exclamation mark", "line", "caret", "v", "crescent", "loop","triangle","heart", "s", "z", "swoopy"
		// },
	};

	//all fish species
	public static List<List<string>> fishePool = new List<List<string>>
	{
		new List<string>{
			"Palefish", "Silverside","Three Spot Gourami","Loweye Catfish","Yellow-Edged Moray","Armorhead Catfish","Slimehead","Salamanderfish","Dogfish Shark","Dwarf Loach","Cutlassfish","Footballfish","Coffinfish",
		},
		new List<string>
		{
			"Yelloweye Hooktail","Greateye Yellowgill","Backgill","Ruffled Nosefish","Angelfish","Fusilier fish","Kingfish","Bigscale Pomfret","Red Snapper","Green Swordtail","Warmouth","Thornheaded Shark","Crocodile Icefish","Rabbitfish","Sawtail Crocmaw","Matryoshka Trout","Goldside","Whalelip Blubberfin","Cranemaw Angler","Fleetfish"
		},
		new List<string>
		{
			"Cheffish","Rainfish","Falseface","Bonnetmouth","Knifefish","Tilefish","Pelican Gulper","Handfish","Cichlid","Snailfish","Slender Snipe Eel","Spadebeak","Flaming Crestfin"
		},
		new List<string>
		{
			"Snowtail","Phillyfish","Arowana","European Perch","Butterfish","Leatherjacket","Megamouth Shark"
		},
		new List<string>
		{
			"Murray Cod","Candlefish","Elephant Fish","Bonefish"
		},
		new List<string>
		{
			"Madagascar Pochard","Sea Dragon","Dunkleosteus"
		}
	};

	//how big each species of fish is

	//!!!!!!!
	//may need revision, heavier weighting towards larger fish in progression to higher tiers
	//!!!!!!!
	public static List<List<float>> fisheSizes = new List<List<float>>
	{
		new List<float>{
			1, 2, 1, 2, 2, 2, 3, 1, 2, 1, 1, 1, 1
		},
		new List<float>
		{
			2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 2, 1, 1, 2, 2, 3, 2
		},
		new List<float>
		{
			3, 2, 2, 4, 4, 4, 4, 2, 1, 2, 2, 3, 3
		},
		new List<float>
		{
			4, 3, 4, 2, 3, 3, 4
		},
		new List<float>
		{
			2, 1, 4, 4
		},
		new List<float>
		{
			5, 5, 5
		}
	};

	// Start is called before the first frame update
	void Start() {


		//Get symbol draw prompt
		symbolToDrawDisplay = GameObject.Find("symbolToDraw").GetComponent<Image>();
		timeMeter = GameObject.Find("Fish Catch Time Meter");
		timeBar = GameObject.Find("Fish Catch Time Bar");
		//set
		setDrawImage();

		//get time left display
		infoDisplay = GameObject.Find("InfoDisplay").GetComponent<TMPro.TextMeshProUGUI>();

		//Imagine a range of numbers, with subsets in that range to pick from. Each subset is equivalent to a rarity of fish. What this code does
		//is take the player's current Frenzy bonus, cut that many numbers out of the bottom of the range, and then reassign all the remaining numbers
		//in the range with a new rarity while maintaining proportions.
		var RNG = Random.Range(1, rollRange);
		RNG = Mathf.FloorToInt(((RNG * -feedingFrenzy) + (rollRange * RNG) + (rollRange * feedingFrenzy)) / rollRange);
		rarity = RNG < 2756 ? 1 : RNG < 3674 ? 2 : RNG < 3980 ? 3 : RNG < 4082 ? 4 : RNG < 4096 ? 5 : 6;

		//Assign the name of the fish's rarity tier for display purposes.
		rarityName = rarity < 2 ? "Common" : rarity < 3 ? "Uncommon" : rarity < 4 ? "Rare" : rarity < 5 ? "Epic" : rarity < 6 ? "Legendary" : "Mythic";

		//Select a random species of fish from the pool of rarity selected
		RNG = Random.Range(0, fishePool[rarity - 1].Count);
		fisheName = fishePool[rarity - 1][RNG];

		//Select sub-species if applicable
		if (fisheName == "Palefish") {
			var fishSpecies = new List<string> { "Bulbnosed", "Heapmouth", "Slackjaw", "Spooljaw", "Stoutbrow", "Tallheaded" };
			fisheName = fishSpecies[Random.Range(0, fishSpecies.Count)] + " Palefish";
		}

		if (fisheName == "Rabbitfish") {
			var fishSpecies = new List<string> { "Blue", "White" };
			fisheName = fishSpecies[Random.Range(0, fishSpecies.Count)] + " Rabbitfish";
		}

		if (fisheName == "Arowana") {
			var fishSpecies = new List<string> { "Boomerang", "Hooklined" };
			fisheName = fishSpecies[Random.Range(0, fishSpecies.Count)] + " Arowana";
		}


		//Get the size of the fish species selected
		fisheLength = fisheSizes[rarity - 1][RNG];
		var sizeVariance = Random.Range(.5f, 2f);


		//take this out
		// for (int i = 0; i < rarity; i++) {
		// 	var symRarityPool = Random.Range(0, rarity - 1);

		// 	var lastSymbol = "Big O Tires";
		// 	if (fisheSymbols.Count > 0) {
		// 		lastSymbol = fisheSymbols[fisheSymbols.Count - 1];
		// 	}

		// 	var newSymbol = "";
		// 	while (newSymbol == "" || newSymbol == lastSymbol) {
		// 		newSymbol = symbolPool[symRarityPool][Random.Range(0, symbolPool[symRarityPool].Count - 1)];
		// 	}

		// 	fisheSymbols.Add(newSymbol);
		// }

		//replace with this


		//populate array with gestures required to catch fish
		for (int i = 0; i < rarity; i++) {
			var symRarityPool = Random.Range(0, rarity - 1);
			fisheSymbols.Add(symbolPool[symRarityPool][Random.Range(0, symbolPool[symRarityPool].Count - 1)]);
		}

		transform.parent.localScale *= 1 + fisheLength * .3f;
		transform.parent.gameObject.GetComponentInChildren<Collider2D>().transform.localScale /= 1 + fisheLength * .3f;
		transform.parent.gameObject.GetComponent<fisheMove>().speed -= fisheLength * .1f;
		transform.parent.gameObject.GetComponent<fisheMove>().rotationSpeed -= fisheLength * .1f;


		// I MADE A MISTAKE I'LL WRITE BETTER CODE IN THE FUTURE OKAY
		foreach (List<Fishe> rarityTier in caugghtFisheList) {
			foreach (Fishe fishe in rarityTier) {
				fishe.name = textInfo.ToTitleCase(fishe.name.ToLower());
			}
		}
	}

	// Update is called once per frame
	void Update() {
		StartCoroutine(MakeMeLotsOfMoney());
		if (feedingFrenzy > 0) {
			feedingFrenzy -= Time.deltaTime * frenzyDecay;
			if (feedingFrenzy < 0) {
				feedingFrenzy = 0;
			}
		}
		if (amBite) {
			timeLeft -= Time.deltaTime;
			setDrawImage();
			if (timeLeft <= 0) {
				Camera.main.GetComponent<RecognizerScript>().GetSymbol();
			} else {
				//infoDisplay.text = timeLeft.ToString();
				timeBar.transform.localScale = new Vector3(Mathf.Clamp(timeLeft/totalTime, .001f, 100f), 1.02f, 1.02f);

				if (Camera.main.GetComponent<RecognizerScript>().enabled && Camera.main.GetComponent<RecognizerScript>().type != null && (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))) {
					//Camera.main.GetComponent<RecognizerScript>().WriteGesture(fisheSymbols[0]);
					checkSymbol(false);
				}
			}
		}

	}

	private void LateUpdate() {
		if (timeLeft <= 0) {
			checkSymbol(true);
		}
		if (exitDrawMode) {
			fisheSymbols.Clear();
			setDrawImage();
			Camera.main.GetComponent<RecognizerScript>().enabled = false;
			Camera.main.GetComponent<RecognizerScript>().type = null;
			Camera.main.GetComponent<Collider2D>().enabled = false;
			Destroy(transform.parent.gameObject);
		}
	}

	private void checkSymbol(bool lastChance) {
		if (Camera.main.GetComponent<RecognizerScript>().type == fisheSymbols[0]) {
			if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Vibration.VibratePeek();
			}
			fisheSymbols.RemoveAt(0);
			FindObjectOfType<AudioManager>().Play("Symbol Hit");
			Camera.main.GetComponent<RecognizerScript>().type = null;
			CalculateTime();
			if (fisheSymbols.Count == 0) {
				symbolToDrawDisplay.color = new Color(255, 255, 255, 255);
				GameObject newDisplay = Instantiate(diegeticDisplay);
				newDisplay.transform.position = transform.position;
				newDisplay.GetComponent<DiegeticFishCatchDisplay>().rarity = rarity;
				newDisplay.GetComponent<DiegeticFishCatchDisplay>().fisheName = fisheName;
				//Yes, I know this is long. Too bad! ;)
				newDisplay.GetComponent<DiegeticFishCatchDisplay>().fishInfo.text = "Caught a" + (rarityName == "Epic" || rarityName == "Uncommon" ? "n" : "") + " " + rarityName + " " + textInfo.ToTitleCase(fisheName.ToLower()) + "!";
				//infoDisplay.text = "";
				PlayerMoney.MONEY += 1;
				PlayerMoney.saveMoney();
				rarityOfLastCaughtFish = rarity;
				adCounter++;
				fisheCount[rarity - 1]++;
				caugghtFisheList[rarity - 1].Add(new Fishe(fisheName, fisheLength));
				FindObjectOfType<fisheCount>().FishesCensus();
				feedingFrenzy += frenzyBonus;
				exitDrawMode = true;
			}
		} else {
			if (lastChance) {
				feedingFrenzy -= 2000;
				feedingFrenzy = Mathf.Clamp(feedingFrenzy, 0, Mathf.Infinity);
				infoDisplay.text = "Failed to Catch the Fishe.";
				//I do not like this line.
				FishingCoroutines.instance.StartCoroutine(FishingCoroutines.instance.ClearTextOnDelay(infoDisplay, 1f));
				rarityOfLastCaughtFish = 0;
				adCounter++;
				Debug.Log(rarityName + " " + fisheName);
				exitDrawMode = true;
			}
			Camera.main.GetComponent<RecognizerScript>().type = null;
		}
	}

	private  void CalculateTime(){
		timeLeft = timeToCatch - feedingFrenzy * frenzyTimeDecay;
		curbDifficulty();
		totalTime = timeLeft;
	}

	private IEnumerator MakeMeLotsOfMoney() {
		if (adCounter < 6 || rarityOfLastCaughtFish >= 3)
			yield break;
		Debug.Log("Set 0");
		adCounter = 0;
		yield return new WaitForSeconds(adDelay);
		//FindObjectOfType<FullscreenAd>().MakeMoneyEverywhere();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "bobe" && !Camera.main.GetComponent<RecognizerScript>().enabled) {
			Vibration.VibratePeek();
			Camera.main.GetComponent<RecognizerScript>().enabled = true;
			Camera.main.GetComponent<RecognizerScript>().type = null;
			Camera.main.GetComponent<Collider2D>().enabled = true;

			CalculateTime();

			amBite = true;
			timeMeter.GetComponent<Image>().enabled = true;
			timeBar.GetComponentInChildren<Image>().enabled = true;
			infoDisplay.text = "";
		}
	}

	[System.Serializable]
	public class Fishe {
		public string name;
		public float length;
		public Fishe(string name, float length) {
			this.name = name;
			this.length = length;
		}
	}

	void setDrawImage() {
		if (fisheSymbols.Count != 0) {
			symbolToDrawDisplay.enabled = true;
			timeMeter.GetComponent<Image>().enabled = true;
			timeBar.GetComponentInChildren<Image>().enabled = true;
			symbolToDrawDisplay.color = new Color(1, 0.9596126f, 0.4858491f, 1);
			symbolToDrawDisplay.sprite = Resources.Load<Sprite>(fisheSymbols[0].ToString());
		} else {
			symbolToDrawDisplay.enabled = false;
			timeMeter.GetComponent<Image>().enabled = false;
			timeBar.GetComponentInChildren<Image>().enabled = false;
		}
	}

	void curbDifficulty() {
		if (fisheSymbols.Count != 0) {
			if (fisheSymbols[0] == "heart" || fisheSymbols[0] == "pentagram" || fisheSymbols[0] == "swoopy") {
				timeLeft += 1 - feedingFrenzy / (rollRange * 1.5f);
			}
		}
	}
}