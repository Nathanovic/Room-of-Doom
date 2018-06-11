using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

    public int scene;
    public PlayerInput.Button button;
    private PlayerInput playerInput;
    public bool isActive = true;
    public bool showOverlay = true;
    public GameObject overlay;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.ButtonIsDown(button) || Input.GetKeyDown(KeyCode.Space))
        {
            Press(scene, showOverlay);
        }
    }

    public void Press(int _scene, bool _showOverlay)
    {
        StartCoroutine(LoadScene(scene,showOverlay));
    }

    private void ActivateOverlay(GameObject _overlay)
    {
        if (_overlay != null)
        {
            _overlay.SetActive(true);
        }
    }

    private IEnumerator LoadScene(int _scene, bool _showOverlay)
    {
        if (_showOverlay == true)
        {
            ActivateOverlay(overlay);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_scene);
        Debug.Log(gameObject + " is pressed and scene " + _scene + " is being loaded.");
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
