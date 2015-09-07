using UnityEngine;
using System.Collections;

public class RadialMovementInput : RadialMovement {

    public string leftButton;
    public string rightButton;
    public bool inUse;
    public Camera cam;

    protected override void Update() {
        base.Update();
        if (inUse) {
            int i = 0;
            if (Input.GetButton(leftButton)) i--;
            if (Input.GetButton(rightButton)) i++;
            if (i > 0) { Move(false); }
            else if (i < 0) { Move(true); }
        }
        cam.enabled = inUse;
    }

}
