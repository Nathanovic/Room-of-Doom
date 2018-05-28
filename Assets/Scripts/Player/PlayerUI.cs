using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Image infoPanel;
    public GameObject inforAbilities;
    public GameObject cdEffect;

    private void Start(){
        CharacterAbilitieBehaviour ab = GetComponent<CharacterAbilitieBehaviour>();

        foreach (var item in ab.characterAbilities){
            if (item.abilitySprite != null){
                GameObject newObj = new GameObject();
                newObj.AddComponent<Image>();
                newObj.GetComponent<Image>().sprite = item.abilitySprite;
                newObj.transform.SetParent(inforAbilities.transform);
                newObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                GameObject cdEffec = Instantiate(cdEffect, newObj.transform);
                cdEffec.SetActive(false);
            }        
        }
    }

    private void Update(){
        
    }



}
