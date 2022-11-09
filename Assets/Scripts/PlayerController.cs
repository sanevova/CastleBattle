using UnityEngine;

public enum Team {
    Left,
    Right
}

public class PlayerController : MonoBehaviour {
    public Team team;

    [Header("Gold")]
    public uint gold;

}
