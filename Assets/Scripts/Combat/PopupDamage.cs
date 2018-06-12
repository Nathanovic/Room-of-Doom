using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDamage : MonoBehaviour {

    public List<Color> colors = new List<Color>();
    public int fontSizeRange;

    private Animator ani;
	private Text damageText;
    private Image texture;

	private void OnEnable(){
        ani = transform.GetChild(0).GetComponent<Animator>();
        AnimatorClipInfo[] clipInfos = ani.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfos[0].clip.length);
		damageText = transform.GetChild(0).GetComponent<Text>();
        texture = transform.GetChild(1).GetComponent<Image>();

		Debug.Log ("damage text: " + damageText);
    }

    public void SetText(string text){
        if (colors.Count > 0){
            damageText.color = colors[Random.Range(0, colors.Count - 1)];
            Debug.Log(colors[Random.Range(0, colors.Count - 1)]);
            Debug.Log(damageText.color);
        }
        damageText.fontSize = Random.Range(damageText.fontSize - fontSizeRange, damageText.fontSize + fontSizeRange);
        damageText.text = text;

    }

    public void SetTexture(Sprite t){
        texture.sprite = t;
        texture.enabled = true;
    }

}
