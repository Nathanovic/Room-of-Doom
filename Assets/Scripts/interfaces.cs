﻿using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public interface IAttackable{
	void ApplyDamage (int dmg, float otherX);
	bool ValidTarget ();//can be used to return false if health equals zero
	Vector2 Position ();
}