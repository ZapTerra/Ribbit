using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lilieArray : MonoBehaviour
{
    public float liliePadeSpacing = 5;
    public float defaultScale = 1.45f;
    public float scaleVariance = .25f;
    public float xyVariance;
    public float fixedScale = 1;
    public int lilieRows = 3;
    public int lilieColumns = 3;
    public float width;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        xyVariance = liliePadeSpacing / 2 - (defaultScale + scaleVariance) / 2;
        //           ^   (insert var here if temp replacing)
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        height = edgeVector.y * 2 / 10;
        width = edgeVector.x * 2 / 10;
        transform.localScale = new Vector2(width, height);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
