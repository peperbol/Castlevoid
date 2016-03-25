using UnityEngine;
using System.Collections;

public class ExplosionEffects : MonoBehaviour
{
    public float ttl;
    public bool autodestroy;
    [Header("Screenshake")]
    public float distance;
    public float duration;
    public float speed;
    public float delay;
    [Range(0,1)]
    public float falloff;

    public void OnEnable()
    {
        GameObject.FindObjectOfType<Screenshake>().Shake(distance, duration, speed,delay,falloff);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (autodestroy)
        {
            ttl -= Time.deltaTime;
            if (ttl < 0) { Destroy(gameObject); }
        }
    }
}
