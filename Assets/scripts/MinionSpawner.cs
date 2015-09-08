using UnityEngine;
using System.Collections;

public class MinionSpawner : RadialPosition
{
    
    private float nextLeft;
    private float nextRight;
    public Minion melee;
    public Minion ranged;
    public Minion shield;
    public Base team;

    protected override void Update()
    {
        base.Update();

        Spawn(ref nextLeft,  true);
        Spawn(ref nextRight,  false);
    }
    public void Spawn(ref float timer,  bool toLeft) {
        timer -= Time.deltaTime;
        if (timer < 0) {
            float m = team.GetMinionsPerMinute((toLeft) ? -1 : 1);
            timer += 60 / m;

            Random.Range(0, m);
            float meleec = team.GetMinionsPerMinute((toLeft) ? -1 : 1, Minion.Type.Melee);
            float shieldc = team.GetMinionsPerMinute((toLeft) ? -1 : 1, Minion.Type.Shield);
            Minion.Spawn(((m < meleec) ? melee :((m < meleec +shieldc)?shield:ranged)), toLeft, Position);
        }
    }
}