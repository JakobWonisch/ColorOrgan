using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KeyboardSpawner : MonoBehaviour
{

    public GameObject[] keys;
    public string start, finish;

    public Material[] materials;

    private int startN, finishN;

    private Vector3[] offsetsFromOrigin = {
       /* new Vector3(0,0,0),
        new Vector3(0.358f, 0.209f, 0.8509998f),
        new Vector3(0.723f, 0.143f, -0.9089999f),
        new Vector3(1.067f, 0.204f, 0.8369999f),
        new Vector3(1.307f, 0.141f, -0.7590003f),
        new Vector3(1.817f, 0.131f, 0.001999855f),
        new Vector3(2.175f, 0.209f, 0.8529997f),
        new Vector3(2.54f, 0.173f, -0.907001f),
        new Vector3(2.884f, 0.204f, 0.8389997f),
        new Vector3(3.249f, 0.148f, -0.921001f),
        new Vector3(3.593f, 0.209f, 0.8249998f),
        new Vector3(3.833f, 0.146f, -0.7710013f),
        new Vector3(4.334f, -0.00999999f, -0.00300014f),*/
       new Vector3(0f, 0f, 0f),
        new Vector3(0.358f, 0.209f, 0.8509998f),
        new Vector3(0.723f, 0.143f, -0.909f),
        new Vector3(1.067f, 0.204f, 0.8369998f),
        new Vector3(1.307f, 0.141f, -0.7590004f),
        new Vector3(1.817f, 0.008999944f, 0.001999795f),
        new Vector3(2.175f, 0.209f, 0.8529996f),
        new Vector3(2.54f, 0.173f, -0.9070011f),
        new Vector3(2.884f, 0.204f, 0.8389996f),
        new Vector3(3.249f, 0.148f, -0.9210011f),
        new Vector3(3.593f, 0.209f, 0.8249997f),
        new Vector3(3.833f, 0.146f, -0.7710015f),
        new Vector3(4.334f, -0.00999999f, -0.003000259f),
    };

    private Vector3[] offsets;

    private Key[] keyObjects;

    // Start is called before the first frame update
    void Start()
    {

        // prepare offsets
        offsets = new Vector3[offsetsFromOrigin.Length - 1];
        // offsets[0] = offsetsFromOrigin[0];
        for(int i=1; i<offsetsFromOrigin.Length; i++)
        {
            offsets[i % offsets.Length] = offsetsFromOrigin[i] - offsetsFromOrigin[i - 1];
        }

        startN = Sampler.NoteToNumber(start);
        finishN = Sampler.NoteToNumber(finish);

        Vector3 position = -offsets[startN % 12];
        for (int i = 0; i <= finishN - startN; i++)
        {
            int k = GetKey(startN + i);

            position += offsets[(startN + i) % 12];
            GameObject go = Instantiate(keys[k], transform.position + position, Quaternion.identity, transform);
            Key key = go.GetComponentInChildren<Key>();
            
            key.note = startN + i;
            key.materialShine = materials[(startN + i) % materials.Length];

        }

        keyObjects = GetComponentsInChildren<Key>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            Vector3[] vs = {
                new Vector3(0,0,1),
                new Vector3(0,0,1),
            };
            // print out positions
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            string output = "Vector3[] offsets = {\n";
            
            for(int i = 0; i<transform.childCount; i++)
            {
                Vector3 pos = transform.GetChild(i).localPosition;

                output += "new Vector3(" + pos.x + "f, " + pos.y + "f, " + pos.z + "f),\n";
            }

            output += "};";

            Debug.LogWarning(output);
        }
    }

    public void SimulateNote(int n, float v)
    {
        // search for key
        foreach (Key k in keyObjects)
        {
            if (k.note != n)
                continue;

            k.SimulateNote(v);
            break;
        }
    }

    private int GetKey(int n)
    {
        int m = n % 12;

        if (m < 5)
            return m;
        else if (m < 9)
            return m - 5;
        else
            return m - 7;
    }

    public int GetStart()
    {
        return startN;
    }

}
