using UnityEngine;
using System.Collections;
using System;

public class RangedMinion : Minion {
    public float timePerAttack;
    private bool ready = true;
    public Projectile projectilePrefab;
    public Vector2 ProjectileForce;
    private Vector2 worldForce { get { return transform.TransformPoint((DirectionIsToLeft) ? new Vector2(ProjectileForce.x, ProjectileForce.y) : ProjectileForce); } }

    bool attacking;
    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
            animator.SetBool("attacking", value);
        }
    }

    protected override void Attack(GameObject go, Attackable a)
    {

        if (ready) {
            StartCoroutine(Wait(go));
        }
    }

    IEnumerator Wait(GameObject go) {
        attacking = true;
        ready = false;
        yield return new WaitForSeconds(timePerAttack/2);
        Projectile.Spawn(projectilePrefab, transform.position, worldForce, go.transform);
        attacking = false;
        yield return new WaitForSeconds(timePerAttack/2);
        ready = true;
    }
    
}
