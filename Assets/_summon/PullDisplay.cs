using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullDisplay : MonoBehaviour
{
    public Animator animator;
    public TMPro.TextMeshProUGUI tritterName;
    private Tritter tritterData;
    // Start is called before the first frame update
    void Start()
    {

        tritterData = TritterGacha.mostRecentPull[0];
        tritterName.text = "~" + tritterData.species + "~";

         if(tritterData.special){
            var tritterBody = Resources.Load<GameObject>(tritterData.species.ToString());
            var instantiated = Instantiate(tritterBody);
            instantiated.transform.parent = transform;
            instantiated.transform.rotation = Quaternion.identity;
            instantiated.transform.localScale *= 2;
            transform.position = Vector3.zero;
            animator.SetBool("rotato", true);
            SetGraphicsLayers("Default");
            instantiated.transform.position = transform.position;
            GetComponent<SpriteRenderer>().sprite = null;
        }else{
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(tritterData.species.ToString());
        }
    }
    void SetGraphicsLayers(string layer) {
		foreach (Transform child in transform){
			child.gameObject.layer = LayerMask.NameToLayer(layer);
			foreach (Transform grandchild in child.transform)
				grandchild.gameObject.layer = LayerMask.NameToLayer(layer);
		}
		gameObject.layer = LayerMask.NameToLayer(layer);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
