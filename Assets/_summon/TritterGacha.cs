using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritterGacha : MonoBehaviour
{
    static public TritterGacha instance;
    public static List<Tritter> mostRecentPull = new List<Tritter>();
    private static List<List<Tritter>> tritterPool = new List<List<Tritter>>
    {
        new List<Tritter> {
            new Tritter("fectorio", false, 2),
            new Tritter("grunch", false, 2),
            new Tritter("hu", false, 2),
            new Tritter("peb", false, 2),
            new Tritter("zeb", false, 2),
            new Tritter("lu", false, 2),
            new Tritter("lu", false, 2),
        },
        new List<Tritter> {
            //tam
            new Tritter("greeb", false, 3),
            new Tritter("grouso", false, 3),
            new Tritter("hube", false, 3),
            new Tritter("lola", false, 3),
            new Tritter("tony", false, 3),
            new Tritter("rico", false, 3),
            new Tritter("ronk", false, 3),
            new Tritter("snorblet", false, 3),
            new Tritter("twill", false, 3),
            new Tritter("zobus", false, 3),
            new Tritter("zort", false, 3),
            new Tritter("morrisa", false, 3),
        },
        new List<Tritter> {
            new Tritter("boldus", false, 4),
            new Tritter("fueriios", false, 4),
            new Tritter("gongaldo", false, 4),
            new Tritter("haze", false, 4),
            new Tritter("hubert", false, 4),
            new Tritter("lucy", false, 4),
            new Tritter("molgual", false, 4),
            new Tritter("paulo", false, 4),
            new Tritter("solthaibrion", false, 4),
            new Tritter("tracter", false, 4),
            new Tritter("zettapede", false, 4),
        },
        new List<Tritter> {
            new Tritter("jeff", true, 5),
            new Tritter("mosset", true, 5),
            new Tritter("sapphire", true, 5),
        },
        new List<Tritter> {
            new Tritter("gunk", false, 1),
        }
    };

    public static List<Tritter> TritterPull(int PullManyTimes){
        instance.StartCoroutine(BigVibes());
        BeetleMaster.TritterDataRead();
        List<Tritter> pull = new List<Tritter>{};
        for(int i = 0; i < PullManyTimes; i++){
            int RNG = Random.Range(1, 101);
            int rarity = RNG < 100 ? RNG < 90 ? RNG < 70 ? RNG < 40 ? 1 : 2 : 3 : 4 : 5;
            pull.Add(tritterPool[rarity - 1][Random.Range(0,tritterPool[rarity - 1].Count)]);
            BeetleMaster.tritterCollection.Add(pull[i]);
            Debug.Log(pull[i].species);
        }
        mostRecentPull = pull;
        BeetleMaster.TritterDataWrite();
        return pull;
    }

    static IEnumerator BigVibes(){
        #if UNITY_ANDROID
        Vibration.Vibrate();
        yield return new WaitForSeconds(8f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        yield return new WaitForSeconds(.1f);
        Vibration.Vibrate();
        #elif UNITY_IOS
        Vibration.VibratePop();
        yield return new WaitForSeconds(8f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        yield return new WaitForSeconds(.1f);
        Vibration.VibratePop();
        #else
        Debug.Log("vibration");
        yield return null;
        #endif
    }

    void Awake(){
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
