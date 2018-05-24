using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public interface IAttackable{
	void ApplyDamage (int dmg, Vector3 hitPos);
	bool ValidTarget ();//can be used to return false if health equals zero
	Vector2 Position ();
}