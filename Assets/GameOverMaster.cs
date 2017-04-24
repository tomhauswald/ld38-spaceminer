using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMaster : MonoBehaviour {

    public GameObject CrystalPrefab;
    public GameObject UfoDummyPrefab;
    public Transform Planet;
    public uint CrystalCount = 200;

    public Text ScoreText;
    public Text HighScoreText;

    // Use this for initialization
    void Start () {
        CrystalPrefab.GetComponent<PlanetaryObject>().Planet = Planet;
        UfoDummyPrefab.GetComponent<PlanetaryObject>().Planet = Planet;

        for (var i=0; i<CrystalCount; ++i) {
            SpawnCrystal();
        }

        SpawnUfo();
        ScoreText.text = string.Format("{0:D4}", PlayerPrefs.GetInt("Latest"));
        HighScoreText.text = string.Format("{0:D4}", PlayerPrefs.GetInt("Best"));
    }

    private void SpawnCrystal() {
        var crystal = Instantiate(CrystalPrefab, Planet);
        crystal.GetComponent<PlanetaryObject>().SetRandomPosition();
        crystal.GetComponent<Crystal>().Color = Color.HSVToRGB(Random.value, 1.0f, 1.0f);
    }

    private void SpawnUfo() {
        var ufo = Instantiate(UfoDummyPrefab, Planet);
        ufo.GetComponent<PlanetaryObject>().SetPosition(180, -45);
        ufo.GetComponent<PlanetaryObject>().Yaw = 0.0f;
    }
}
