using UnityEngine;
using System.Collections;

public class RadialMovement : RadialPosition {
    public float force = 1;
    [Range(0,1)]
    public float drag ;
    protected float speed;
    public void Move(bool toLeft) {
        speed += ((toLeft)?1:-1) * force;
    }

    protected override void Update()
    {
        base.Update();
        speed *= 1 - drag;
        Position += speed * Time.deltaTime;
    }

}
