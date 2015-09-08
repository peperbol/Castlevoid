using UnityEngine;
using System.Collections;
using System;

public class ShieldMinion : Minion {
    protected override void Attack(GameObject go, Attackable a)
    {
        
    }
    public override void Damage(MonoBehaviour damager)
    {
        animator.SetTrigger("Dmg");
        if (damager is Projectile) return;
        Health--;
    }
    
}
