using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaveMeasurement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Measuring child objects.");

        Vector3[] offsets = new Vector3[transform.childCount];

        Vector3 baseOffset = transform.GetChild(0).position;

        for(int i = 0; i < transform.childCount; i++)
        {
            offsets[i] = transform.GetChild(i).position - baseOffset;
        }

        string output = "";

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 pos = offsets[i];

            output += "new Vector3(" + pos.x + "f, " + pos.y + "f, " + pos.z + "f),\n";
        }

        //output += "};";


        Debug.Log(output);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
