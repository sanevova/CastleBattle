using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {

    [HideInInspector]
    public Hp hp;

    void Awake() {
        hp = GetComponent<Hp>();
    }

    void Update() {

    }

    public bool IsEnemy(Killable other) {
        return other != this;
    }
}
