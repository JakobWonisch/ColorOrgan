using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainSwitch : MonoBehaviour
{

    public GameObject endTopPressed;

    public Sampler sampler;

    private bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {

       endTopPressed.SetActive(pressed);

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

       endTopPressed.SetActive(pressed);
       
        Debug.Log("Pressed: " + pressed);
        sampler.SetSustain(pressed);
    }
}
