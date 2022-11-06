using UnityEngine;
using TMPro;


public class UIController : MonoBehaviour {
    public PlayerController player;

    [SerializeField]
    private TMP_Text goldText;

    void Update() {
        goldText.text = $"£ {player.gold}";
    }
}
