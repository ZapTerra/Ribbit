using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFreckerColors : MonoBehaviour
{
    public Color flamesColor;
    public Color helixColor;
    public Color sparksColor;
    public ParticleSystem flames;
    public ParticleSystem helix1;
    public ParticleSystem helix2;
    public ParticleSystem helix3;
    public ParticleSystem helix4;
    public ParticleSystem sparks;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule m = flames.main;
        m.startColor = flamesColor;
        m = helix1.main;
        m.startColor = helixColor;
        m = helix2.main;
        m.startColor = helixColor;
        m = helix3.main;
        m.startColor = helixColor;
        m = helix4.main;
        m.startColor = helixColor;
        m = sparks.main;
        m.startColor = sparksColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
