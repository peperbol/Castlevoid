using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class RadialPosition : MonoBehaviour {
    public Transform center;
    public float radius;

    [Range(0,360)]
    public float position;

    public float Position {
        get { return position; }
        set {
            position = ( value  +360) %360;
            SetPos();
        }
    }
    public float RadPos {
        get { return Position / 180 * Mathf.PI; }
        set { Position = value * 180 / Mathf.PI; }
    }
    public static float RadialDistance(float p1, float p2) {
        return (Mathf.Abs( p1 - p2)> 180)? 360 - Mathf.Abs(p1 - p2) : Mathf.Abs(p1 - p2);
    }
    public void SetPos() {
        transform.position = new Vector3(Mathf.Cos(RadPos), Mathf.Sin(RadPos), 0)* radius + center.position;

        transform.eulerAngles = new Vector3(0, 0, position);
    }
    protected virtual void Update () {
            SetPos();
            
	}
}
