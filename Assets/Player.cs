using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlanetaryObject))]
public class Player : MonoBehaviour {

    public enum PlayerState {
        Spawning,
        Active,
        Caught
    };

    public PlayerState State;
    public float MovementSpeed = 10.0f;
    public float RotationSpeed = 4 * Mathf.PI;
    public float HeadlightThreshold = 0.1f;
    public Transform Sun;
    public Light[] Headlights;
    public Transform[] FrontTires;
    public AudioClip ExplosionSound;

    private PlanetaryObject planetary;
    private float targetSurfaceDistance;
    private Vector3 caughtPosition;
    private float caughtTime;
    private UFO ufo;

    private void Start () {
        State = PlayerState.Spawning;
        planetary = GetComponent<PlanetaryObject>();
        planetary.Yaw = 0.0f;
        targetSurfaceDistance = planetary.SurfaceDistance;
        planetary.SurfaceDistance = 1.0f;
    }

    private void UpdateMovement() {
        planetary.Yaw -= Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
        planetary.Move(Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime);

        foreach (var tyre in FrontTires) {
            var euler = tyre.localEulerAngles;
            euler.y = 20.0f * Mathf.Sin(Input.GetAxis("Horizontal"));
            tyre.localEulerAngles = euler;
        }

        foreach (var light in Headlights) {
            light.enabled = Vector3.Dot(
                transform.up,
                (Sun.position - transform.position).normalized
            ) <= HeadlightThreshold;
        }
    }

    public void OnCaught(UFO ufo) {
        caughtPosition = transform.position;
        State = PlayerState.Caught;
        this.ufo = ufo;
        planetary.enabled = false;
        FindObjectOfType<OverheadCamera>().enabled = false;
    }

    void Update() {
        switch (State) {
            case PlayerState.Spawning:
                if ((planetary.SurfaceDistance -= 0.25f * Time.deltaTime) <= targetSurfaceDistance) {
                    planetary.SurfaceDistance = targetSurfaceDistance;
                    State = PlayerState.Active;
                }
                break;

            case PlayerState.Active:
                UpdateMovement();
                break;

            case PlayerState.Caught:
                transform.localRotation *= Quaternion.AngleAxis(360.0f * Time.deltaTime, Vector3.up);
                transform.position = Vector3.Lerp(
                    caughtPosition,
                    ufo.transform.position,
                    (caughtTime += Time.deltaTime) / 5.0f
                );
                if(caughtTime >= 5.0f) {
                    FindObjectOfType<AudioSource>().PlayOneShot(ExplosionSound);
                    Destroy(gameObject);

                    // Store score in prefs.
                    var score = (int) FindObjectOfType<Game>().PlayerScore;
                    PlayerPrefs.SetInt("Latest", score);
                    if(!PlayerPrefs.HasKey("Best") || score > PlayerPrefs.GetInt("Best")) {
                        PlayerPrefs.SetInt("Best", score);
                    }
                    PlayerPrefs.Save();

                    // Load gameover scene.
                    SceneManager.LoadScene("Gameover");
                }
                break;
        }
    }
}