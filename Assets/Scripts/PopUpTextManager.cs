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

    public void CreateFloatingTextTransform(Transform pos, string text = null, Sprite texture = null){
        CreateFloatingText(pos.position, text, texture);
    }

    public void CreateFloatingText(Vector3 pos, string text = null, Sprite texture = null){
        popup = Resources.Load<PopupDamage>("PopUpParent");
        //Vector2 wPos = Camera.main.WorldToScreenPoint(new Vector2(pos.x + Random.Range(-3f, 3f), pos.y + Random.Range(-0.5f, 0.5f)));

        PopupDamage pop = Instantiate(popup);
        pop.transform.SetParent(parent.transform);
        pop.transform.position = (pos);

        if (text != null){
            pop.SetText(text);
        }
        if (texture != null){
            pop.SetTexture(texture);
        }

    }
} 
