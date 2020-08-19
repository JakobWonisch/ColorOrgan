using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartridgeSlot : MonoBehaviour
{
    public Sampler sampler;

    private Cartridge current;

    private float snapSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current != null)
        {
            // animate cartridge to holding position
            current.transform.rotation = Quaternion.Slerp(current.transform.rotation, transform.rotation, snapSpeed);
            current.transform.position = Vector3.Lerp(current.transform.position, transform.position, snapSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        Cartridge next = other.transform.parent.GetComponent<Cartridge>();

        if (next == null || !next.IsHeld())
            return;
        
        if(current != null)
        {
            // eject
            current.GetComponent<Rigidbody>().isKinematic = false;
            current.Eject(other.transform.parent);
        }

        // move new to current
        current = next;
        current.GetComponent<Rigidbody>().isKinematic = true;

        sampler.SetInstrument(current.sampleName, current.prefix);
    }

}
