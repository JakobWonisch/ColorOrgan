using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgeSlot : MonoBehaviour
{
    public Sampler sampler;
    public MidiPlayer midiPlayer;

    public Light onLight;
    public MeshRenderer mrLight;
    public Material lightOn, lightOff;

    public Transform holdingPoint;

    [HideInInspector]
    public Cartridge current;

    private float snapSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        SetLightOff();
    }

    // Update is called once per frame
    void Update()
    {
        if (current != null)
        {
            // animate cartridge to holding position
            current.transform.rotation = Quaternion.Slerp(current.transform.rotation, holdingPoint.rotation, snapSpeed);
            current.transform.position = Vector3.Lerp(current.transform.position, holdingPoint.position, snapSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform otherParent = other.transform.parent;

        if (otherParent == null)
            return;

        Cartridge next = otherParent.GetComponent<Cartridge>();

        if (next == null || !next.IsHeld() || current == next)
            return;

        if (current != null)
        {
            // eject
            current.GetComponent<Rigidbody>().isKinematic = false;

            if (current.midiFile)
                midiPlayer.StopMidi();

            current.Eject(other.transform.parent);
        }

        // move new to current
        current = next;
        current.GetComponent<Rigidbody>().isKinematic = true;
        SetLightOn();

        if (current.midiFile)
        {
            midiPlayer.PlayMidi(current.sampleName);
        }
        else
        {
            sampler.SetInstrument(current.sampleName, current.prefix);
        }

        Debug.Log("Cartridge set: " + current.sampleName);
    }

    public void UnsetCurrent()
    {
        if (current != null)
        {
            current.GetComponent<Rigidbody>().isKinematic = false;
            
            if (current.midiFile)
                midiPlayer.StopMidi();

            current = null;
            SetLightOff();
        }
    }

    public void EjectCurrent()
    {

        if (current != null)
        {
            // eject
            current.GetComponent<Rigidbody>().isKinematic = false;

            if (current.midiFile)
                midiPlayer.StopMidi();

            current.Eject();
            current = null;
            SetLightOff();
        }

    }

    private void SetLightOn()
    {
        mrLight.material = lightOn;
        onLight.enabled = true;
    }

    private void SetLightOff()
    {
        mrLight.material = lightOff;
        onLight.enabled = false;
    }

}
