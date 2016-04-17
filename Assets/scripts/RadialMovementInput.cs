using UnityEngine;
using System.Collections;

public class RadialMovementInput : RadialMovement, Freezable
{

    public float zoomSpeed;
    public float minFOV;
    public float maxFOV;
    public string horizontalButton;
    public string verticalButton;
    [Range(0,1)]
    public float deadZone = 0.2f;

    public bool Frozen { get { return frozen; } set { frozen = value; } }
    private bool frozen = false;
    
    protected bool directionIsToLeft;
    public bool CanMove {
        get { return canMoveLevel <= 0; }
        set { canMoveLevel += (value)?-1:1; }
    }
    private int canMoveLevel;

    public virtual bool DirectionIsToLeft
    {
        get { return directionIsToLeft; }
        set
        {
            directionIsToLeft = value;
        }
    }

    protected override void Update() {
        base.Update();
        if (!Frozen) {
            float i = Input.GetAxis(horizontalButton);
            if (CanMove && Mathf.Abs(i) > deadZone)
            {
                if (i > 0) { Move(false); DirectionIsToLeft = false; }
                else if (i < 0) { Move(true); DirectionIsToLeft = true; }
            }
        }
    }

}
