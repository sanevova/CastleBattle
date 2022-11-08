using UnityEngine;

public class Hp : MonoBehaviour {
    public float maxHp = 100;

    public float value;
    [HideInInspector]
    public Killable hpOwner;

    void Awake() {
        value = maxHp;
        hpOwner = GetComponent<Killable>();
    }

    void Update() {

    }

    public void TakeDamageFrom(Killable damageSource, float damageValue) {
        value = Mathf.Max(0, value - damageValue);
        if (value == 0) {
            hpOwner.Die();
            damageSource.DidKill(hpOwner);
        }
    }
}
