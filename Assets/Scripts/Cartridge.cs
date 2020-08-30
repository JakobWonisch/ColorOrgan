using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cartridge : MonoBehaviour
{

    public string sampleName, prettyName;
    public bool prefix,
        midiFile = false;

    public Text textInstrument, textMidi;
    public Image imageInstrument, imageMidi;

    private float ejectionForce = 2;

    public bool isHeld = false; // true if held by player

    void Start()
    {
        if (midiFile)
        {
            textMidi.text = prettyName;
            imageMidi.gameObject.SetActive(true);
            imageInstrument.gameObject.SetActive(false);
            textMidi.gameObject.SetActive(true);
            textInstrument.gameObject.SetActive(false);
        }
        else
        {
            textInstrument.text = prettyName;
            imageMidi.gameObject.SetActive(false);
            imageInstrument.gameObject.SetActive(true);
            textMidi.gameObject.SetActive(false);
            textInstrument.gameObject.SetActive(true);
        }
    }

    public void Eject(Transform awayFrom)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.AddRelativeForce(Vector3.up * ejectionForce, ForceMode.Impulse);

        Vector3 delta = transform.position - awayFrom.position;

        // build normal
        float t = delta.x;

        delta.x = delta.z;
        delta.z = t;
        delta.y *= -1;

        rb.AddForce(delta.normalized * ejectionForce, ForceMode.Impulse);
    }

    public void Eject()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.AddRelativeForce(Vector3.up * ejectionForce, ForceMode.Impulse);

    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public void Hold()
    {
        isHeld = true;
    }

    public void Drop()
    {
        isHeld = false;
    }
}
