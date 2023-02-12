using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liliePade : MonoBehaviour
{
    public GameObject froge;
    public GameObject liliepade;
    public GameObject fishe;
    public lilieArray e;
    public frogeFisheMove f;
    public float fisheOdds = 3;
    public int arrayPosX;
    public int arrayPosY;
    public bool amCenterStartLiliePade = false;

    private GameObject childeFishe;
    private int xWarpDir;
    private int yWarpDir;
    private float xVariance = 0;
    private float yVariance = 0;
    private float fixedScale;
    private bool transfer = false;
    private bool haveChecked = false;
    private bool isFirstUpdate = true;
    // Start is called before the first frame update
    void Start()
    {
        e = transform.parent.GetComponent<lilieArray>();
        f = froge.GetComponent<frogeFisheMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstUpdate)
        {
            newRandAttributes();
            isFirstUpdate = false;
            if (amCenterStartLiliePade)
            {
                froge.transform.position = transform.position;
            }
        }

        if (f.frogeState == 1)
        {
            if (!haveChecked)
            {
                posUpdate(f.jumpX, e.lilieColumns, ref arrayPosX, ref xWarpDir);
                posUpdate(f.jumpY, e.lilieRows, ref arrayPosY, ref yWarpDir);
                haveChecked = true;
            }
        }
        else
        {
            haveChecked = false;
        }

        if (!GetComponent<Renderer>().isVisible && transfer)
        {
            newRandAttributes();
            yWarpDir = 0;
            xWarpDir = 0;
            transfer = false;
        }
    }

    private void newRandAttributes()
    {
        //set parent so that movement to new position is matched to the screen aspect
        transform.parent = e.gameObject.transform;

        //move to new position
        transform.localPosition += new Vector3(xWarpDir * e.lilieColumns * e.liliePadeSpacing, yWarpDir * e.lilieRows * e.liliePadeSpacing);

        //randomize fixedScale and x/y variance within parameters, subtracting the previous x/y variance before adding the new variance
        fixedScale = e.defaultScale + Random.Range(-e.scaleVariance, e.scaleVariance);

        //go back to default position before randomization
        transform.position -= new Vector3(xVariance * e.width, yVariance * e.height);

        //get the amount to move for newly randomized position
        xVariance = Random.Range(-e.xyVariance, e.xyVariance);
        yVariance = Random.Range(-e.xyVariance, e.xyVariance);

        //move to randomized position, randomly rotate, set parent to null to avoid wonky scaling, and change to new random size
        transform.position += new Vector3(xVariance * e.width, yVariance * e.height);
        transform.eulerAngles += Vector3.forward * Random.Range(0, 361);
        transform.parent = null;
        transform.localScale = new Vector3(fixedScale, fixedScale, 1);

        //randomize whether lilypad has a fish
        Destroy(childeFishe);
        if (Random.Range(0, fisheOdds) > fisheOdds - 1)
        {
            childeFishe = Instantiate(fishe);
            childeFishe.transform.parent = gameObject.transform;
            childeFishe.transform.localPosition = Vector3.zero;
        }
    }

    private void posUpdate(int destination, int jump, ref int self, ref int warpDir)
    {
        if (destination != 0 && Mathf.Abs(self) > Mathf.Ceil((jump - 1) / 2) - Mathf.Abs(destination) && Mathf.Sign(self) != Mathf.Sign(destination))
        {
            self += (destination / Mathf.Abs(destination)) * jump - destination;
            warpDir = destination / Mathf.Abs(destination);
            transfer = true;
        }
        else
        {
            self -= destination;
        }
    }
}
