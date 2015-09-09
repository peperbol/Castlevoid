using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuidlingCounter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public Base team;
    public Minion.Type type;
    public Text t;
	// Update is called once per frame
	void Update () {
        t.text = team.GetHousesCount(type) + "";
	}
}
