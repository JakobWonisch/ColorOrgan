using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InFocus : MonoBehaviour
{

    public Material focused;

    private Material[] materialBases;
    private MeshRenderer[] meshRenderers;
    private Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        images = GetComponentsInChildren<Image>(); 
        materialBases = new Material[meshRenderers.Length + images.Length];

        for(int i=0; i<meshRenderers.Length; i++)
        {
            materialBases[i] = meshRenderers[i].material;
        }

        for (int i = 0; i < images.Length; i++)
        {
            materialBases[meshRenderers.Length + i] = images[i].material;
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

        for (int i = 0; i < images.Length; i++)
        {
            images[i].material = focused;
        }
    }

    public void Defocus()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material = materialBases[i];
        }

        for(int i = 0; i < images.Length; i++)
        {
            images[i].material = materialBases[meshRenderers.Length + i];
        }
    }

}
