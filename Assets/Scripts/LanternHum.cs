using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternHum : MonoBehaviour
{

    // adds an audio source when a lantern is close to the camera, so that a hum can be heard

    public GameObject humPrefab;

    public float humVolume = 0.3f;

    private int a4;

    private float semiFactor;

    // Start is called before the first frame update
    void Start()
    {
        a4 = Sampler.NoteToNumber("A4");

        semiFactor = (float)Math.Pow(2, 1 / 12f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // if object entered is a lantern, add humming source
        LanternMovement lm = other.GetComponentInParent<LanternMovement>();

        if (lm == null)
            return;

        GameObject go = Instantiate(humPrefab, other.transform.parent);
        AudioSource source = go.GetComponent<AudioSource>();

        // get difference to lantern note
        int diff = lm.note - a4;

        source.pitch = (float)Math.Pow(semiFactor, diff);
        source.volume = humVolume;

        source.Play();
    }

    void OnTriggerExit(Collider other)
    {
        // if object entered is a lantern, remove humming source

        LanternMovement lm = other.GetComponentInParent<LanternMovement>();

        if (lm == null)
            return;

        // destroy the right child
        Destroy(other.transform.parent.GetComponentInChildren<AudioSource>().gameObject);
    }
}
