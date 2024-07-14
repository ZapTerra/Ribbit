using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PictureHolder : MonoBehaviour
{

    public Texture2D texture;
    [SerializeField]
    Vector2 leftRightCrop;
    [SerializeField]
    Vector2 upDownCrop;
    [SerializeField]
    float rotation;
    [SerializeField]
    bool ignoreflags;
    Sprite sprite;
    SpriteRenderer sr;
    Rect rect;
    Vector2 pivot;
    float angle = 0;
    Vector2 size;
    Vector2 pos;
    ImageHolder testimg;
    PictureHolder testholder;
    Texture2D testtexture;
    int testval;
    int currentcleared;
    float chance = 100;
    float power = 0.99f;
    int[] totalnumbers = { 0, 0, 0, 0, 0, 0, 0, 0};
    public bool testing;
    int sortingording;
    public bool anythinggoes;
    int flag;
    public bool sometimes;
    public bool starting;
    public bool starting2;
    enum Occupied
    {
        One = 0,    
        Two = 1,    
        Three = 2,    
        Four = 4,    
        Five = 8,     
        Six = 16,
        Seven = 32,
        Eight = 64
    }

    public ImageHolder[] images;

    [System.Serializable]
    public struct ImageHolder
    {
        public GameObject picture;
        public Vector2 position;
        public Vector2 leftRightCrop;
        public Vector2 upDownCrop;
        public float scale;
        public float rotation;
        public bool testvisible;
        public Texture2D testtext;
        public int flag;
        public int sort;
    };

    public Texture2D getTex()
    {
        return texture;
    }

    private void Update()
    {

    }

    public void SetChance(float nchance,float npower)
    {
        if (nchance >=100)
        {
            chance = npower * 100;
        } else
        {
            chance = nchance*npower;

        }
        if (sometimes == true)
        {
            chance = 0.25f;
        }
        power = npower;
    }

    // Use this for initialization

    void Start()
    {
        sprite = Sprite.Create(texture, new Rect(0.0f+leftRightCrop.x, 0.0f+upDownCrop.x, texture.width-leftRightCrop.y, texture.height-upDownCrop.y+upDownCrop.x), new Vector2(0.5f, 0.5f+(-upDownCrop.x/ texture.height)), 100.0f);
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        int val = 0;
        sortingording = GetComponent<SpriteRenderer>().sortingOrder;
        foreach (ImageHolder img in images)
        {
            totalnumbers[img.flag]++;
            if (img.testvisible == true)
            {
                testimg = img;
                testholder = img.picture.GetComponent<PictureHolder>();
                testtexture = img.testtext;
                testval = val;
            }
            val++;
        }



        for (int i = 0; i<7; i++)
        {
            if (totalnumbers[i] == 0 || (((int)Mathf.Pow(2,i+1) & currentcleared) != 0 && ignoreflags == false && starting2 == false))
            {
                totalnumbers[i] = -2;
            } else 
            {
                totalnumbers[i] = Random.Range(0, totalnumbers[i]);
            }
        }
        if (images.Length>0)
        {
            UpdateSettings();

        }
        foreach (ImageHolder img in images)
        {
            totalnumbers[img.flag]--;
            if (totalnumbers[img.flag]==-1 || anythinggoes == true) { 
                if (Random.Range(0,100)<chance*100)
                {
                    GameObject create = Instantiate(img.picture,gameObject.transform);
                    create.transform.localPosition = img.position;
                    create.transform.localEulerAngles = Vector3.forward * img.rotation;
                    create.transform.localScale = new Vector3(img.scale,Mathf.Abs(img.scale), Mathf.Abs(img.scale));
                    create.GetComponent<SpriteRenderer>().sortingOrder = sortingording+img.sort;
                    if (create.GetComponent<PictureHolder>()!=null)
                    {
                        PictureHolder ph = create.GetComponent<PictureHolder>();
                        if (!ignoreflags)
                        {
                            ph.SetCrop(img.leftRightCrop, img.upDownCrop);
                            ph.SetFlag(img.flag);

                        }
                        if (ignoreflags)
                        {
                            ph.SetCrop(leftRightCrop, upDownCrop);
                            ph.SetFlag(flag);

                        }
                        if (starting==true && ignoreflags == true)
                        {
                            ph.GetComponent<PictureHolder>().starting2 = true;
                        }
                        ph.GetComponent<PictureHolder>().SetChance(chance, power);
                        OurStattyFriend.totalvalue++;
                    }
                }
            }
        }
    }

    void SetCrop(Vector2 lr,Vector2 ud)
    {
        leftRightCrop = lr;
        upDownCrop = ud;
    }

    void SetFlag(int f)
    {

        flag = f;
        if (f == 6)
        {
            currentcleared |= (int)Mathf.Pow(2, 1);
            currentcleared |= (int)Mathf.Pow(2, 2);


        } else
        {
            currentcleared |= (int)Mathf.Pow(2, f + 1);

        }

    }

    void UpdateSettings()
    {
        /*
        if (images.Length > 0 && testing == true)
        {
            pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + images[testval].position.x * transform.localScale.x, -transform.position.y - images[testval].position.y * transform.localScale.y, 0));
            Vector2 bottomleft = Camera.main.WorldToScreenPoint(new Vector2(images[testval].position.x - (images[testval].testtext.width / 200f) * images[testval].scale * transform.localScale.x, images[testval].position.y - (images[testval].testtext.height / 200f) * Mathf.Abs(images[testval].scale) * transform.localScale.y));
            Vector2 topright = Camera.main.WorldToScreenPoint(new Vector3(images[testval].position.x + (images[testval].testtext.width / 200f) * images[testval].scale * transform.localScale.x, images[testval].position.y + (images[testval].testtext.height / 200f) * Mathf.Abs(images[testval].scale) * transform.localScale.y));
            size = topright - bottomleft;
            angle = -images[testval].rotation;
            rect = new Rect(pos.x - size.x * 0.5f, pos.y - size.y * 0.5f, size.x, size.y);
            pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);
        }*/
        
    }





    void OnGUI()
    {
        
        if (images.Length > 0 && testing == true)
        {
            if (Application.isEditor) { UpdateSettings(); }
            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, pivot);
            GUI.DrawTexture(rect, images[testval].testtext);
            //GUI.DrawTexture(rect, images[testval].testtext);

            GUI.matrix = matrixBackup;
        }
    }


}
