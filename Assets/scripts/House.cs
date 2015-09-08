using UnityEngine;
using System.Collections;
using System;

public class House : RadialPosition, Attackable
{

    public static void Build(House prefab, Base team, float position)
    {
        House h = Instantiate(prefab);
        h.center = GameObject.FindGameObjectWithTag("Center").transform;
        h.Position = position;
        h.teamBase = team;
        h.directionIsToLeft = team.isLeftTheWeakest;
        team.houses.Add(h);

    }
    public int health;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0)
            {
                //DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                GameObject.Destroy(gameObject);
            }
        }
    }

    public float minionsPerMinute;
    public float MinionsPerMinute {
        get { return (completed) ? minionsPerMinute : 0; }
    }
    public int upgradesLeft;
    public bool directionIsToLeft;
    public Base teamBase;
    public Minion.Type type;
    public bool completed = false;
    public void OnDestroy()
    {
        teamBase.houses.Remove(this);
    }

    public void Damage(MonoBehaviour damager)
    {
        Health--;
    }

    protected virtual void Start() {
        StartCoroutine(Building());
    }
    IEnumerator Building() {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.5f);
            GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            GetComponent<Renderer>().enabled = true;
        }
        completed = true;
    }
}
