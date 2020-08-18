using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Key : MonoBehaviour
{

    [HideInInspector]
    public int note = 36;

    private Sampler sampler;
    private Transform controllerLeft, controllerRight;
    private Vector3 lastLeft, lastRight, deltaLeft, deltaRight;

    // Start is called before the first frame update
    void Start()
    {
        sampler = GameObject.Find("Piano").GetComponent<Sampler>();
        controllerLeft = GameObject.Find("Controller (left)").transform;
        controllerRight = GameObject.Find("Controller (right)").transform;

        Debug.Log("Sampler: " + sampler);

        lastLeft = controllerLeft.position;
        lastRight = controllerRight.position;
    }

    // Update is called once per frame
    void Update()
    {
        deltaLeft = controllerLeft.position - lastLeft;
        lastLeft = controllerLeft.position;

        deltaRight = controllerRight.position - lastRight;
        lastRight = controllerRight.position;

    }

    void OnCollisionEnter(Collision collision)
    {
        bool isLeft = collision.gameObject.name.Equals("Controller (left)");

        Debug.Log(isLeft ? deltaLeft.y : deltaRight.y);

        float v = isLeft ? deltaLeft.y : deltaRight.y;

        if (v > 0)
            return;

        sampler.StartNote(note, Mathf.Min(1, -v / 0.05f));

    }

    void OnCollisionExit(Collision collision)
    {
        sampler.EndNote(note);
    }
}
