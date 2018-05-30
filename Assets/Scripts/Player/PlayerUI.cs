using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject infoAbilities;
    public GameObject cdEffect;

    private void Start(){
        CharacterAbilitieBehaviour ab = GetComponent<CharacterAbilitieBehaviour>();

        foreach (var item in ab.cloneAbilities){
            if (item.abilitySprite != null){
                GameObject newObj = new GameObject();
                newObj.AddComponent<Image>();
                newObj.GetComponent<Image>().sprite = item.abilitySprite;
                //newObj.transform.localScale = new Vector3(infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, 1);
                newObj.GetComponent<RectTransform>().sizeDelta = new Vector2((infoAbilities.transform.localScale.y / ab.cloneAbilities.Length) * 100, (infoAbilities.transform.localScale.y / ab.cloneAbilities.Length) * 100);
                newObj.transform.SetParent(infoAbilities.transform);
                GameObject cdEffec = Instantiate(cdEffect, newObj.transform);
                cdEffec.GetComponent<PlayerCooldownInfo>().ab = item;
                foreach (Transform child in cdEffec.transform){
                    child.GetComponent<RectTransform>().sizeDelta = newObj.GetComponent<RectTransform>().sizeDelta;
                }
            }        
        }
    }

    private void Update(){
        
    }



}
