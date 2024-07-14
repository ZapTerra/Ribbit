using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterSpawn : MonoBehaviour {

    [SerializeField]
    GameObject[] spawntypes;
    [SerializeField]
    Material mat;
    float timer;
    float minFov = 1;
    float maxFov = 100;
    float sensitivity = 100;
    float fovchange;
    Vector3 dragOrigin;
    float dragSpeed = 2;
    float xchange;
    float ychange;
    Vector3 campos;
    [SerializeField]
    bool testing;
    GameObject create;
    // Use this for initialization
    void Start () {
        OurStattyFriend.totalvalue = 0;
        int i = spawntypes.Length;
        i = Random.Range(0, i);
        if (!testing)
        {
            create = Instantiate(spawntypes[i]);
            create.GetComponent<PictureHolder>().starting = true;

        }
        mat.SetFloat("_Reveal", -1);
        timer = 0;
        if (testing)
        {
            timer = 2;
        }
        fovchange = Camera.main.orthographicSize;
        campos = Camera.main.transform.position;

    }

    // Update is called once per frame
    void Update () {

        if (timer<=2)
        {
            timer += Time.deltaTime/2;
            mat.SetFloat("_Reveal", timer);

        }
    }

    void LateUpdate()
    {
        if (timer<=1.4f)
        {
            create.transform.position += Vector3.up * Time.deltaTime/4;
        }
        float fov = Camera.main.orthographicSize;
        fovchange -= Input.GetAxis("Mouse ScrollWheel") * 10;
        fovchange = Mathf.Clamp(fovchange, minFov, maxFov);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,fovchange,Time.deltaTime*5);


        float x = 0;
        float y = 0;

        if (Input.GetMouseButton(0))
        {
            x = Input.GetAxis("Mouse X") * -0.7f*fov/7*Time.deltaTime*15;
            y = Input.GetAxis("Mouse Y") * -0.7f*fov/7*Time.deltaTime*15;
            //Camera.main.transform.Translate(x, y, 0);

        }

        if (Input.GetMouseButton(1))
        {
            Camera.main.transform.Rotate(Vector3.forward, Input.GetAxis("Mouse Y")*120*Time.deltaTime);
        }

        if (Input.GetMouseButton(2))
        {
            Camera.main.transform.position = new Vector3(0, 0, -10);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);
            Camera.main.orthographicSize = 5;
        }

        campos = campos + Vector3.up * y + Vector3.right * x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, campos, Time.deltaTime*10);

    }
}
