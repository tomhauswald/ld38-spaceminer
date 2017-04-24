using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetaryObject))]
public class UFO : MonoBehaviour {

    private enum State {
        Approaching,
        StartingBeam,
        Active,
        CaughtPlayer
    };

    public float MovementSpeed = 7.0f;
    public float Acceleration = 0.05f;
    private PlanetaryObject planetary;

    private State state;

    private float targetSurfaceDistance;
    public float SpawnSurfaceDistance = 2.0f;
    public float LandingSpeed = 1.0f;

    private float beamCylinderTargetScale;
    private Transform beamCylinder;

    private AudioSource audioSource;
    public AudioClip CatchSound;

	// Use this for initialization
	void Start () {
        audioSource = FindObjectOfType<AudioSource>();

        planetary = GetComponent<PlanetaryObject>();
        planetary.Yaw = Random.Range(-Mathf.PI, Mathf.PI);
        targetSurfaceDistance = planetary.SurfaceDistance;
        planetary.SurfaceDistance = SpawnSurfaceDistance;
        planetary.SetPosition(Random.Range(-400, 400), Random.Range(-200, 200));

        // Shrink cylinder and make it transparent.
        beamCylinder = transform.Find("Cylinder");
        beamCylinderTargetScale = beamCylinder.localScale.y;
        beamCylinder.localScale = new Vector3(
            beamCylinder.localScale.x, 
            0, 
            beamCylinder.localScale.z
        );
        beamCylinder.position = transform.position;

        // Turn off lights.
        foreach (var light in GetComponentsInChildren<Light>()) {
            light.enabled = false;
        }

        state = State.Approaching;
    }
	
	// Update is called once per frame
	void Update () {
        switch (state) {
            case State.Approaching:
                planetary.SurfaceDistance -= Time.deltaTime * LandingSpeed;
                if (planetary.SurfaceDistance <= targetSurfaceDistance) {
                    planetary.SurfaceDistance = targetSurfaceDistance;
                    state = State.StartingBeam;
                }
                break;

            case State.StartingBeam:
                beamCylinder.localScale += Vector3.up * Time.deltaTime;
                beamCylinder.localPosition -= Vector3.up * Time.deltaTime;
                if (beamCylinder.localScale.y >= beamCylinderTargetScale) {
                    state = State.Active;
                }
                break;

            case State.Active:
                MovementSpeed += Acceleration * Time.deltaTime;
                planetary.Move(MovementSpeed * Time.deltaTime);
                break;

            case State.CaughtPlayer:
                break;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            audioSource.PlayOneShot(CatchSound);
            foreach (var ufo in FindObjectsOfType<UFO>()) {
                ufo.state = State.CaughtPlayer;
            }
            other.gameObject.GetComponent<Player>().OnCaught(this);
        }
    }
}