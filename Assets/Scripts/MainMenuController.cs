using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuController : MonoBehaviour
{

    public SceneController sceneController;


    //default values
    public bool isNormal = true;
    public bool isStandard = true;
    public bool isRegularPack = true;

    
    //Serialized fields for text to change
    [SerializeField] private TextMeshProUGUI gameModeText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI spritePack; 

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set initial values
        UpdateGameModeText();
        UpdateDifficultyText();
        UpdateSpritePackName();
        
    }

    //Toggles game mode to switch accordingly 
    public void ToggleGameMode()
    {
        isStandard = !isStandard; // switch
        sceneController.timeAttackMode = !sceneController.timeAttackMode; //switch
        UpdateGameModeText();

        if (!isStandard) //set to time attack
        {
            sceneController.timeAttackMode = true;
        }
        else //normal mode
        {
            sceneController.timeAttackMode = false;
        }

    }

    //Changes difficulty which is time based
    public void ToggleDifficulty()
    {
        isNormal = !isNormal; // switch
        UpdateDifficultyText();

        if (!isNormal) // set to hard
        {
            sceneController.startTime = 10f;
        }
        else //set to normal
        {
            sceneController.startTime = 20f;
        }
    }

    //Changes between the two packs more later on
    public void TogglePack()
    {
        isRegularPack = !isRegularPack; // switch
        UpdateSpritePackName();
    }
    
    //Updates The text on the button as well as the respectful setting
    private void UpdateGameModeText()
    {
        gameModeText.text = isStandard ? "GAMEMODE: STANDARD" : "GAMEMODE: TIME ATTACK";

       // sceneController.timeAttackMode = sceneController.timeAttackMode ? true : false; 
        if (sceneController.timeAttackMode == true)
        {
            Debug.Log("TIME ATTACK IS ON RUNNN");
        }
        else if (sceneController.timeAttackMode == false)
        {
            Debug.Log("STANDARD IS NOW RUNNING");

        }
    }


    //IF ELSE wwith the switches
    private void UpdateDifficultyText()
    {
        difficultyText.text = isNormal ? "DIFFICULTY: NORMAL" : "DIFFICULTY: HARD"; 
    }

    private void UpdateSpritePackName()
    {
        spritePack.text = isRegularPack ? "CARD PACK: NORMAL" : "CARD PACK: GOOFY";
    }

    //Quits the game 
    public void QuitGame()
    {
        Application.Quit();
    }

}
