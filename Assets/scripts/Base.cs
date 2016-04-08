using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public List<House> houses = new List<House>();
    public Minion melee;
    public Minion ranged;
    public Minion shield;
    public bool isLight;
    public Base enemy;

    public int MaxHealth;
    private int health;
    public float timeToDie;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0 )
            {
                StartCoroutine(Destroy());
            }
        }
    }
    public float buildDepth;
    public GameObject explosion;

    public IEnumerator Undestroy()
    {
        explosion.SetActive(false);
        float time = timeToDie;
        Vector3 pos = transform.GetChild(0).position;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.GetChild(0).position = Vector3.Lerp(pos + transform.right * buildDepth, pos, time / timeToDie);
            yield return null;
        }

    }
    IEnumerator Destroy()
    {
        if (explosion.activeSelf) yield break;
        float time = timeToDie;
        Vector3 pos = transform.GetChild(0).position;
        explosion.SetActive(true);
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.GetChild(0).position = Vector3.Lerp(pos - transform.right * buildDepth, pos, time / timeToDie);
            yield return null;
        }
        Menu.GameOver(this);
        yield break;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Minion m = c.GetComponent<Minion>();
        if (m != null)
        {
            Health--;
            enemy.Health++;
            Destroy(m.gameObject);
        }
        Builder b = c.GetComponent<Builder>();
        if (b != null && b.team == this)
        {
            healTimer = TimePerHeath;
            b.isInBase = true;
            Debug.Log("in");
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        Builder b = c.GetComponent<Builder>();
        if (b != null && b.team == this)
        {
            b.isInBase = false;
            Debug.Log("out");
        }
    }
    public float TimePerHeath;
    private float healTimer;
    public void OnTriggerStay2D(Collider2D c)
    {
        Builder b = c.GetComponent<Builder>();
        if (b != null && b.team == this && b.Health < b.startHealth)
        {
            healTimer -= Time.deltaTime;
            if (healTimer < 0)
            {
                healTimer += TimePerHeath;
                b.Health++;
            }
        }
    }

    public int GetHousesCount(Minion.Type t = Minion.Type.None)
    {

        List<House> h = houses;


        switch (t)
        {
            case Minion.Type.Melee:
                h = h.FindAll(e => e.type == Minion.Type.Melee);
                break;
            case Minion.Type.Ranged:
                h = h.FindAll(e => e.type == Minion.Type.Ranged);
                break;
            case Minion.Type.Shield:
                h = h.FindAll(e => e.type == Minion.Type.Shield);
                break;
        }
        return h.Count;
    }
    void Start()
    {
        transform.GetChild(0).position -= transform.right * buildDepth;
        StartCoroutine(Undestroy());
        Health = MaxHealth;
    }

}
