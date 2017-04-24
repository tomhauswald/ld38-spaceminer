using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetaryObject))]
public class SunMovement : MonoBehaviour {

    private PlanetaryObject planetary;

	// Use this for initialization
	void Start () {
        planetary = GetComponent<PlanetaryObject>();
        planetary.Yaw = 0.0f;
    }
	
	// Update is called once per frame
	void Update () {
        planetary.Move(5*Time.deltaTime);
    }
}
