using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MovePlayer", order = 1)]
public class MovePlayerSO : ScriptableObject {
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;


}
