using MidiJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMidi : MonoBehaviour
{

    public Sampler sampler;
    public KeyboardSpawner keyboard;

    // Start is called before the first frame update
    void Start()
    {
        MidiMaster.noteOnDelegate += noteOn;
        MidiMaster.noteOffDelegate += noteOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void noteOn(MidiChannel channel, int note, float velocity)
    {
        // ignore if velocity is zero
        if (velocity == 0)
            return;

        // Debug.Log("Note on " + note + " " + velocity);
        
        int m = note - 24; // midi offset

        // start note
        sampler.StartNote(m, velocity);

        keyboard.SimulateNote(m, velocity);
    }

    private void noteOff(MidiChannel chanel, int note)
    {
        // Debug.Log("Note off " + note);
        
        int m = note - 24;

        sampler.EndNote(m);
    }
}
