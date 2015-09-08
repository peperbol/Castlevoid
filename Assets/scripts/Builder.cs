using UnityEngine;
using System.Collections;

public class Builder : RadialMovementInput
{
    public string Special1;
    public string Special2;
    public string Special3;
    public string Build;
    public string attack;
    public House housePrefab;
    public House wallPrefab;
    public House archerPrefab;
    public Base team;
    public LayerMask BuildObstruction;
    public float buildSpace;
    private Minion.Type preview = Minion.Type.None;

    protected Minion.Type Preview
    {
        get { return preview; }
        set
        {
            switch (value)
            {
                case Minion.Type.Melee:
                    CurrentPreview = buildHouseVisual;
                    break;
                case Minion.Type.Ranged:
                    CurrentPreview = buildArcherVisual;
                    break;
                case Minion.Type.Shield:
                    CurrentPreview = buildWallVisual;
                    break;
                case Minion.Type.None:
                    CurrentPreview = null;
                    break;
            }
            preview = value;
        }
    }

    public Renderer[] buildHouseVisual;
    public Renderer[] buildWallVisual;
    public Renderer[] buildArcherVisual;
    private Renderer[] buildCurrentVisual;

    protected Renderer[] CurrentPreview
    {
        get { return buildCurrentVisual; }
        set
        {
            if (buildCurrentVisual != null)
            {

                for (int i = 0; i < buildCurrentVisual.Length; i++)
                {

                    buildCurrentVisual[i].enabled = false;
                }
                
            }
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {

                    value[i].enabled = true;
                }
            }
            Debug.Log(2);
            buildCurrentVisual = value;
        }
    }
    public Material buildPositive;
    public Material buildNegative;

    protected bool CanBuild()
    {
        return Physics2D.OverlapCircle(transform.position, buildSpace, BuildObstruction) == null;
    }
    protected override void Update()
    {
        base.Update();
        if (inUse)
        {
            /*
            //Debug.Log(Input.GetAxis(Build));
            if (Input.GetAxis(Build) > deadZone)
            {
                BuildMode = true;
            }*/

            if (Input.GetButtonDown(Special1))
            {
                if (Preview != Minion.Type.Melee)
                {

                    Preview = Minion.Type.Melee;
                }
                else if (CanBuild())
                {
                    House.Build(housePrefab, team, Position);
                    Preview = Minion.Type.None;
                }

            }
            else if (Input.GetButtonDown(Special2))
            {
                if (Preview != Minion.Type.Shield)
                {
                    Preview = Minion.Type.Shield;
                }
                else if (CanBuild())
                {
                    House.Build(wallPrefab, team, Position);
                    Preview = Minion.Type.None;
                }
            }
            else if (Input.GetButtonDown(Special3))
            {
                if (Preview != Minion.Type.Ranged)
                {
                    Preview = Minion.Type.Ranged;
                }
                else if (CanBuild())
                {
                    House.Build(archerPrefab, team, Position);
                    Preview = Minion.Type.None;
                }
            }
            if (Input.GetButtonDown(attack))
            {
                Preview = Minion.Type.None;
            }
        }


        if (CurrentPreview != null)
        {
            for (int i = 0; i < CurrentPreview.Length; i++)
            {
                CurrentPreview[i].material = (CanBuild()) ? buildPositive : buildNegative;
            }
        }
    }
}
