using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuilderHealthDisplay : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public Image bar;
    public Builder player;
	// Update is called once per frame
	void Update () {
        bar.fillAmount = 1f * player.Health / player.startHealth;
	}
}
