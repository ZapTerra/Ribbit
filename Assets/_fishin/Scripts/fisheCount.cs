using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class fisheCount : MonoBehaviour {
	public TMPro.TextMeshProUGUI fisheScore1;
	public TMPro.TextMeshProUGUI fisheScore2;
	public TMPro.TextMeshProUGUI fisheScore3;
	public TMPro.TextMeshProUGUI fisheScore4;
	public TMPro.TextMeshProUGUI fisheScore5;
	public TMPro.TextMeshProUGUI fisheScore6;
	public TMPro.TextMeshProUGUI fishesCaugght;
	private string[][] fishes;

	void Start(){
		DoSaveData();
	}


	// Start is called before the first frame update
public void DoSaveData() {
		// fisheEatBobe.fisheCount[0] = PlayerPrefs.GetInt("common fishe");
		// fisheEatBobe.fisheCount[1] = PlayerPrefs.GetInt("uncommon fishe");
		// fisheEatBobe.fisheCount[2] = PlayerPrefs.GetInt("rare fishe");
		// fisheEatBobe.fisheCount[3] = PlayerPrefs.GetInt("epic fishe");
		// fisheEatBobe.fisheCount[4] = PlayerPrefs.GetInt("legendary fishe");
		// fisheEatBobe.fisheCount[5] = PlayerPrefs.GetInt("mythic fishe");
		try {
			using (Stream stream = File.Open(Application.persistentDataPath + "/save.json", FileMode.Open)) {
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				fisheEatBobe.caugghtFisheList = (List<List<fisheEatBobe.Fishe>>)bformatter.Deserialize(stream);
			}
		} catch {
			FileStream fs = new FileStream(Application.persistentDataPath + "/save.json", FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, fisheEatBobe.caugghtFisheList);
			fs.Close();
			using (Stream stream = File.Open(Application.persistentDataPath + "/save.json", FileMode.Open)) {
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				fisheEatBobe.caugghtFisheList = (List<List<fisheEatBobe.Fishe>>)bformatter.Deserialize(stream);
			}
		}
	}

	// Update is called once per frame
	void Update() {
		DoSaveData();
	}


	public void FishesCensus() {
		// PlayerPrefs.SetInt("common fishe", fisheEatBobe.fisheCount[0]);
		// PlayerPrefs.SetInt("uncommon fishe", fisheEatBobe.fisheCount[1]);
		// PlayerPrefs.SetInt("rare fishe", fisheEatBobe.fisheCount[2]);
		// PlayerPrefs.SetInt("epic fishe", fisheEatBobe.fisheCount[3]);
		// PlayerPrefs.SetInt("legendary fishe", fisheEatBobe.fisheCount[4]);
		// PlayerPrefs.SetInt("mythic fishe", fisheEatBobe.fisheCount[5]);

		//Previously counted the fishes the player owned of each rarity
		//fisheScore1.text = fisheEatBobe.fisheCount[0].ToString();
		//fisheScore2.text = fisheEatBobe.fisheCount[1].ToString();
		//fisheScore3.text = fisheEatBobe.fisheCount[2].ToString();
		//fisheScore4.text = fisheEatBobe.fisheCount[3].ToString();
		//fisheScore5.text = fisheEatBobe.fisheCount[4].ToString();
		//fisheScore6.text = fisheEatBobe.fisheCount[5].ToString();



		FileStream fs = new FileStream(Application.persistentDataPath + "/save.json", FileMode.Create);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(fs, fisheEatBobe.caugghtFisheList);
		fs.Close();
	}


	public string GetFormattedFishesList() {
		DoSaveData();
		FishesCensus();
		string fishes = "";
		for (int f = 0; f < fisheEatBobe.caugghtFisheList.Count; f++) {
			var fisheList = new List<string>();
			fishes += (f < 1 ? "Common" : f < 2 ? "\nUncommon" : f < 3 ? "\nRare" : f < 4 ? "\nEpic" : f < 5 ? "\nLegendary" : "\nMythic") + ":\n";

			for (int r = 0; r < fisheEatBobe.caugghtFisheList[f].Count; r++) {
				fisheList.Add(fisheEatBobe.caugghtFisheList[f][r].name);
			}

			var o = from g in fisheList
					group g by g into g
					let count = g.Count()
					orderby count descending
					select new { Value = g.Key, Count = count };
			var a = true;
			foreach (var e in o) {
				if (a) {
					a = false;
				} else {
					fishes += ", ";
				}
				fishes += e.Value + ": " + e.Count;
			}
		}
		return fishes;
	}

	public static List<List<string>> GetFormattedFishCensus() {
		var fishes = new List<List<string>>();
		for (int f = 0; f < fisheEatBobe.caugghtFisheList.Count; f++) {
			fishes.Add(new List<string> { });
			for (int r = 0; r < fisheEatBobe.caugghtFisheList[f].Count; r++) {
				fishes[f].Add(fisheEatBobe.caugghtFisheList[f][r].name);
			}
		}
		return fishes;
	}
}
