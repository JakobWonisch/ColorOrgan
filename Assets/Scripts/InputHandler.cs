using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputHandler : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean gripAction;

    public GameObject line;

    public Transform holdingPoint;

    private GameObject currentObject;

    private bool grabbing = false;

    private Quaternion startingRot;

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
            Debug.Log("Grab down " + handType);

            if(currentObject != null)
            {
                startingRot = Quaternion.Inverse(transform.rotation) * currentObject.transform.rotation;

                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp != null)
                    temp.Defocus();

                Cartridge cartridge = currentObject.GetComponent<Cartridge>();

                if(cartridge != null)
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
        if (!grabbing)
            DetectHits();
    }
    
    private void DetectHits() {
        int layerMask = 1 << 13;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            // Debug.Log("Did Hit" + hit.transform.gameObject.name);

            if(currentObject != null && currentObject != hit.transform.gameObject)
            {
                InFocus temp = currentObject.GetComponent<InFocus>();

                if (temp  != null)
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
        if(currentObject != null && grabbing)
        {
            currentObject.transform.position = holdingPoint.position;
            currentObject.transform.rotation = transform.rotation * startingRot;
        }
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
