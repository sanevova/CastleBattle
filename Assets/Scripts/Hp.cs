using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour {
    public float maxHp = 100;

    public float value;
    [HideInInspector]
    public Killable owner;

    void Awake() {
        value = maxHp;
        owner = GetComponent<Killable>();
    }

    void Update() {

    }

    public void TakeDamage(float damageValue) {
        value = Mathf.Max(0, value - damageValue);
        Debug.Log(value);
    }
}
