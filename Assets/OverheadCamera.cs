using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCamera : MonoBehaviour {

    public Transform Target;
    public float Height;
    public float Distance;

	void Start () {
    }

	void Update () {
        if (Target != null) {
            var point = Target.position + Height * Target.up - Distance * Target.forward;
            transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime);

            /* Sadly produces weird jitter:
                transform.position = Vector3.SmoothDamp(
                transform.position,
                
                ref smoothDampVelocity,
                0.3f
            );
            */

            transform.LookAt(Target.position, Target.forward);
        }
	}
}
