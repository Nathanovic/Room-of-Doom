using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadManager : MonoBehaviour {

    public static DeadManager instance;

    public int deadCount;
    public int winCount;
    public int revivedCount;
    public int lostCount;

    public Text deadAmount;
    public Text revivesAmount;
    public Text winsAmount;
    public Text lostAmount;

    private int playersAlive;
    private GameObject gameOverScreen;

    private void Awake(){
        instance = this;

    }

    private void Start(){
        StartCoroutine(GetPlayersAliveDelay());
        gameOverScreen = transform.GetChild(0).gameObject;
        gameOverScreen.SetActive(false);

        if (Display.displays.Length > 1){
            Display.displays[1].Activate();
            Display.displays[1].SetParams(1920, 1080, 0, 0);
        }

        if (Display.displays.Length > 2){
            Display.displays[2].Activate();
            Display.displays[1].SetParams(1920, 1080, 0, 0);
        }

        lostCount = PlayerPrefs.GetInt("Lost");
        winCount = PlayerPrefs.GetInt("Win");
        revivedCount = PlayerPrefs.GetInt("Revived");
        deadCount = PlayerPrefs.GetInt("Dead");

        deadAmount.text = deadCount.ToString();
        revivesAmount.text = revivedCount.ToString();
        winsAmount.text = winCount.ToString();
        lostAmount.text = lostCount.ToString();

    }

    private void Update(){
        if (gameOverScreen.activeInHierarchy){
            if (Input.GetKeyDown(KeyCode.JoystickButton0)){
                Restart();
            }
        }
    }

    public void OnPlayerRevive(){
        revivedCount += 1;
        playersAlive += 1;
        revivesAmount.text = revivedCount.ToString();
    }

    public void PlayerDied(){
        deadCount++;
        playersAlive--;
        deadAmount.text = deadCount.ToString();

        if (playersAlive == 0){
            GameOver();
        }

        if (false){
            GameWon();
        }
    }

    private IEnumerator GetPlayersAliveDelay(){
        yield return new WaitForSeconds(0.3f);
        playersAlive = CameraMovement.players.Count;

    }

    private void GameWon(){
        SetCounts();
    }

    private void GameOver(){
        lostCount++;
        gameOverScreen.SetActive(true);
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

	[ContextMenu("ResetCount")] //wow sick! dit is vet handig! :)
    public void ResetCount(){
        PlayerPrefs.SetInt("Lost", 0);
        PlayerPrefs.SetInt("Win", 0);
        PlayerPrefs.SetInt("Revived", 0);
        PlayerPrefs.SetInt("Dead", 0);
    }
}
