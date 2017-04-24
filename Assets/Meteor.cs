using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetaryObject))]
public class Meteor : MonoBehaviour {

    private PlanetaryObject planetary;

	// Use this for initialization
	void Start () {
        planetary = GetComponent<PlanetaryObject>();
	}
	
	// Update is called once per frame
	void Update () {
        planetary.SurfaceDistance -= 0.1f * Time.deltaTime;
	}
}
