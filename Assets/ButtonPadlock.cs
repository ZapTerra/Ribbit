using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPadlock : MonoBehaviour
{
    public float timeToRelock = 3f;
    public float chainThudTime = .25f;
    public float timeBetweenChainThuds = .2f;
    public float chainThudScale = 2f;
    public List<Image> chains;
    public Button breakButton;
    public Button button;
    private bool unlocking;
    private int chainIndex = -1;
    private float unlockStartTime;
//
    // Start is called before the first frame update
    void Start()
    {
        breakButton.enabled = false;
        foreach(Image i in chains){
            i.color = Color.clear;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(unlocking && Time.time - unlockStartTime > timeToRelock){
            Lock();
        }
    }

    public void Lock(){
        StartCoroutine(Relock());
    }

    public void BreakChain(){
        unlockStartTime = Time.time;

        if(!unlocking){
            StartCoroutine(RelockTimer());
            chainIndex = chains.Count - 1;
            unlocking = true;
        }
        
        StartCoroutine(BreakChain(chains[chainIndex]));

        chainIndex--;
        if(chainIndex < 0){
            button.interactable = true;
            breakButton.enabled = false;
        }
    }

    private IEnumerator Relock(){
        unlocking = false;
        button.interactable = false;
        breakButton.enabled = true;
        for(int i = chainIndex + 1; i < chains.Count; i++){
            StartCoroutine(ThudChain(chains[i]));
            yield return new WaitForSeconds(timeBetweenChainThuds);
        }
    }

    private IEnumerator BreakChain(Image chain){
        Vector3 smallScale = chain.transform.localScale;
        float time = 0;
        while(time < chainThudTime){
            chain.color = new Color(1, 1, 1, Mathf.Clamp(1 - time/chainThudTime, 0, chain.color.a));
            chain.transform.localScale = smallScale + (((chainThudScale * smallScale) - smallScale) * (time/chainThudTime));
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        chain.color = Color.clear;
        chain.transform.localScale = smallScale;
    }

    private IEnumerator ThudChain(Image chain){
        Vector3 smallScale = chain.transform.localScale;
        float time = chainThudTime;
        while(time > 0){
            chain.color = new Color(1, 1, 1, Mathf.Clamp(1 - time/chainThudTime, chain.color.a, 1));
            chain.transform.localScale = smallScale + (((chainThudScale * smallScale) - smallScale) * (time/chainThudTime));
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        chain.color = Color.white;
        chain.transform.localScale = smallScale;
        
        yield return null;
    }

    private IEnumerator RelockTimer(){
        yield return new WaitForSeconds(timeToRelock);
        
    }
}
