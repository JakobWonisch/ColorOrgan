using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerKeyboard : MonoBehaviour
{

    public Sampler sampler;
    public KeyboardSpawner keyboard;

    private int octave = 3;

    private string[] keys =
    {
        "a",
        "w",
        "s",
        "e",
        "d",
        "f",
        "t",
        "g",
        "z",
        "h",
        "u",
        "j",
        "k",
    };

    // Update is called once per frame
    void Update()
    {

        // octave shift
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                octave = Math.Max(0, octave - 1);
            else
                octave++;
        }

        for(int i=0; i<keys.Length; i++)
        {
            int n = octave * 12 + i;

            if (Input.GetKeyDown(keys[i]))
            {
                sampler.StartNote(n);
                keyboard.SimulateNote(n, 1);
            }

            if (Input.GetKeyUp(keys[i]))
            {
                sampler.EndNote(n);
            }
        }  
    }
}
