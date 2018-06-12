using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPack : MonoBehaviour {

    public int healing;
    public ParticleSystem particle;
    public float autoDestroyTime;

    private void OnEnable(){
        StartCoroutine(EnableAfterDelay());
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.root.tag == "Player"){
            PlayerCombat pc = collision.gameObject.GetComponent<PlayerCombat>();
            if (pc.health > 0){
                Debug.Log(pc.health + " " + pc.maxHealth);

                if (pc.health < pc.maxHealth){               
                    if (pc.maxHealth - pc.health >= healing){
                        collision.gameObject.GetComponent<PlayerCombat>().health += healing;
                    }
                    else{
                        int restHealing = pc.maxHealth - pc.health;
                        collision.gameObject.GetComponent<PlayerCombat>().health += restHealing;
                    }
                    pc.HealthChangedEvent();
                    gameObject.SetActive(false);

                    ParticleSystem p = Instantiate(particle, transform.position + new Vector3(0, 0, 1), Quaternion.identity);
                    Destroy(p, 5f);

                }
            }
            
        }

    }

    private IEnumerator EnableAfterDelay(){
        yield return new WaitForSeconds(autoDestroyTime);
        gameObject.SetActive(false);

    }
}
