using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{

    public string sampleName;
    public bool prefix;

    private float ejectionForce = 2;

    private bool isHeld = false; // true if held by player

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
