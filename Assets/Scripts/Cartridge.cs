using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cartridge : MonoBehaviour
{

    public string sampleName;
    public bool prefix;

    public Text textField;

    private float ejectionForce = 2;

    public bool isHeld = false; // true if held by player

    void Start()
    {
        textField.text = sampleName;
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
