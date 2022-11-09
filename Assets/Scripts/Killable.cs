using TMPro;
using UnityEngine;

public class Killable : MonoBehaviour {
    [Header("General")]
    public PlayerController owner;
    public uint killRewardGold;
    [SerializeField]
    public TMP_Text plusGoldText;

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

    protected bool IsEnemy(Killable other) {
        return owner.team != other.owner.team;
    }

    public virtual void Die() {
    }

    public void DidKill(Killable deadGuy) {
        owner.gold += deadGuy.killRewardGold;
        if (IsClientPlayerTeam()) {
            ShowPlusGoldFloatingText(deadGuy);
        }
    }

    private void ShowPlusGoldFloatingText(Killable deadGuy) {
        if (deadGuy.plusGoldText == null) {
            Debug.Log($"NO TEXT ON {deadGuy.name}");
            return;
        }
        deadGuy.plusGoldText.text = $"+{deadGuy.killRewardGold}";
        deadGuy.plusGoldText.GetComponent<Animator>().Play("Float");
        Debug.Log($"{name} killed {deadGuy.name}");
    }

    private bool IsClientPlayerTeam() {
        // TODO: GENERALIZE
        return owner.team == Team.Left;
    }
}
