using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlanetaryObject))]
public class Pickup : MonoBehaviour {

    private Action<Pickup> pickupAction;

    private PlanetaryObject planetary;

    public Pickup(Action<Pickup> pickupAction) {
        this.pickupAction = pickupAction;
    }

    private void Start() {
        planetary = GetComponent<PlanetaryObject>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (pickupAction != null) {
                pickupAction.Invoke(this);
            }
            Destroy(gameObject);
        }
    }
}
