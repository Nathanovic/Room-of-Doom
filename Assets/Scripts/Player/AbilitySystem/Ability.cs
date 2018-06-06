using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public string nameAbility;
    public PlayerInput.Button button;
    public float cooldown;
    public float castingTime;
    public Sprite abilitySprite;
    public AudioClip soundEffect;
    public float readyAtTime;

    public Ability Clone() { Ability other = (Ability)this.MemberwiseClone(); return other; }
    public void Cooldown() { readyAtTime = cooldown + Time.time; }
    public abstract void Init(GameObject player);
    public abstract IEnumerator TriggerAbility();

}
