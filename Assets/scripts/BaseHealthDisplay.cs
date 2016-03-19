using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseHealthDisplay : MonoBehaviour {
    public Base team;
    public Image bar;
    public bool left;
    float progress;
    float visualProgress;
    public float tweenSpeed = 0.1f;
    public void setProgress(float p) {
        bar.fillAmount = p;
        float w = GetComponent<RectTransform>().rect.width;
        float offset = w * p /2 ;
        bar.GetComponent<RectTransform>().localPosition = new Vector3( -w/2 + ( (left)? - offset:  offset) , 0, 0);
    }
	void Update () {
        progress = team.Health / (team.MaxHealth * 1f) ;
        visualProgress = Mathf.Lerp(visualProgress, progress, tweenSpeed);
        setProgress(visualProgress);
    }
}
