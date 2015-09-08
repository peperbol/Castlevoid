using UnityEngine;
using System.Collections;
using System;

public abstract class Minion : RadialMovement, Attackable
{
    public static void Spawn(Minion prefab, bool toLeft, float pos)
    {
        Minion m = Instantiate(prefab);
        m.center = GameObject.FindGameObjectWithTag("Center").transform;
        m.DirectionIsToLeft = toLeft;
        m.Position = pos;
    }

    public int health;
    public Animator animator;

    public int Health {
        get { return health; }
        set
        { health = value;

            if (health <= 0) {
                //DIEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
                StartCoroutine(Die());
            }
       }
    }
    protected abstract IEnumerator Die();
    public float sight;
    public LayerMask enemyMask;
    private bool directionIsToLeft;
    public bool DirectionIsToLeft {
        get { return directionIsToLeft; }
        set { 
            if(directionIsToLeft != value)
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y,transform.localScale.z);
            directionIsToLeft = value;
        }
    }
    private Vector2 V3toV2(Vector3 v) {
        return new Vector2(v.x, v.y);
    }
    protected bool CanSeeEnemy(out Attackable a, out GameObject go)
    {
        a = null;
        go = null;
        RaycastHit2D h = Physics2D.Raycast(transform.position, (DirectionIsToLeft) ? transform.up : -transform.up, sight, enemyMask);

        if (h.collider == null) return false;

        a = h.collider.GetComponent<Attackable>();
        go = h.collider.gameObject;
        return a != null;
    }

    protected abstract void Attack(GameObject go, Attackable a);

    protected override void Update() {
        base.Update();
        Attackable a;
        GameObject go;
        if (CanSeeEnemy(out a, out go)) {
            Attack(go, a);
        } else
        Move(DirectionIsToLeft);
    }

    public virtual void Damage(MonoBehaviour damager) {
        Health--;
    }
    public enum Type { Melee, Ranged, Shield, All}
}
