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

    public int MaxHealth;
    private int health;
    public float timeToDie;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0)
            {
                StartCoroutine(Destroy());
            }
        }
    }
    public float buildDepth;
    public GameObject endOverlay;
    public GameObject endMenu;
    IEnumerator Destroy()
    {
        
        float time = timeToDie;
        Vector3 pos = transform.GetChild(0).position;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.GetChild(0).position = Vector3.Lerp(pos - transform.right * buildDepth , pos, time / timeToDie);
            yield return null;
        }
        endOverlay.SetActive(true);
        Builder[] b = FindObjectsOfType<Builder>();
        for (int i = 0; i < b.Length; i++)
        {
            b[i].CanMove = false;
        }
        endMenu.SetActive(true);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Minion m = c.GetComponent<Minion>();
        if (m != null)
        {
            Health--;
            Destroy(m.gameObject);
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
    void Start() {
        Health = MaxHealth;
        Debug.Log(KeyCode.Slash.ToString());
        Debug.Log(KeyCode.Greater.ToString());
        Debug.Log(KeyCode.Less.ToString());
    }

}
