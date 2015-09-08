using UnityEngine;
using System.Collections;

public class RadialMovementInput : RadialMovement {

    public float zoomSpeed;
    public float minFOV;
    public float maxFOV;
    public string leftButton;
    public string rightButton;
    public string zoomInButton;
    public string zoomOutButton;
    public bool inUse;
    public Camera cam;
    private float zoomVal;
    protected override void Update() {
        base.Update();
        if (inUse) {
            int i = 0;
            if (Input.GetButton(leftButton)) i--;
            if (Input.GetButton(rightButton)) i++;
            if (i > 0) { Move(false); }
            else if (i < 0) { Move(true); }

            i = 0;
            if (Input.GetButton(zoomOutButton)) i--;
            if (Input.GetButton(zoomInButton)) i++;
            if (i > 0) { Zoom(false); }
            else if (i < 0) { Zoom(true); }
        }
        cam.enabled = inUse;
        cam.fieldOfView = Mathf.Lerp(minFOV, maxFOV, zoomVal);
    }

    public void Zoom(bool zoomOut) {
        zoomVal = Mathf.Clamp(zoomVal + ((zoomOut)?1:-1) * zoomSpeed * Time.deltaTime, 0, 1);

        
    }

}
