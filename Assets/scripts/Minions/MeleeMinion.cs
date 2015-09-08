using UnityEngine;
using System.Collections;
using System;

public class MeleeMinion : Minion {
    public float timePerAttack;
    private bool ready = true;
    bool attacking;
    public bool Attacking {
        get {
            return attacking;
        }
        set {
            attacking = value;
            animator.SetBool("attacking", value);
        }
    }
    protected override void Attack(GameObject go, Attackable a)
    {

        if (ready) {
            StartCoroutine(Wait(a));
        }
    }

    IEnumerator Wait(Attackable a) {
        Attacking = true;
        ready = false;
        yield return new WaitForSeconds(timePerAttack / 2);
        if(a!= null)
        a.Damage(this);
        Attacking = false;
        yield return new WaitForSeconds(timePerAttack/2);
        ready = true;
    }

    protected override IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return null;
    }
}
