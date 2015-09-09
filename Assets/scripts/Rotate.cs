using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public float speed;
	void Update () {
        GetComponent<Rigidbody2D>().angularVelocity = speed;
	}
}
