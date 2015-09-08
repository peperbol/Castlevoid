using UnityEngine;
using System.Collections;

public class MinionSpawner : RadialPosition
{
    
    private float nextLeft = 1;
    private float nextRight = 1;
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
    public void Spawn(ref float timer,  bool toLeft)
    {
        float m = team.GetMinionsPerMinute((toLeft) ? -1 : 1);
        timer -= Time.deltaTime * m;
        if (timer <= 0) {
            timer += 60 ;

            float r = Random.Range(0, m);
            float meleec = team.GetMinionsPerMinute((toLeft) ? -1 : 1, Minion.Type.Melee);
            float shieldc = team.GetMinionsPerMinute((toLeft) ? -1 : 1, Minion.Type.Shield);
            Minion.Spawn(((r < meleec) ? melee :((r < meleec +shieldc)?shield:ranged)), toLeft, Position);
        }
    }
}