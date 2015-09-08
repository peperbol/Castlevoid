using UnityEngine;
using System.Collections;
using System;

public class ShieldMinion : Minion {
    protected override void Attack(GameObject go, Attackable a)
    {
        
    }
    public override void Damage(MonoBehaviour damager)
    {
        if (damager is Projectile) return;
        Health--;
    }

    protected override IEnumerator Die()
    {
        throw new NotImplementedException();
    }
}
