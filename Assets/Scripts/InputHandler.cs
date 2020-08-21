using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputHandler : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean gripAction;
    public SteamVR_Action_Vibration vibrationAction;

    public GameObject line;

    public Transform holdingPoint;

    private GameObject currentObject;

    private bool grabbing = false;

    private Quaternion startingRot;

    private Vector3 lastLocation, delta;
    private bool wasGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (GetGrabDown())
        {
            grabbing = true;
            wasGrabbing = true;
            Debug.Log("Grab down " + handType);

            if (currentObject != null)
            {
                startingRot = Quaternion.Inverse(transform.rotation) * currentObject.transform.rotation;

                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp != null)
                    temp.Defocus();

                Cartridge cartridge = currentObject.GetComponent<Cartridge>();

                if (cartridge != null)
                {
                    cartridge.Hold();
                }
            }
        }

        if (GetGrabUp())
        {
            grabbing = false;
            Debug.Log("Grab up " + handType);

            if (currentObject != null)
            {
                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp != null)
                    temp.Focus();

                Cartridge cartridge = currentObject.GetComponent<Cartridge>();

                if (cartridge != null)
                {
                    cartridge.Drop();
                }
            }
        }

        if (GetGripDown())
        {
            line.SetActive(true);
        }

        if (GetGripUp())
        {
            line.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (currentObject != null && wasGrabbing)
        {
            Rigidbody rb = currentObject.GetComponent<Rigidbody>();

            Debug.Log("Throwing " + (delta / Time.deltaTime));
            Debug.Log("Test " + (currentObject.transform.position - lastLocation));
            Debug.Log("time " + Time.deltaTime);

            if (rb == null)
            {
                Vector3 vel = delta / Time.deltaTime;

                // vel = currentObject.transform.InverseTransformVector(vel);
                vel = currentObject.transform.rotation * vel;
                rb.velocity = vel;
                // rb.AddForce(vel * rb.mass, ForceMode.Impulse);
            }
        }

        if (!grabbing)
            DetectHits();
    }

    private void DetectHits()
    {
        int layerMask = 1 << 13;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Did Hit" + hit.transform.gameObject.name);

            if (currentObject != null && currentObject != hit.transform.gameObject)
            {
                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp != null)
                    temp.Defocus();
            }

            currentObject = hit.transform.gameObject;
            InFocus inFocus = currentObject.GetComponent<InFocus>();

            if (inFocus != null)
                inFocus.Focus();
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            // Debug.Log("Did not Hit");

            if (currentObject != null)
            {
                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp != null)
                    temp.Defocus();
            }

            currentObject = null;
        }

    }

    void LateUpdate()
    {
        if (currentObject != null)
        {
            if (grabbing)
            {
                /*Rigidbody rb = currentObject.GetComponent<Rigidbody>();

                if (rb == null)
                {*/
                currentObject.transform.position = holdingPoint.position;
                currentObject.transform.rotation = transform.rotation * startingRot;

                if (lastLocation != null)
                    delta = currentObject.transform.position - lastLocation;

                lastLocation = currentObject.transform.position;
                Debug.Log("Location " + lastLocation);
                /*}
                else
                {
                    rb.position = holdingPoint.position;
                    rb.rotation = transform.rotation * startingRot;

                }*/
            }
        }


    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter" + collision.gameObject.name);
        vibrationAction.Execute(0, 0.05f, 100, 0.2f, handType);
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private bool GetGrabDown()
    {
        return grabAction.GetStateDown(handType);
    }

    private bool GetGrabUp()
    {
        return grabAction.GetStateUp(handType);
    }
    private bool GetGripDown()
    {
        return gripAction.GetStateDown(handType);
    }

    private bool GetGripUp()
    {
        return gripAction.GetStateUp(handType);
    }
}
