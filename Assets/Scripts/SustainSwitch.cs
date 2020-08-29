using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainSwitch : MonoBehaviour
{

    public GameObject endTopPressed;

    public Material bOn, bOff, lOn, lOff;
    public MeshRenderer mrButton, mrLight;

    public Sampler sampler;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {

       SetActive(pressed);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.name.Equals("SwitchTop"))
            return;

        pressed = !pressed;

        SetActive(pressed);

        Debug.Log("Pressed: " + pressed);
        sampler.SetSustain(pressed);
    }

    private void SetActive(bool p)
    {
        endTopPressed.SetActive(p);

        mrButton.material = p ? bOn : bOff;
        mrLight.material = p ? lOn : lOff;
    }
}
