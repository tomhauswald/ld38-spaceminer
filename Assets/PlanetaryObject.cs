using UnityEngine;
using System.Collections;

public class PlanetaryObject : MonoBehaviour {

    public Transform Planet;

    private float yaw;
    public float Yaw { get { return yaw; } set { yaw = value; UpdateDirection(); } }
    
    public float SurfaceDistance = 0.0f;

    private Vector3 direction;
    private Quaternion orientation;
    private float radius;

    private void OnEnable() {
        direction = Vector3.zero;
        orientation = Quaternion.identity;
        radius = Planet.GetComponent<SphereCollider>().radius;
    }

    void Update() {
        transform.localPosition = orientation * Vector3.forward * radius;
        transform.localPosition += transform.up * SurfaceDistance;
        transform.localRotation = orientation * Quaternion.LookRotation(direction, Vector3.forward);
    }

    public void Strafe(float amount) {
        orientation *= Quaternion.AngleAxis(amount, direction);
        Update();
    }

    public void Move(float amount) {
        var sideways = new Vector3(-direction.y, direction.x, 0);
        orientation *= Quaternion.AngleAxis(amount, sideways);
        Update();
    }

    public void SetPosition(float horizontal, float vertical) {
        var prevYaw = Yaw;

        orientation = Quaternion.identity;
        Yaw = 0.0f;
        Update();

        Move(vertical);
        Strafe(horizontal);
        Yaw = prevYaw;
    }

    public void SetRandomPosition() {
        var prevYaw = Yaw;
        orientation = Quaternion.identity;
        Yaw = Random.Range(-Mathf.PI, Mathf.PI);
        Move(Random.Range(0, 360));
        Yaw = prevYaw;
    }

    private void UpdateDirection() {
        direction = new Vector3(Mathf.Sin(Yaw), Mathf.Cos(Yaw), 0).normalized;
    }
}
