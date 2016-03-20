using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public static void Spawn(Projectile prefab, Vector3 pos, Vector2 force, Transform target)
    {
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
    bool locked;
    float lockRange;
    // Update is called once per frame

    void FixedUpdate()
    {
        timeToLive -= Time.fixedDeltaTime;
        if (timeToLive <= 0 || target == null || target.GetComponent<Collider2D>() == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 dir = ((Vector2)(target.position - transform.position));
        if (!locked)
        {
            if (dir.magnitude < lockRange)
            {
                locked = true;
                steerForce = rb.velocity.magnitude;
            }
            float scale = dir.magnitude;
            dir = dir.normalized * steerForce;
            rb.AddForce(dir * Time.fixedDeltaTime / scale);
        }
        else
        {
            rb.velocity = dir.normalized * steerForce;
        }
    }
    void start()
    {
        lockRange = ((Vector2)(target.position - transform.position)).magnitude /2;
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Attackable a = coll.gameObject.GetComponent<Attackable>();
        if (a != null)
        {
            a.Damage(this);
            Destroy(gameObject);
            AudioPlay.PlaySound(hitSound, 0.05f);
        }
    }
}
