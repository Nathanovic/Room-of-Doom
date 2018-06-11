using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {
    public int scene;

    private void Start()
    {
        Button buttonComponent = gameObject.GetComponent<Button>();
    }

    public void Pressed()
    {
        SceneManager.LoadScene(scene);
    }
}
