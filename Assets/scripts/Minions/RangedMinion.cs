using UnityEngine;
using System.Collections;
using System;

public class RangedMinion : Minion {
    public float timePerAttack;
    private bool ready = true;
    public Projectile projectilePrefab;
    public Vector2 ProjectileForce;
    private Vector2 worldForce { get { return transform.TransformPoint((DirectionIsToLeft) ? new Vector2(ProjectileForce.x, ProjectileForce.y) : ProjectileForce); } }
    protected override void Attack(GameObject go, Attackable a)
    {

        if (ready) {
            Projectile.Spawn(projectilePrefab, transform.position,worldForce, go.transform);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        ready = false;
        yield return new WaitForSeconds(timePerAttack);
        ready = true;
    }
    
}
