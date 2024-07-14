using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BeetleMaster : MonoBehaviour {
    //I don't know if this full line is necessaery, but I do not have an interet connection to check if it is and I know it will do its job perfectly.
    public static List<Tritter> tritterCollection = new List<Tritter>{};
    public GameObject pickupPlane;
    public GameObject spriteTritterPrefab;
    private float timeSinceLastSave;
    void Start()
    {
        //Instiantiate the prefabs for each tritter, update position etm.
        TritterDataRead();
        foreach (Tritter activeTritterData in tritterCollection)
        {
            GameObject tritter = Instantiate(spriteTritterPrefab);
            tritter.transform.parent = transform;
            tritter.transform.position = spriteTritterPrefab.transform.position;
            tritter.transform.localScale= spriteTritterPrefab.transform.localScale;
            TritterManager tritterManager = tritter.GetComponent<TritterManager>();
            tritterManager.childTritter.GetComponent<BeetleMove>().pickupPlane = pickupPlane;
            tritterManager.tritterData = activeTritterData;
        }
    }

    void Update()
    {
        timeSinceLastSave += Time.deltaTime;
        if(timeSinceLastSave >= 1){
            BeetleMaster.TritterDataWrite();
            timeSinceLastSave = 0;
        }
    }

    public static void TritterDataClear(){
        FileStream fs = new FileStream(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, BeetleMaster.tritterCollection);
			fs.Close();
			using (Stream stream = File.Open(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Open)) {
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				BeetleMaster.tritterCollection = (List<Tritter>)bformatter.Deserialize(stream);
			}
    }

    public static void TritterDataWrite(){
		FileStream fs = new FileStream(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Create);
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(fs, BeetleMaster.tritterCollection);
		fs.Close();
	}

    public static void TritterDataRead() {
		try {
			using (Stream stream = File.Open(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Open)) {
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				BeetleMaster.tritterCollection = (List<Tritter>)bformatter.Deserialize(stream);
			}
		} catch {
			FileStream fs = new FileStream(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Create);
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, BeetleMaster.tritterCollection);
			fs.Close();
			using (Stream stream = File.Open(Application.persistentDataPath + "/mostespecialfriends.json", FileMode.Open)) {
				var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				BeetleMaster.tritterCollection = (List<Tritter>)bformatter.Deserialize(stream);
			}
		}
	}
}

[System.Serializable]
public class Tritter{
    public string species;
    public string nickname;
    public string bio;
    public int rarity = 2;
    public bool special;
    public bool crawlOnTree = true;
    //90 because I'm not fixing my code and if I did it would probably break it because I was using euler angles instead quaternions
    public float rotation = 90;
    public float trunkXPos;
    public float trunkYPos;

    public Tritter(string species, bool special){
        this.species = species;
        this.special = special;
    }
    public Tritter(string species, bool special, int rarity){
        this.species = species;
        this.special = special;
        this.rarity = rarity;
    }
}