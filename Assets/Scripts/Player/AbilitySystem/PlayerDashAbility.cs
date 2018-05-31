using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/PlayerDashAbility")]
public class PlayerDashAbility : Ability{

    public float dashDuration;
    public float power;

    public float ghostTimer;
    public float ghostActiveTimer;
    public GameObject ghostPrefab;
    public int amountGhosts;


    private float dashTime;
    private float curPower;
    private GameObject player;

    private PlayerGhost ghostSprite;
    private GameObject ghostParent;
    private SpriteRenderer playerSprite;
    private List<GameObject> ghostsObj = new List<GameObject>();
    private List<PlayerGhost> ghostsScript = new List<PlayerGhost>();

    private float nextGhost = 0;
    private int currentGhostIndex = 0;

    public override void Init(GameObject p){
        player = p;
        playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        ghostsScript.Clear();
        ghostsObj.Clear();
        nextGhost = 0;
        currentGhostIndex = 0;
        ghostParent = new GameObject();
        ghostParent.name = "Ghost Parent";

        for (int i = 0; i < amountGhosts; i++){
            GameObject g = Instantiate(ghostPrefab, ghostParent.transform);
            ghostSprite = ghostPrefab.GetComponent<PlayerGhost>();
            ghostSprite.ghostTimer = ghostActiveTimer;
            ghostSprite.SetSprite(player);
            ghostsScript.Add(ghostSprite);
            ghostsObj.Add(g);
            g.transform.position = player.transform.position;
        }

    }

    public override IEnumerator TriggerAbility(){
        Debug.Log("Dash");
        currentGhostIndex = 0;
    
        curPower = power;
        dashTime += Time.deltaTime;

        PlayerBase pb = player.GetComponent<PlayerBase>();
        pb.canControl = false;

        Vector2 dashVelocity = player.transform.localScale.x > 0 ? Vector2.right * curPower : Vector2.left * curPower;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        CharacterAbilitieBehaviour cb = player.GetComponent<CharacterAbilitieBehaviour>();
        bool stun = cb.isStunned;

        while (dashTime <= dashDuration && stun == false){
            Ghost();
            stun = cb.isStunned;
            dashTime += Time.deltaTime;
            curPower = Mathf.Lerp(curPower, 0, 1f / dashTime);
            rb.velocity = dashVelocity;
            yield return new WaitForEndOfFrame();
        }

        pb.canControl = true;
        rb.velocity = Vector3.zero;
        dashTime = 0;
    }

    private void Ghost(){
        if (nextGhost < Time.time){
            nextGhost = Time.time + ghostTimer;
            currentGhostIndex += 1;
            if (currentGhostIndex >= ghostsScript.Count){
                currentGhostIndex = 0;
            }
            ghostsObj[currentGhostIndex].transform.position = player.transform.position;
            ghostsObj[currentGhostIndex].transform.localScale = player.transform.localScale;
            ghostsObj[currentGhostIndex].gameObject.SetActive(true);
        }

    }
}
