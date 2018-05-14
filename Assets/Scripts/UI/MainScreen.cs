using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour {
    public enum buttonLayout {Horizontal, Vertical}
    public buttonLayout layoutButtons = buttonLayout.Horizontal;

    public SpriteRenderer[] playerCharacters;
    public Sprite[] characters;

    public List<MainMenuScreens> screens = new List<MainMenuScreens>();
    private MainMenuScreens currenScreen;
    public Button selectedBut;
    //private int screenIndex = 0;
    private bool canChangeButSelect = true;

    private bool readyP1;
    private bool readyP2;
 
    private void Start(){
        NewScreenOpen(0);
        string[] connectedControllers = Input.GetJoystickNames();
        foreach (var item in connectedControllers){
            Debug.Log(item);
        }


    }

    private void Update(){
        if (Input.GetAxis(layoutButtons.ToString()) == 0){
            canChangeButSelect = true;
        }

        if ((Input.GetButtonDown("Fire1") && selectedBut != null) || Input.GetKeyDown(KeyCode.Return) && selectedBut != null){
            selectedBut.onClick.Invoke();
        }

        if (Input.GetButtonDown("Fire2")){
            Back();
            Debug.Log("back");
        }
        
        if (selectedBut != null){
            selectedBut.Select();
        }

        if (currenScreen == screens[1]){
            CharacterScreen();
        }

        if (canChangeButSelect && currenScreen.buttons.Count > 0){
            if (Input.GetAxis(layoutButtons.ToString()) > 0.5f){
                canChangeButSelect = false;
                if (currenScreen.startButtonIndex < currenScreen.buttons.Count - 1){
                    currenScreen.startButtonIndex++;
                }
                else{
                    currenScreen.startButtonIndex = 0;
                }

                selectedBut = currenScreen.buttons[currenScreen.startButtonIndex];
            }

            if (Input.GetAxis(layoutButtons.ToString()) < -0.5f){
                canChangeButSelect = false;
                if (currenScreen.startButtonIndex > 0){
                    currenScreen.startButtonIndex--;
                }
                else{
                    currenScreen.startButtonIndex = currenScreen.buttons.Count - 1;
                }

                selectedBut = currenScreen.buttons[currenScreen.startButtonIndex];
            }
        }
        else{
            selectedBut = null;
        }
    }

    public void Ready(){
        int temp = (System.Array.IndexOf(characters, playerCharacters[0].sprite)) + 1;

        PlayerPrefs.SetInt("CharacterPlayer1", temp);
        PlayerPrefs.SetInt("CharacterPlayer2", temp == 1 ? 2 : 1);
        SceneManager.LoadScene(2);
        
    }

    private void NewScreenOpen(int sc){
        currenScreen = screens[sc];
        if (currenScreen.buttons.Count > 0){
            selectedBut = currenScreen.buttons[currenScreen.startButtonIndex];
        }
    }


    public void Play(){
        screens[0].screenItem.SetActive(false);

        int player1 = Random.Range(0, characters.Length);
        playerCharacters[0].sprite = characters[player1];
        playerCharacters[1].sprite = characters[player1 == 0 ? 1 : 0];

        screens[1].screenItem.SetActive(true);
        NewScreenOpen(1);
        StartCoroutine(Delay());
    }

    private IEnumerator Delay(){
        yield return new WaitForEndOfFrame();
        readyP1 = false;
        readyP2 = false;
    }

    public void Slotmachine(){
        screens[0].screenItem.SetActive(false);
        screens[2].screenItem.SetActive(true);
        NewScreenOpen(2);
    }

    public void Badges(){
        screens[0].screenItem.SetActive(false);
        screens[3].screenItem.SetActive(true);
        NewScreenOpen(3);
    }

    public void Back(){
        readyP1 = false;
        readyP2 = false;
        foreach (var item in screens){
            if (item == screens[0]){
                screens[0].screenItem.SetActive(true);
                continue;
            }
            item.screenItem.SetActive(false);
        }
        NewScreenOpen(0);
    }

    private void CharacterScreen(){
        if ((Input.GetKey(KeyCode.Joystick1Button3) && Input.GetKey(KeyCode.Joystick2Button3))              || Input.GetKeyDown(KeyCode.Space)){
            int temp = System.Array.IndexOf(characters, playerCharacters[0].sprite);
            playerCharacters[0].sprite = characters[temp == 0 ? 1 : 0];
            playerCharacters[1].sprite = characters[temp == 0 ? 0 : 1];
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button0)                                                      || Input.GetKeyDown(KeyCode.Alpha1)){
            readyP1 = true;
            Debug.Log("ready1");
        }

        if (Input.GetKeyDown(KeyCode.Joystick2Button0)                                                      || Input.GetKeyDown(KeyCode.Alpha2)){
            readyP2 = true;
            Debug.Log("ready2");

        }

        if (readyP1 && readyP2){
            Ready();
        }


    }

}

[System.Serializable]
public class MainMenuScreens{

    public GameObject screenItem;
    public List<Button> buttons = new List<Button>();
    public int startButtonIndex;

}