using UnityEngine;
using UnityEngine.UI;

//this script is used to show world-notifications
public class NotificationPanel : MonoBehaviour {

	public static NotificationPanel instance;

	private PanelState state = PanelState.Idle;
	private CanvasGroup panel;
	private Text panelText;

	public float fadeTime = 2f;
	public float maxActiveTime = 2f;
	private float activeMoment = 0f;
	private float fadeMoment;

	private void Awake(){
		instance = this;
	}

	private void Start () {
		panel = GetComponent<CanvasGroup> ();
		panelText = GetComponentInChildren<Text> ();
		panel.alpha = 0f;
	}

	private void Update () {
		if (state == PanelState.Idle) {
			//make sure we can't stay active for too long:
			if (panel.alpha > 0f) {
				activeMoment += Time.deltaTime;
				if (activeMoment >= maxActiveTime) {
					DeactivateNotification ();
				}
			}

			return;
		}

		//make sure to fade the panel in or out over time:
		switch (state) {
		case PanelState.FadeIn:
			fadeMoment += Time.deltaTime;
			if (fadeMoment >= fadeTime) {
				state = PanelState.Idle;
			}
			break;
		case PanelState.FadeOut:
			fadeMoment -= Time.deltaTime;
			if (fadeMoment <= 0f) {
				state = PanelState.Idle;
			}
			break;
		}
			
		float alpha = 1f / fadeTime * fadeMoment;
		panel.alpha = alpha;
	}

	public void ShowNotification(Vector3 position, string text){
		activeMoment = 0f;
		transform.position = position;
		panelText.text = text;

		if (panel.alpha < 1f) {
			state = PanelState.FadeIn;
		}
	}

	public void DeactivateNotification(){
		if (panel.alpha > 0f) {
			state = PanelState.FadeOut;
		}
	}

	private enum PanelState{
		Idle,
		FadeIn,
		FadeOut
	}
}
