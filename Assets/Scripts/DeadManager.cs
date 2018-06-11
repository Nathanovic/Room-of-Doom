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
<<<<<<< HEAD
        revivedCount += 1;
        playersAlive += 1;
        revivesAmount.text = revivedCount.ToString();

=======
        playersRevived++;
        playersAlive++;
>>>>>>> 3117246008abfc7cc41ac310ea08ac7eb784ea81
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
<<<<<<< HEAD
        lostAmount.text = lostCount.ToString();
        SetCounts();

    }

    public void Restart(){
        SetCounts();

        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

    private void SetCounts(){
        PlayerPrefs.SetInt("Lost", lostCount);
        PlayerPrefs.SetInt("Win", winCount);
        PlayerPrefs.SetInt("Revived", revivedCount);
        PlayerPrefs.SetInt("Dead", deadCount);
    }

    [ContextMenu("ResetCount")] 
    public void ResetCount(){
        PlayerPrefs.SetInt("Lost", 0);
        PlayerPrefs.SetInt("Win", 0);
        PlayerPrefs.SetInt("Revived", 0);
        PlayerPrefs.SetInt("Dead", 0);
    }

=======

    }

    public void Restart(){ 
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

>>>>>>> 3117246008abfc7cc41ac310ea08ac7eb784ea81
}
