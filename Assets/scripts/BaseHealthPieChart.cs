using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BaseHealthPieChart : MonoBehaviour {

    public Base team1;
    public Base team2;
    public Material m;
    float val = 0.5f;
    public bool reset;
    float target {
        get
        {
            return (reset)? 0.5f : team1.Health / (float)(team1.Health + team2.Health);
        }
    } 

    public float tweenSpeed = 0.1f;

    void Start()
    {
       // i = GetComponent<Image>();
        val = (Time.time < 0.1)? 0.5f : m.GetFloat("_Split") ;
    }

    void Update () {
        val = Mathf.Lerp(val, target, tweenSpeed);
        m.SetFloat("_Split", val);
    }
}
