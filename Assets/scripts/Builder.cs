using UnityEngine;
using System.Collections;
using System;

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

    protected void tryAttack() {
        if (ready) StartCoroutine(Attack());
    }

    protected bool CanHitEnemy(out Attackable a)
    {
        a = null;
        RaycastHit2D h = Physics2D.Raycast(transform.position, (DirectionIsToLeft) ? transform.up : -transform.up, attackRange, attackMask);

        if (h.collider == null) return false;

        a = h.collider.GetComponent<Attackable>();
        return a != null;
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
        yield return new WaitForSeconds(attackTime/2);
        Attackable a;

        if (CanHitEnemy(out a)) a.Damage(this);
        yield return new WaitForSeconds(attackTime / 2);
        ready = true;
    }

    protected override void Update()
    {
        base.Update();
        if (inUse && !Dead)
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
                    animator.SetTrigger("Build");
                    House.Build(housePrefab, team, Position);
                    Preview = Minion.Type.None;
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

                    animator.SetTrigger("Build");
                    House.Build(wallPrefab, team, Position);
                    Preview = Minion.Type.None;
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
                    animator.SetTrigger("Build");
                    House.Build(archerPrefab, team, Position);
                    Preview = Minion.Type.None;
                }
            }
            if (Input.GetButton(attack))
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
        animator.SetBool("Walking", speed > 0.05f);
    }

    public void Damage(MonoBehaviour damager)
    {
        Health--;
    }
    void Start() {
        Health = startHealth;
    }
}
