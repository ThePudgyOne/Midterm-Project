using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    //instance of our game object 
    public GameObject pauseMenu;
    public SceneController sceneController; // reference Scene Controller

    public static bool isPaused; //!PauseMenu.isPaused <- input in other files to prevent interaction

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(true); // hides the pause menu initially
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // check for input to pause and unpause game
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }


    // if game is paused freeze everything within the game
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // freezes input 
        isPaused = true;
        sceneController.flippable = false; // Can't flip cards when paused
        Debug.Log("GAME HAS BEEN PAUSED");
    }


    //if game is unpaused resume everything within game switch to false
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        sceneController.flippable = true; // Back to game, flip cards
        Debug.Log("GAME HAS BEEN UNPAUSED");
    }

    //Quits the game 
    public void QuitGame()
    {
        Application.Quit();
    }

    /*/ ----- NOT IN USE ----
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // CHANGE LATER WHEN MENU CREATED
        isPaused = false;
    }*/

    
}
