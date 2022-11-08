using UnityEngine;

public class HpBar : MonoBehaviour {
    [SerializeField]
    private Hp hp;
    [SerializeField]
    private Transform hpBarLocation;
    [SerializeField]
    private bool isBackground;

    private RectTransform rect;

    void Start() {
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        rect.position = Camera.main.WorldToScreenPoint(hpBarLocation.transform.position);
        if (isBackground) {
            return;
        }
        var newScale = rect.localScale;
        newScale.x = hp.value / hp.maxHp;
        rect.localScale = newScale;
    }
}
