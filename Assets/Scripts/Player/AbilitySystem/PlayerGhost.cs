using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

    [HideInInspector]
    public float timer;
    [HideInInspector]
    public float ghostTimer;
    [HideInInspector]
    public Sprite currentSprite;
    [HideInInspector]
    public SpriteRenderer playerSprite;

    private SpriteRenderer sp;

    private void Awake(){
        sp = GetComponent<SpriteRenderer>();
    }

    private void OnEnable(){
        sp.sprite = playerSprite.sprite;
    }

    private void Update(){
        timer -= Time.deltaTime;

        if (timer < 0){
            timer = ghostTimer;
            gameObject.SetActive(false);
        }

    }

    public void SetSprite(GameObject p){
        playerSprite = p.GetComponentInChildren<SpriteRenderer>();

    }

}
