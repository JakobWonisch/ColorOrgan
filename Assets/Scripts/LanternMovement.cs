using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LanternMovement : MonoBehaviour
{

    [HideInInspector]
    public int note;

    private float startTime;

    private float bounce = 1.2f, timeScale = 10f, stiffness = 5,
        lift = 12, initialTilt = 15;

    private Rigidbody body;

    private Light lightObj; // disable light after time in seconds
    private float lightTime = 7;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        body = GetComponent<Rigidbody>();

        transform.localRotation = Quaternion.Euler(Random.Range(0, initialTilt), Random.Range(0, 360f), 0);

       // body.AddRelativeTorque(new Vector3(0, Random.Range(0, 1) * 2 - 1, 0), ForceMode.Impulse);

        lightObj = transform.GetChild(1).GetComponent<Light>();

        lightObj.color = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;

        t *= timeScale;

        float scale = (float) (Math.Sin(t * Math.PI) * bounce * (1 / (t + 1) / stiffness) + 1 - Math.Pow(1 / (t + 1) / stiffness, 2));
        transform.localScale = new Vector3(scale, scale, scale); 

        if(lightObj.enabled && Time.time - startTime > lightTime)
        {
            lightObj.enabled = false;
        }
    }

    void FixedUpdate()
    {
        // accelerate up
        body.AddForce(Vector3.up * lift);
    }
}
