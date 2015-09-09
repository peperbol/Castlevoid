using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class House : RadialPosition, Attackable
{

    public static void Build(House prefab, Base team, float position)
    {
        House h = Instantiate(prefab);
        h.center = GameObject.FindGameObjectWithTag("Center").transform;
        
        h.Position = position;
        h.teamBase = team;
        h.DirectionIsToLeft = !(team.isLight) == (position < 270 && position > 90);
        team.houses.Add(h);

    }
    public int health;
    public Renderer[] visuals;
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
    public bool DirectionIsToLeft
    {
        get { return directionIsToLeft; }
        set
        {

            if (directionIsToLeft != value)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            directionIsToLeft = value;

        }
    }
    public Base teamBase;
    public Minion.Type type;
    public bool completed = false;
    private float timer = 0.1f;
    public void OnDestroy()
    {
        if(teamBase != null)
        teamBase.houses.Remove(this);
    }

    public Material flash;
    public virtual void Damage(MonoBehaviour damager)
    {
        Health--;
        StartCoroutine(DmgFlash());
    }
    List<Material[]> mats = new List<Material[]>();
    public IEnumerator DmgFlash()
    {
        for (int i = 0; i < visuals.Length; i++)
        {
            Material[] m = visuals[i].materials;

            for (int j = 0; j < m.Length; j++)
            {
                m[j] = flash;
            }
            visuals[i].materials = m;
        }
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < visuals.Length; i++)
        {

            visuals[i].materials = mats[i];
        }
    }

    protected virtual void Start() {

        for (int i = 0; i < visuals.Length; i++)
        {
            mats.Add(visuals[i].materials);
        }
        StartCoroutine(Building());
    }

    public float buildTime;
    public float buildDepht;
    IEnumerator Building() {
        float time = buildTime;
         Vector3 pos = transform.GetChild(0).position;
        while (time>0)
        {
            time -= Time.deltaTime;
            transform.GetChild(0).position = Vector3.Lerp(pos , pos - transform.right * buildDepht, time / buildTime);
            yield return null;
        }
        completed = true;
    }

    public void Spawn()
    {
        timer -= Time.deltaTime * MinionsPerMinute;
        if (timer <= 0)
        {
            timer += 60;

            Minion m = null;
            switch (type)
            {
                case Minion.Type.Melee:
                    m = teamBase.melee;
                    break;
                case Minion.Type.Ranged:
                    m = teamBase.ranged;
                    break;
                case Minion.Type.Shield:
                    m = teamBase.shield;
                    break;
            }
            Minion.Spawn(m, DirectionIsToLeft, Position);
        }
    }
    protected override void Update()
    {
        base.Update();
        Spawn();
    }
}
