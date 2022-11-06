using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour {
    [Header("General")]
    public PlayerController owner;
    public int killRewardGold;

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

    public bool IsAlive() {
        return hp?.value > 0;
    }

    public bool IsDead() {
        return !IsAlive();
    }

    public virtual void Die() {
        Debug.Log($"Died - {name}");
    }

    public void DidKill(Killable deadGuy) {
        owner.gold += deadGuy.killRewardGold;
    }
}
