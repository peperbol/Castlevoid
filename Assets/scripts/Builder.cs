using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Builder : RadialMovementInput, Attackable
{
    public string Special1;
    public string Special2;
    public string Special3;
    public string Build;
    public string attack;
    public House housePrefab;
    public House wallPrefab;
    public House archerPrefab;
    public Base team;
    public LayerMask BuildObstruction;
    public float buildSpace;
    private Minion.Type preview = Minion.Type.None;

    protected Minion.Type Preview
    {
        get { return preview; }
        set
        {
            switch (value)
            {
                case Minion.Type.Melee:
                    CurrentPreview = buildHouseVisual;
                    break;
                case Minion.Type.Ranged:
                    CurrentPreview = buildArcherVisual;
                    break;
                case Minion.Type.Shield:
                    CurrentPreview = buildWallVisual;
                    break;
                case Minion.Type.None:
                    CurrentPreview = null;
                    break;
            }
            preview = value;
        }
    }

    public Animator animator;

    public Renderer[] buildHouseVisual;
    public Renderer[] buildWallVisual;
    public Renderer[] buildArcherVisual;
    private Renderer[] buildCurrentVisual;

    protected Renderer[] CurrentPreview
    {
        get { return buildCurrentVisual; }
        set
        {
            if (buildCurrentVisual != null)
            {

                for (int i = 0; i < buildCurrentVisual.Length; i++)
                {

                    buildCurrentVisual[i].enabled = false;
                }
                
            }
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {

                    value[i].enabled = true;
                }
            }
            buildCurrentVisual = value;
        }
    }
    public Material buildPositive;
    public Material buildNegative;

    protected bool CanBuild()
    {
        return Physics2D.OverlapCircle(transform.position, buildSpace, BuildObstruction) == null;
    }
    private int health;
    public int startHealth;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            if (health <= 0)
            {
                //DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                StartCoroutine(Die());
            }
        }
    }

    IEnumerator Die()
    {
        Dead = true;
        yield return new WaitForSeconds(deadTime);
        Dead = false;
        Position = (team.isLight) ? 270 : 90;
        Health = startHealth;
    }
    private bool dead;
    private bool Dead
    {
        get { return dead; }
        set {
            GetComponent<Collider2D>().enabled = !value;
            dead = value;
            canMove = !value;
            animator.SetBool("Dead", value);

       }
    }
    public float deadTime = 2.5f;
    private bool ready = true;
    public float attackTime = 1;
    public float attackRange;
    public LayerMask attackMask;
    public float buildTime = 0.7f;

    protected void tryAttack() {
        if (ready) StartCoroutine(Attack());
    }

    protected bool CanHitEnemy(out List<Attackable> a)
    {
        
        a = new List<Attackable>();
        RaycastHit2D[] h = Physics2D.RaycastAll(transform.position, (DirectionIsToLeft) ? transform.up : -transform.up, attackRange, attackMask);

        for (int i = 0; i < h.Length; i++)
        {
            if (h[i].collider.GetComponent<Attackable>() != null)
                a.Add(h[i].collider.GetComponent<Attackable>());
        }

        return a.Count > 0;
    }

    public override bool DirectionIsToLeft
    {
        get { return directionIsToLeft; }
        set
        {
            if (directionIsToLeft != value)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            directionIsToLeft = value;
        }
    }
    IEnumerator Attack() {
        animator.SetTrigger("Attack");
        ready = false;
        yield return new WaitForSeconds(attackTime / 8);
        canMove = false;
        yield return new WaitForSeconds(attackTime/8 *2);

        List<Attackable> a;
        if (CanHitEnemy(out a)) a.ForEach(e => e.Damage(this));
        yield return new WaitForSeconds(attackTime / 8 );
        canMove = true;
        yield return new WaitForSeconds(attackTime / 2);
        ready = true;
    }

    IEnumerator BuildHouse(House prefab)
    {
        canMove = false;
        animator.SetTrigger("Build");
        House.Build(prefab, team, Position);
        Preview = Minion.Type.None;
        yield return new WaitForSeconds(buildTime);
        canMove = true;
    }
    protected override void Update()
    {
        base.Update();
        if (inUse && canMove)
        {
            /*
            //Debug.Log(Input.GetAxis(Build));
            if (Input.GetAxis(Build) > deadZone)
            {
                BuildMode = true;
            }*/

            if (Input.GetButtonDown(Special1))
            {
                if (Preview != Minion.Type.Melee)
                {

                    Preview = Minion.Type.Melee;
                }
                else if (CanBuild())
                {
                    StartCoroutine(BuildHouse(housePrefab));
                }

            }
            else if (Input.GetButtonDown(Special2))
            {
                if (Preview != Minion.Type.Shield)
                {
                    Preview = Minion.Type.Shield;
                }
                else if (CanBuild())
                {

                    StartCoroutine(BuildHouse(wallPrefab));
                }
            }
            else if (Input.GetButtonDown(Special3))
            {
                if (Preview != Minion.Type.Ranged)
                {
                    Preview = Minion.Type.Ranged;
                }
                else if (CanBuild())
                {
                    StartCoroutine(BuildHouse(archerPrefab));
                }
            }
            if (Input.GetButton(attack) )
            {
                Preview = Minion.Type.None;
                tryAttack();
            }
        }


        if (CurrentPreview != null)
        {
            for (int i = 0; i < CurrentPreview.Length; i++)
            {
                Material[] mats = CurrentPreview[i].materials;
                for (int j = 0; j < mats.Length; j++)
                {
                    mats[j] = (CanBuild()) ? buildPositive : buildNegative;
                }
                CurrentPreview[i].materials = mats;
            }
        }
        animator.SetBool("Walking", Mathf.Abs(speed) > 0.05f);
    }

    public Renderer[] visuals;
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
    void Start() {
        Health = startHealth;
        for (int i = 0; i < visuals.Length; i++)
        {
            mats.Add(visuals[i].materials);
        }
    }
}
