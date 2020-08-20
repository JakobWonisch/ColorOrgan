using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFocus : MonoBehaviour
{

    public Material focused;

    private Material[] materialBases;
    private MeshRenderer[] meshRenderers;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        materialBases = new Material[meshRenderers.Length];

        for(int i=0; i<materialBases.Length; i++)
        {
            materialBases[i] = meshRenderers[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Focus()
    {
        for(int i=0; i<meshRenderers.Length; i++)
        {
            meshRenderers[i].material = focused;
        }
    }

    public void Defocus()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = materialBases[i];
        }
    }

}
