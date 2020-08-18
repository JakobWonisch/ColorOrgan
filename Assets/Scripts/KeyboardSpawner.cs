using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardSpawner : MonoBehaviour
{

    public GameObject[] keys;
    public string start, finish;

    private int startN, finishN;

    // Start is called before the first frame update
    void Start()
    {
        startN = Sampler.NoteToNumber(start);
        finishN = Sampler.NoteToNumber(finish);

        for (int i = 0; i <= finishN - startN; i++)
        {
            
            Vector3 position = new Vector3(i * 0.6f, 0, 0);
            GameObject go = Instantiate(keys[0], position, Quaternion.identity, transform);
            Key key = go.GetComponentInChildren<Key>();
            
            key.note = startN + i;

        }
    }

}
