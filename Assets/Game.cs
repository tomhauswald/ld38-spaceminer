using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    private const int width = 72;
    private const int height = 36;
    private const float spacing = 5.0f;

    public Transform Planet;

    public GameObject UfoPrefab;
    public float UfoInterarrivalTime;
    public uint MaxUfoCount;

    public GameObject CrystalPrefab;
    public uint MaxCrystalCount;

    public uint PlayerScore { get; private set; }
    public Text PlayerScoreText;

    private float lastUfoSpawnTime;
    private uint ufoCount;    

    // Use this for initialization
    void Start() {
        PlayerScore = 0;
        lastUfoSpawnTime = 0.0f;
        ufoCount = 0;

        UfoPrefab.GetComponent<PlanetaryObject>().Planet = Planet;
        CrystalPrefab.GetComponent<PlanetaryObject>().Planet = Planet;

        for (var i = 0; i < MaxCrystalCount; ++i) {
            SpawnCrystal();
        }

        if (MaxUfoCount > 0) {
            SpawnUFO();
        }
    }

    // Update is called once per frame
    void Update() {
        if (ufoCount < MaxUfoCount && Time.time - lastUfoSpawnTime >= UfoInterarrivalTime) {
            SpawnUFO();
        }
    }

    private void SpawnCrystal() {
        var crystal = Instantiate(CrystalPrefab, Planet);
        crystal.GetComponent<PlanetaryObject>().SetRandomPosition();
        crystal.GetComponent<Crystal>().Color = Color.HSVToRGB(Random.value, 1.0f, 1.0f);
    }

    private void SpawnUFO() {
        Instantiate(UfoPrefab, Planet);
        lastUfoSpawnTime = Time.time;
        ufoCount++;
    }

    public void OnCrystalMined() {
        ++PlayerScore;
        PlayerScoreText.text = PlayerScore.ToString();
        SpawnCrystal();
    }
}
