using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TritterManager : MonoBehaviour
{
    public Tritter tritterData;
    public GameObject childTritter;
    public BeetleMove childMove;
    private float timeSinceLastSave;

    void Start()
    {
        if(tritterData.special){
            var tritterBody = Resources.Load<GameObject>(tritterData.species.ToString());
            var instantiated = Instantiate(tritterBody);
            instantiated.transform.parent = childTritter.transform;
            instantiated.transform.localPosition = tritterBody.transform.position;
            //instantiated.transform.localScale = tritterBody.transform.localScale;
            childTritter.GetComponent<SpriteRenderer>().sprite = null;
        }else{
            childTritter.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(tritterData.species.ToString());
        }
        childMove = childTritter.GetComponent<BeetleMove>();
        childMove.tritterData = tritterData;
    }

    void Update()
    {

    }
}
