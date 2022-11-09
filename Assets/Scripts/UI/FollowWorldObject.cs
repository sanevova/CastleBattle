using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWorldObject : MonoBehaviour {
    [SerializeField]
    private Transform follow;

    private RectTransform rect;

    void Start() {
        rect = GetComponent<RectTransform>();
    }

    void Update() {
        rect.position = Camera.main.WorldToScreenPoint(follow.position);
    }
}
