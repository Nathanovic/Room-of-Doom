using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadManager : MonoBehaviour {

    public static DeadManager instance;

    public int deadCount;
    public int winCount;
    public int playersRevived;

    private int playersAlive;
    private GameObject gameOverScreen;

    private void Awake(){
        instance = this;

    }

    private void Start(){
        StartCoroutine(GetPlayersAliveDelay());
        gameOverScreen = transform.GetChild(0).gameObject;
        gameOverScreen.SetActive(false);

    }

    private void Update(){

        if (gameOverScreen.activeInHierarchy){
            if (Input.GetKeyDown(KeyCode.JoystickButton0)){
                Restart();
            }
        }
    }

    public void OnPlayerRevive(){
        playersRevived++;
        playersAlive++;
    }

    public void OnPlayerDead(){
        deadCount++;
        playersAlive--;

        if (playersAlive == 0){
            GameOver();
        }
    }

    private IEnumerator GetPlayersAliveDelay(){
        yield return new WaitForSeconds(0.3f);
        playersAlive = CameraMovement.players.Count;

    }

    private void GameOver(){
        gameOverScreen.SetActive(true);

    }

    public void Restart(){ 
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

}
