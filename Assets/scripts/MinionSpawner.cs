using UnityEngine;
using System.Collections;

public class MinionSpawner : RadialPosition
{

    public float leftMinionsPerMinute;
    public float rightMinionsPerMinute;
    private float nextLeft;
    private float nextRight;
    public Minion melee;

    protected override void Update()
    {
        base.Update();

        Spawn(ref nextLeft, leftMinionsPerMinute, true);
        Spawn(ref nextRight, rightMinionsPerMinute, false);
    }
    public void Spawn(ref float timer,  float rate, bool toLeft) {
        timer -= Time.deltaTime;
        if (timer < 0) {
            timer += 60 / rate;
            Minion.Spawn(melee, toLeft, Position);
        }
    }
}