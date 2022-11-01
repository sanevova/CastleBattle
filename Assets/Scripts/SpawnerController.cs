using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : Killable {
    [Header("Spawner")]
    public UnitController spawnTarget;
    public Transform spawnPoint;
    public float spawnDelay;

    private float lastSpawnTime;


    void Start() {
        // don't spawn immediately and wait spawnDelay first
        lastSpawnTime = Time.time;
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
        var spawn = Instantiate(spawnTarget, spawnPoint.position, Quaternion.identity);
        spawn.owner = owner;
    }
}
