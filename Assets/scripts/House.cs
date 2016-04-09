using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class House : RadialPosition, Attackable, Freezable
{

    public static void Build(House prefab, Base team, float position)
    {
        House h = Instantiate(prefab);
        SceneManager.MoveGameObjectToScene(h.gameObject, SceneManager.GetSceneByName("gameplay"));
        h.center = GameObject.FindGameObjectWithTag("Center").transform;
        
        h.Position = position;
        h.teamBase = team;
        h.DirectionIsToLeft = !(team.isLight) == (position < 270 && position > 90);

    }
    public int resourcesCost;
    public AudioClip buildSound;
    public int health;
    public Renderer[] visuals;
    public bool Frozen { get { return frozen; } set { frozen = value; } }
    private bool frozen = false;
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

    public virtual bool Attackable
    {
        get
        {
            return true;
        }
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
        if (!Application.isPlaying) return;
        for (int i = 0; i < visuals.Length; i++)
        {
            mats.Add(visuals[i].materials);
        }
        costDisplay.text = "- " + resourcesCost; 
        StartCoroutine(Building());
    }
    public float buildTime;
    public float buildDepht;
    public Text costDisplay;
    IEnumerator Building() {
        AudioPlay.PlaySound(buildSound,0.5f);
        float time = buildTime;
        Vector3 pos = transform.GetChild(0).position;
        Color textColor = costDisplay.color;
        if (!directionIsToLeft)
        {
            Vector3 s = costDisplay.transform.localScale;
            s.x = -s.x;
            costDisplay.transform.localScale = s;
        }
        while (time>0)
        {
            time -= Time.deltaTime;
            textColor.a = Mathf.Max(2 * time / buildTime,1);
            costDisplay.color = textColor;
            transform.GetChild(0).position = Vector3.Lerp(pos , pos - transform.right * buildDepht, time / buildTime);
            yield return null;
        }
        completed = true;
        Destroy(costDisplay.gameObject);
        teamBase.houses.Add(this);
    }

    IEnumerator Destroy() {
        completed = false;

        AudioPlay.PlaySound(buildSound);
        float time = buildTime;
        Vector3 pos = transform.GetChild(0).position;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.GetChild(0).position = Vector3.Lerp(pos - transform.right * buildDepht, pos , time / buildTime);
            yield return null;
        }
        Destroy(gameObject);
        teamBase.houses.Remove(this);
    }

    public void Spawn()
    {
        if (frozen) return;
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
