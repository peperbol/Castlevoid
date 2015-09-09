using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MinionsCounter : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
    public Base team;
    public Text t;
	protected int AmountOfMinions
    {
        get { return new List<Minion>(FindObjectsOfType<Minion>()).FindAll(e => (e.gameObject.layer == ((team.isLight) ? 8 : 9))&& !e.dead ).Count; }
    }

	void Update () {
        t.text = AmountOfMinions +"";
	}
}
