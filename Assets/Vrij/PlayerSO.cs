using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MovePlayer", order = 1)]
public class PlayerSO : ScriptableObject {
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public int player;
}
