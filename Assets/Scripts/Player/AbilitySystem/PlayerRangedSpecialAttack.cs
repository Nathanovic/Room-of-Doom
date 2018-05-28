using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerRangedSpecialAttack")]
public class PlayerRangedSpecialAttack : Ability{

    public GameObject projectile;
    public int amount;
    public float dis;

    public override IEnumerator TriggerAbility(GameObject player){
        Debug.Log("PlayerRangedSpecialAttack");

        for (int i = 0; i < amount; i++){
            GameObject clonedBullet = Instantiate(projectile, player.transform.position + new Vector3(player.transform.localScale.x > 0 ? 0.3f : -0.3f, 0.3f), Quaternion.identity) as GameObject;
            ProjectileMovement pm = clonedBullet.GetComponent<ProjectileMovement>();

            pm.hasTarget = true;
            pm.delayNextTarget = Random.Range(0.7f, 1);
            pm.target.Add(new Vector2(player.transform.position.x, player.transform.position.y + (amount - i) * dis));
            pm.target.Add(new Vector2(6, 5));
        }

        yield return null;
    }
}
