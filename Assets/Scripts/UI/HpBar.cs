using UnityEngine;

public class HpBar : MonoBehaviour {
    [SerializeField]
    private Hp hp;

    private RectTransform rect;

    void Start() {
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        var newScale = rect.localScale;
        newScale.x = hp.value / hp.maxHp;
        rect.localScale = newScale;
    }
}
