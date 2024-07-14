using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class panelOpener : MonoBehaviour {
	public GameObject panel;
	public GameObject homeButtonToDisable;
	public GameObject currencyToDisable;
	public GameObject gameManager;
	public GameObject prefabFishPlate;
	public GameObject commonFishGroup;
	public GameObject uncommonFishGroup;
	public GameObject rareFishGroup;
	public GameObject epicFishGroup;
	public GameObject legendaryFishGroup;
	public GameObject mythicFishGroup;
	private List<List<string>> playerFishes;
	private bool hasSubspecies;

	public TMPro.TextMeshProUGUI fishesListDisplay;
	public static bool paused;
	public void OpenPanel() {
		Time.timeScale = Time.timeScale != 0 ? 0 : 1;
		paused = Time.timeScale == 0 ? true : false;
		if (homeButtonToDisable != null) {
			homeButtonToDisable.SetActive(!paused);
		}
		if (currencyToDisable != null) {
			currencyToDisable.SetActive(!paused);
		}


		//fishesListDisplay.text = gameManager.GetComponent<fisheCount>().GetFormattedFishesList();
		if(paused){
			playerFishes = fisheCount.GetFormattedFishCensus();
			for (int i = 0; i < fisheEatBobe.fishePool.Count; i++) {
				for (int x = 0; x < fisheEatBobe.fishePool[i].Count; x++) {
					var currentSpecies = fisheEatBobe.fishePool[i][x];
					var activePlate = Instantiate(prefabFishPlate);
					var plateManager = activePlate.GetComponentInChildren<FishPlateManager>();
					var caught = 0;
					foreach (List<string> l in playerFishes)
						foreach (string z in l)
							if (z.Contains(currentSpecies)){
								caught++;
								if(z != currentSpecies){
									hasSubspecies = true;
								}
							}
					if(0 < caught)
						plateManager.haveCaught = true;
					plateManager.fishType = currentSpecies;
					plateManager.hasSubspecies = hasSubspecies;
					hasSubspecies = false;
					plateManager.caught.text = caught.ToString();
					plateManager.transform.parent.SetParent(returnGroup(i).transform);
					plateManager.transform.parent.localScale = prefabFishPlate.transform.localScale;
				}
			}
		}
		panel.SetActive(!panel.activeInHierarchy);
	}

	private GameObject returnGroup(int rare){
		switch(rare){
			case 0: return commonFishGroup;
			case 1: return uncommonFishGroup;
			case 2: return rareFishGroup;
			case 3: return epicFishGroup;
			case 4: return legendaryFishGroup;
			case 5: return mythicFishGroup;
		}
		return mythicFishGroup;
	}
}
//fisheSprite.sprite = Resources.Load<Sprite>(fisheName.ToString());