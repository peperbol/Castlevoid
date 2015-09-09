using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingCounter : MonoBehaviour {
    
    public Base team;
    public Minion.Type type;
    public Text t;

	void Update () {
        t.text = team.GetHousesCount(type) + "";
    }
}
