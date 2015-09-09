using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public static void Spawn(Projectile prefab, Vector3 pos,Vector2 force, Transform target) {
        Projectile t = Instantiate(prefab);
        t.transform.position = pos;
        t.GetComponent<Rigidbody2D>().AddForce(force);
        t.target = target;
    }

    public float timeToLive = 7;
    public float steerForce = 1;
    public Transform target;
    public AudioClip hitSound;
    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0) Destroy(gameObject);
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 dir = ((Vector2)(target.position - transform.position));
        float d = dir.sqrMagnitude;
        dir = dir.normalized * steerForce;
        rb.AddForce(dir * Time.deltaTime / d );
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
       Attackable a = coll.gameObject.GetComponent<Attackable>();
        if (a != null) {
            a.Damage(this);
            Destroy(gameObject);
            AudioPlay.PlaySound(hitSound, 0.05f);
        }
    }
}
