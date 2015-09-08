using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

    public static void Build(House prefab, Base team) {
        House h = Instantiate(prefab);
        h.teamBase = team;
        h.directionIsToLeft = team.isLeftTheWeakest;
    }

    public float minionsPerMinute;
    public int upgradesLeft;
    public bool directionIsToLeft;
    public Base teamBase;
    public Minion.Type type;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
