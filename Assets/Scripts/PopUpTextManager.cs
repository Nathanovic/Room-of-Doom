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
        popup = Resources.Load<PopupDamage>("PopUpParent");

    }

    public void CreateFloatingText(string tex, Transform pos){
        PopupDamage pop = Instantiate(popup);
        pop.transform.SetParent(parent.transform);
        pop.SetText(tex);

    }


}
