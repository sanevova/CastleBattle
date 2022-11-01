using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {
    [Header("General")]
    public PlayerController owner;

    [HideInInspector]
    public Hp hp;
    private Collider coll;

    void Awake() {
        hp = GetComponent<Hp>();
        coll = GetComponent<Collider>();
    }

    void Update() {

    }

    public virtual float DistanceFrom(Killable other) {
        Ray ray = new(other.transform.position, transform.position - other.transform.position);
        var didRayHit = Physics.Raycast(ray, out var hit);
        var closestPoint = didRayHit && hit.collider == coll
            ? hit.point
            : transform.position;
        return Vector3.Distance(other.transform.position, closestPoint);
    }
}
