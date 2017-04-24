using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Pickup {

    public enum CrystalState {
        Growing,
        Done
    };

    public Color Color = Color.blue;
    public AudioClip MiningSound;
    public float GrowingDuration = 3.0f;

    private Game game;
    private AudioSource audioSource;

    private CrystalState state;
    private Vector3 targetLocalScale;
    private float growingTime;

    public Crystal()
        :base(pickup => ((Crystal) pickup).OnMined()) {
    }

	// Use this for initialization
	void Start () {
        game = FindObjectOfType<Game>();
        audioSource = FindObjectOfType<AudioSource>();
	    foreach(var renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = Color;
        }
        state = CrystalState.Growing;
        targetLocalScale = transform.localScale * Random.Range(0.75f, 1.25f);
        transform.localRotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
        growingTime = 0.0f;
    }

    private void Update() {
        if(state == CrystalState.Growing) {
            transform.localScale = targetLocalScale * Mathf.Lerp(0, 1, growingTime / GrowingDuration);
            if ((growingTime += Time.deltaTime) > GrowingDuration) {
                state = CrystalState.Done;
            }
        }
    }

    private void OnMined() {
        if (MiningSound.loadState == AudioDataLoadState.Loaded) {
            audioSource.PlayOneShot(MiningSound);
        }
        game.OnCrystalMined();
    }
}
