using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPack : MonoBehaviour {

    public int healing;

    private void OnCollisionEnter2D(Collision2D collision){
            PlayerCombat pc = collision.gameObject.GetComponent<PlayerCombat>();
            Debug.Log(pc.health + " " + pc.maxHealth);
            if (pc.health < pc.maxHealth){
                if (pc.maxHealth - pc.health >= healing){
                    collision.gameObject.GetComponent<PlayerCombat>().health += healing;
                }
                else{
                    int restHealing = pc.maxHealth - pc.health;
                    collision.gameObject.GetComponent<PlayerCombat>().health += restHealing;
                } 

                gameObject.SetActive(false);
            }
        }

    }

}
