﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Key : MonoBehaviour
{

    [HideInInspector]
    public int note = 36;

    [HideInInspector]
    public Material materialShine;

    private Sampler sampler;
    private Transform controllerLeft, controllerRight;
    private Vector3 lastLeft, lastRight, deltaLeft, deltaRight;

    private Material materialBase;
    private MeshRenderer meshRenderer;

    private Light pointLight;

    private Coroutine fadeOut;

    // Start is called before the first frame update
    void Start()
    {

        sampler = GameObject.Find("Piano").GetComponent<Sampler>();
        controllerLeft = GameObject.Find("Controller (left)").transform;
        controllerRight = GameObject.Find("Controller (right)").transform;

        lastLeft = controllerLeft.position;
        lastRight = controllerRight.position;

        meshRenderer = GetComponent<MeshRenderer>();
        materialBase = new Material(meshRenderer.material);

        pointLight = GetComponentInChildren<Light>();
        pointLight.color = materialShine.color;
        pointLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        deltaLeft = controllerLeft.position - lastLeft;
        lastLeft = controllerLeft.position;

        deltaRight = controllerRight.position - lastRight;
        lastRight = controllerRight.position;
       

        /*float duration = 2;
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        Debug.Log(lerp);
        meshRenderer.material.Lerp(materialBase, materialShine, lerp);
        */

    }

    void OnCollisionEnter(Collision collision)
    {
        bool isLeft = collision.gameObject.name.Equals("Controller (left)");

        if (!isLeft && !collision.gameObject.name.Equals("Controller (right)"))
            return;

        float v = isLeft ? deltaLeft.y : deltaRight.y;

        if (v > 0)
            return;

        if (v == 0)
            v = -0.05f; // have a min velocity

        float vel = Mathf.Min(1, -v / 0.05f);

        sampler.StartNote(note, vel);

        StartShine(vel);
    }

    void OnCollisionExit(Collision collision)
    {

        if (!collision.gameObject.name.Equals("Controller (left)") || !collision.gameObject.name.Equals("Controller (right)"))
            return;

        sampler.EndNote(note);
    }

    private void StartShine(float v)
    {
        if (fadeOut != null)
            StopCoroutine(fadeOut);

        fadeOut = StartCoroutine(FadeOut(v));
    }

    private IEnumerator FadeOut(float v)
    {
        while(v > 0)
        {
            v -= 0.01f;

            meshRenderer.material.Lerp(materialBase, materialShine, v);
            pointLight.intensity = v;

            yield return new WaitForSeconds(0.01f);
        }
    }
}
