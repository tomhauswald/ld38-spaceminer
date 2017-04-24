using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMaster : MonoBehaviour {

    public GameObject CrystalPrefab;
    public Transform Planet;
    public uint CrystalCount;

	// Use this for initialization
	void Start () {
        CrystalPrefab.GetComponent<PlanetaryObject>().Planet = Planet;
        for (var i=0; i<CrystalCount; ++i) {
            SpawnCrystal();
        }
	}

    private void SpawnCrystal() {
        var crystal = Instantiate(CrystalPrefab, Planet);
        crystal.GetComponent<PlanetaryObject>().SetRandomPosition();
        crystal.GetComponent<Crystal>().Color = Color.HSVToRGB(Random.value, 1.0f, 1.0f);
    }
}
