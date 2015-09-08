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
    protected override void Update()
    {
        base.Update();
        if (inUse) {
            if (Input.GetAxis(Build) > deadZone) {
                buildmode = true;
            }
            if (buildmode) {

                if (Input.GetButtonDown(Special1)) {
                    House.Build(housePrefab, team, Position);
                    buildmode = false;
                }
                else if (Input.GetButtonDown(Special2))
                {

                    buildmode = false;
                }
                else if (Input.GetButtonDown(Special3))
                {

                    buildmode = false;
                }
            }

            if (Input.GetButtonDown(attack)) {

            }
        }
    }
}
