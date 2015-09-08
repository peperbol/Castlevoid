using UnityEngine;
using System.Collections;

public class Builder : RadialMovementInput {
    public string Special1;
    public string Special2;
    public string Special3;
    public string Build;
    public string attack;
    public House housePrefab;
    public Base team;
    public bool buildmode;
    public LayerMask BuildObstruction;
    public float buildSpace;
    public bool BuildMode{
        get{ return buildmode; }
        set{
            buildmode = value;

            buildVisual.enabled = value;
        }
    }


    public Renderer buildVisual;
    public Material buildPositive;
    public Material buildNegative;

    protected bool CanBuild() {
        return Physics2D.OverlapCircle(transform.position, buildSpace, BuildObstruction) == null;
    }
    protected override void Update()
    {
        base.Update();
        if (inUse) {
            if (Input.GetAxis(Build) > deadZone) {
                BuildMode = true;
            }
            if (BuildMode) {
                if (CanBuild())
                {
                    if (Input.GetButtonDown(Special1))
                    {
                        House.Build(housePrefab, team, Position);
                        BuildMode = false;
                    }
                    else if (Input.GetButtonDown(Special2))
                    {

                        BuildMode = false;
                    }
                    else if (Input.GetButtonDown(Special3))
                    {

                        BuildMode = false;
                    }
                }
            }

            if (Input.GetButtonDown(attack)) {

            }
        }
        if (buildmode) {
            buildVisual.material = (CanBuild()) ? buildPositive : buildNegative;
        }
    }
}
