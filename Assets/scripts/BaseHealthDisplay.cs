using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseHealthDisplay : MonoBehaviour {
    public Base team;
    public Image bar;
    public bool left;

    public void setProgress(float p) {
        bar.fillAmount = p;
        float offset =
        GetComponent<RectTransform>().rect.width * (1- p);
        bar.GetComponent<RectTransform>().localPosition = new Vector3((left) ? -offset: offset, 0, 0);
    }
	void Update () {
        setProgress(team.Health / (team.MaxHealth * 1f) );
    }
}
