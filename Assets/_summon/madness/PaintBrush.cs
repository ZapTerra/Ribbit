using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public int resolution = 512;
    Texture2D whiteMap;
    public float brushSize;
    public Texture2D brushTexture;
    public Texture2D whiteTexture;
    bool drawing;
    float timerset;
    Collider col;
    Material rendredn;
    Vector2 stored;
    float clickval;
    Vector4 valuevec;
    bool clicked;
    float clicktimer;
    [SerializeField]
    Camera camcam;
    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        CreateClearTexture();// clear white texture to draw on
        rendredn = GetComponent<Renderer>().material;
        clickval = 1;
    }

    void Update()
    {
        if (clicked==true)
        {
            clicktimer += Time.deltaTime*9;
            clickval = Mathf.Min(1 - Mathf.Sin(clicktimer)*2,1);
            if (clicktimer>Mathf.PI)
            {
                clicked = false;
                clicktimer = 0;
            }
        }

        drawing = false;
        Debug.DrawRay(transform.position, Vector3.down * 20f, Color.magenta);
        RaycastHit hit;
        if (Physics.Raycast(camcam.ScreenPointToRay(Input.mousePosition), out hit)) // delete previous and uncomment for mouse painting
        {
            Collider coll = hit.collider;
            if (coll != null)
            {
                if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that
                {
                    col = coll;
                    Renderer rend = hit.transform.GetComponent<Renderer>();
                    paintTextures.Add(coll, getWhiteRT());
                    rend.material.SetTexture("_PaintMap", paintTextures[coll]);

                } else
                {
                    DrawTexture(paintTextures[coll], Input.mousePosition.x/Screen.width*resolution, Input.mousePosition.y / Screen.height * resolution);
                    drawing = true;
                    timerset = 1;
                    valuevec.Set(Input.mousePosition.x, Input.mousePosition.y, clickval, 0);
                    rendredn.SetVector("_MousePos", valuevec);
                }

            }
        } if (drawing == false && col!=null && timerset>0)
        {
            ClearTexture(paintTextures[col]);
            timerset -= Time.deltaTime;

        }
    }

    void DrawTexture(RenderTexture rt, float posX, float posY)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
        Graphics.DrawTexture(new Rect(0, 0, resolution, resolution), whiteTexture);
        // draw brushtexture
        float newbrushsize = brushSize - ((3 - (clickval + 2)) / 3) * 1.2f;
        Graphics.DrawTexture(new Rect(posX - brushTexture.width / newbrushsize, (rt.height - posY) - brushTexture.height / newbrushsize, brushTexture.width / (newbrushsize * 0.5f), brushTexture.height / (newbrushsize * 0.5f)), brushTexture);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture

    }


    void ClearTexture(RenderTexture rt)
    {

        RenderTexture.active = rt; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, resolution, resolution, 0);      // setup matrix for correct size
        Graphics.DrawTexture(new Rect(0, 0, resolution, resolution), whiteTexture);
        // draw brushtexture
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture

    }

    RenderTexture getWhiteRT()
    {
        RenderTexture rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(whiteMap, rt);
        return rt;
    }

    void CreateClearTexture()
    {
        whiteMap = new Texture2D(1, 1);
        whiteMap.SetPixel(0, 0, Color.white);
        whiteMap.Apply();
    }

    private void OnMouseDown()
    {
        clicked = true;
        clicktimer = 0;
    }
}