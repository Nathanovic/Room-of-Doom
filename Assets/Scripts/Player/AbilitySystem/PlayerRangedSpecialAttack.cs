using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerRangedSpecialAttack")]
public class PlayerRangedSpecialAttack : Ability{

    public GameObject projectile;
    //public float dis;

    public float attackRange;
    public float spawnRadiusX;
    public float SpawnRadiusY;
    [Range(0, 1)]
    public float spawnWidth;

    private int amount;
    private Vector3 centrePos;

    public override IEnumerator TriggerAbility(GameObject player){
        Debug.Log("PlayerRangedSpecialAttack");

        List <Vector3> mw = new List<Vector3>();
        if (BossManager.instance.GetHeadPositions(player.transform.position, attackRange).Count != 0){
            foreach (var item in BossManager.instance.GetHeadPositions(player.transform.position, attackRange)){
                mw.Add(item);
            }
        }


        amount = mw.Count;

        for (var pointNum = 0; pointNum < amount; pointNum++){
            centrePos = player.transform.position;
            var i = ((pointNum - ((amount - 1) * 0.5f)) * spawnWidth) / amount;
            float angle = (float)(i * Mathf.PI * 2);

            var x = Mathf.Sin(angle) * spawnRadiusX;
            var y = Mathf.Cos(angle) * SpawnRadiusY;
            var pos = new Vector3(x, y, 0) + centrePos;

            Vector3 instaPos = player.transform.position + new Vector3(player.transform.localScale.x > 0 ? 0.3f : -0.3f, 0.3f);
            GameObject proj = Instantiate(projectile, instaPos, Quaternion.identity);
            //GameObject proj = Instantiate(projectile, pos, Quaternion.identity);
            ProjectileMovement pm = proj.GetComponent<ProjectileMovement>();

            pm.hasTarget = true;
            pm.delayNextTarget = Random.Range(0.7f, 1);
            pm.target.Add(pos);
            pm.target.Add(mw[pointNum]);

        }

        yield return null;
    }
 
}

/*
        for (int i = 0; i < amount; i++){
            Vector3 pos = player.transform.position + new Vector3(player.transform.localScale.x > 0 ? 0.3f : -0.3f, 0.3f);
            GameObject clonedBullet = Instantiate(projectile, pos, Quaternion.identity) as GameObject;
            ProjectileMovement pm = clonedBullet.GetComponent<ProjectileMovement>();

            pm.hasTarget = true;
            pm.delayNextTarget = Random.Range(0.7f, 1);
            pm.target.Add(new Vector2(player.transform.position.x, player.transform.position.y + (amount - i) * dis));
            pm.target.Add(new Vector2(6, 5));
        }
 */