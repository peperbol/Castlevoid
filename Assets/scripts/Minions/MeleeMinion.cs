﻿using UnityEngine;
using System.Collections;
using System;

public class MeleeMinion : Minion
{
    public float attackReach;
    public float timePerAttack;
    private bool ready = true;
    bool attacking;
    public AudioClip attackSound;
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

        if (ready)
        {
            StartCoroutine(Wait(a));
  
        }
    }

    IEnumerator Wait(Attackable a)
    {
        Attacking = true;
        ready = false;
        yield return new WaitForSeconds(timePerAttack / 2 - 0.02f);
        GameObject go;
        if (!dead && CanSeeEnemy(out a,out go, attackReach))
        {
            yield return new WaitForSeconds( 0.02f);
            if (a != null)
            {
                AudioPlay.PlaySound(attackSound);
                a.Damage(this);
            }
        }
        Attacking = false;
        yield return new WaitForSeconds(timePerAttack / 2);
        ready = true;
    }

}