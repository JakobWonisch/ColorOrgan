using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternLauncher : MonoBehaviour
{

    public GameObject lanternPrefab;
    public Material[] materials;

    public BoxCollider spawn;

    public int maxLanterns = 1000;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // notes starting from c1
    public void Launch(int n)
    {
        
        // pick random point in spawn
        Vector3 rand = spawn.size;
        rand.Scale(new Vector3(Random.Range(0f, 1f) -0.5f, Random.Range(0f, 1f) - 0.5f, Random.Range(0f, 1f) - 0.5f));
        rand += spawn.center + Camera.main.transform.position;
        
        GameObject go = Instantiate(lanternPrefab, rand, Quaternion.identity, transform);
        // go.transform.GetChild(0).GetComponent<MeshRenderer>().material = materials[n % materials.Length];
        MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
        
        foreach(MeshRenderer mr in mrs)
        {
            mr.material = materials[n % materials.Length];
        }

        Rigidbody rb = go.GetComponent<Rigidbody>();
        
        rb.position = rand;
        rb.velocity = Vector3.zero;

        go.GetComponent<LanternMovement>().note = n;
        for( int i = 0; transform.childCount - i > maxLanterns; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }
}
