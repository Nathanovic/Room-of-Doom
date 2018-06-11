using UnityEngine;

public static class ExtensionMethods {

	public static void Activate(this CanvasGroup panel){
		panel.alpha = 1f;
		panel.interactable = true;
		panel.blocksRaycasts = true;
	}

	public static void Deactivate(this CanvasGroup panel){
		panel.alpha = 0f;
		panel.interactable = false;
		panel.blocksRaycasts = false;
	}
}
