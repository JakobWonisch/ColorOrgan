using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDestroy : MonoBehaviour
{

    private AudioSource source;

    [HideInInspector]
    public float startTime;
    [HideInInspector]
    public int note;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if not playing anymore and destory self
        if (!source.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
