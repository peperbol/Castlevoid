using UnityEngine;
using System.Collections;

public class CloudWiggle : MonoBehaviour {
    public float outerradius = 2;
    public float innerradius = 1;
    public float speed = 0.001f;
    public float pullforce = 0.5f;
    Vector3 pos;
    Vector3 locpos;
    bool returning = true;
    Vector3 target;
    Vector3 velocity;
    void Start () {
        pos = transform.position;
        locpos = RandomVector() * outerradius ;
        target = -locpos ;
        velocity= - locpos ;
    }
	void Update () {
        if (!returning && locpos.magnitude > outerradius)
        {
            target = RandomVector() * innerradius;
            returning = true;
        }
        else if(returning && locpos.magnitude < innerradius)
        {
            returning = false;
        }
        if (returning)
        {
            velocity += (target - locpos).normalized * pullforce * Time.deltaTime* Random.Range(0.5f,1); 
        }
        locpos += speed * Time.deltaTime * velocity;
        transform.position = pos + locpos;
	}
    Vector3 RandomVector() {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
    }
}
