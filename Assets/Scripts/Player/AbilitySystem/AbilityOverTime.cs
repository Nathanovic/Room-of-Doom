using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityOverTime : ScriptableObject{

    public string nameAbility;
    public PlayerInput.Button button;
    public float couldown;
    public float castingTime;
    public Sprite abilitySprite;
    public AudioClip soundEffect;

    public float readyAtTime;

    public AbilityOverTime Clone() { AbilityOverTime other = (AbilityOverTime)this.MemberwiseClone(); return other; }
    public void Cooldown() { readyAtTime = couldown + Time.time; }
    public abstract IEnumerator TriggerAbility(GameObject player);
    public abstract void StopAbility();
}
