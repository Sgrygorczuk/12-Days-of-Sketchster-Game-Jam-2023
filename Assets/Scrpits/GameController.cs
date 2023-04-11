using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //==================================================================================================================
    // Variables 
    //==================================================================================================================
    private GameObject _winCanvas;
    private GameObject _loseCanvas;

    public string levelName;
    public string menuName;
    
    //==================================================================================================================
    // Variables 
    //==================================================================================================================

    /// <summary>
    /// Connects the game objects and turns them off so player can't see them till the game is over 
    /// </summary>
    void Start()
    {
        _winCanvas = GameObject.Find("Canvas").transform.Find("WinScreen").gameObject;
        _loseCanvas = GameObject.Find("Canvas").transform.Find("LoseScreen").gameObject;

        _winCanvas.SetActive(false);
        _loseCanvas.SetActive(false);
    }

    /// <summary>
    /// Used by the Player Controller to end the game and select which canvas object should pop up 
    /// </summary>
    /// <param name="lost"></param>
    public void EndGame(bool lost)
    {
        if (lost) { _loseCanvas.SetActive(true); }
        else { _winCanvas.SetActive(true); }
    }

    /// <summary>
    /// Used by the button to reload the level 
    /// </summary>
    public void ReloadLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Used by the button to send the player back to the main menu 
    /// </summary>
    public void ToTitleScreen()
    {
        SceneManager.LoadScene(menuName);
    }
    
    
}
