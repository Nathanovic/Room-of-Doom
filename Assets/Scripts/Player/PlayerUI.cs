using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    //public GameObject infoAbilities;
    public GameObject cdEffect;
    public List<GameObject> abilities = new List<GameObject>();

    private void Start(){
        CharacterAbilitieBehaviour ab = GetComponent<CharacterAbilitieBehaviour>();

        for (int i = 0; i < ab.cloneAbilities.Length; i++){
            if (ab.cloneAbilities[i].abilitySprite != null){
                //sprite for image component.
                Sprite sprite = ab.cloneAbilities[i].abilitySprite;

                //GameObject newObj = new GameObject();
                //newObj.AddComponent<Image>();
                //newObj.GetComponent<Image>().sprite = ab.cloneAbilities[i].abilitySprite;
                //newObj.transform.localScale = new Vector3(infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, 1);
                //newObj.GetComponent<RectTransform>().sizeDelta = newObj.GetComponentInParent<RectTransform>().sizeDelta;
                //newObj.GetComponent<RectTransform>().position = Vector3.one;
                //newObj.transform.SetParent(positions[i]);
                GameObject cdEffec = Instantiate(cdEffect, abilities[i].transform);
                cdEffec.GetComponent<PlayerCooldownInfo>().ab = ab.cloneAbilities[i];
                //set image of ability.
                abilities[i].GetComponentsInChildren<Image>()[0].sprite = sprite;
                foreach (Transform child in cdEffec.transform){
                    //child.GetComponent<RectTransform>().sizeDelta = ab.cloneAbilities[i].GetComponent<RectTransform>().sizeDelta;
                }
            } 
        }
    }

    private void Update(){
        
    }



}

/*
 *         foreach (var item in ab.cloneAbilities){
            if (item.abilitySprite != null){
                GameObject newObj = new GameObject();
                newObj.AddComponent<Image>();
                newObj.GetComponent<Image>().sprite = item.abilitySprite;
                //newObj.transform.localScale = new Vector3(infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, infoAbilities.transform.localScale.y / ab.cloneAbilities.Length, 1);
                //newObj.GetComponent<RectTransform>().sizeDelta = new Vector2((infoAbilities.transform.localScale.y / ab.cloneAbilities.Length) * 100, (infoAbilities.transform.localScale.y / ab.cloneAbilities.Length) * 100);
                newObj.transform.SetParent(positions[i]transform);
                GameObject cdEffec = Instantiate(cdEffect, newObj.transform);
                cdEffec.GetComponent<PlayerCooldownInfo>().ab = item;
                foreach (Transform child in cdEffec.transform){
                    child.GetComponent<RectTransform>().sizeDelta = newObj.GetComponent<RectTransform>().sizeDelta;
                }
            }        
        }*/
