using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextManager : MonoBehaviour {

    public static PopUpTextManager instance;

    private PopupDamage popup;
    private GameObject parent;

    private void Start(){
        instance = this;
        parent = gameObject;
        //popup = Resources.Load<PopupDamage>("PopUpParent");

    }

    public void CreateFloatingText(Transform pos, string text = null, Sprite texture = null){
        popup = Resources.Load<PopupDamage>("PopUpParent");

        PopupDamage pop = Instantiate(popup);
        pop.transform.SetParent(parent.transform);

        if (text != null){
            pop.SetText(text);
        }
        else if (texture != null){
            pop.SetTexture(texture);
        }

    }


} 
