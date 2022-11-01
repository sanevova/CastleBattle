using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {
    public GameObject spawnTarget;
    public Transform spawnPoint;
    public float spawnDelay;

    private float lastSpawnTime = 0;


    void Start() {
        // spawnDelay = Mathf.Max(spawnDelay, 5);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Spawn();
        }

        if (Time.time - lastSpawnTime >= spawnDelay) {
            lastSpawnTime = Time.time;
            Spawn();
        }
    }

    void Spawn() {
        Instantiate(spawnTarget, spawnPoint.position, Quaternion.identity);
    }
}
