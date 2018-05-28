using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerRangedNormalAttack")]
public class PlayerRangedNormalAttack : Ability {

    public GameObject projectile;


    public override IEnumerator TriggerAbility(GameObject player){
        Debug.Log("PlayerRangedNormalAttack");

        GameObject clonedBullet = Instantiate(projectile, player.transform.position + new Vector3(player.transform.localScale.x > 0 ? 0.3f : -0.3f, 0), Quaternion.identity) as GameObject;
        clonedBullet.GetComponent<ProjectileMovement>().speed *= player.transform.localScale.x > 0 ? 1 : -1;
        yield return null;
    }

}
