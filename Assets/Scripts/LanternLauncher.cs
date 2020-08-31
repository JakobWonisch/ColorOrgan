using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternLauncher : MonoBehaviour
{

    public GameObject lanternPrefab;
    public Material[] materials;

    public Transform mainCamera;
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
        float halfHeight = lanternPrefab.GetComponentInChildren<Renderer>().bounds.size.y / 2;
        
        // pick random point in spawn
        Vector3 rand = spawn.size;
        rand.Scale(new Vector3(Random.Range(0f, 1f) -0.5f, Random.Range(0f, 1f) - 0.5f, Random.Range(0f, 1f) - 0.5f));
        // rand += spawn.center + mainCamera.position;
        rand += spawn.center;

        rand = mainCamera.TransformPoint(rand);

        // check if there's anything above...
        // if so, move above that point
        RaycastHit[] hits;
        hits = Physics.RaycastAll(rand - Vector3.up * halfHeight, Vector3.up);

        float maxDistance = -1;
        RaycastHit furthestHit = new RaycastHit();

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            if (hit.distance > maxDistance)
            {
                furthestHit = hit;
                maxDistance = hit.distance;
            }
        }

        // Debug.Log("Hits: " + hits.Length + " distance " + maxDistance);

        // move lantern
        if(maxDistance >= 0)
        {
            Renderer r = furthestHit.transform.GetComponentInChildren<Renderer>();
            
            if (r != null)
                maxDistance += r.bounds.size.y;

            rand += Vector3.up * maxDistance;
            // Debug.Log("y: " + furthestHit.transform.GetComponent<Renderer>().bounds.size.y);
        }

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
