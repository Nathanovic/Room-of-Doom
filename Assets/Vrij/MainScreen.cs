using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour {

    public GameObject badges, slotmachine, play;

    public int amountPlayers = 2;
    public int readyPlayers = 0;


    public void Ready(){       
        readyPlayers++;
        PlayerPrefs.SetInt("CharacterPlayer1", 0);
        PlayerPrefs.SetInt("CharacterPlayer1", 1);

        if (readyPlayers == amountPlayers){
            SceneManager.LoadScene(2);
        }
    }
    
    public void Play(){
        play.SetActive(true);
    }

    public void Slotmachine(){
        slotmachine.SetActive(true);
    }

    public void Badges(){
        badges.SetActive(true);
    }

    public void Back(){
        if (badges.activeInHierarchy == false && slotmachine.activeInHierarchy == false && play.activeInHierarchy == false){
            SceneManager.LoadScene(0);
            return;
        }

        badges.SetActive(false);
        slotmachine.SetActive(false);
        play.SetActive(false);
    }

}
