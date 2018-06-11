using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public interface IAttackable{
	void ApplyDamage (int dmg, Vector3 hitPos, Vector3 hitDir);
	bool ValidTarget ();//can be used to return false if health equals zero
	Vector2 Position ();
}

public interface IWormTraverseable{
	float GetLength();
	void Run(WormSegment[] wormSegments);
	void Prepare (Vector3 startPos, Vector3 targetPos);
	Vector3 GetPoint(float t);//t Range(0, 1)
	bool SpawnCentered();
}