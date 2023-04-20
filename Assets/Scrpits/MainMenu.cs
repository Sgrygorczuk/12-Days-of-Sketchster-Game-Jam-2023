using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelName;
    public GameObject mainCanvas;
    public GameObject secondCanvas;

    public GameObject setOne;
    public GameObject setTwo;
    public GameObject setThree;
    public GameObject setFour;
    
    public void ToLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void ToSecondCanvas()
    {
        mainCanvas.SetActive(false);
        secondCanvas.SetActive(true);
    }

    public void BackToMainCanvas()
    {
        mainCanvas.SetActive(true);
        secondCanvas.SetActive(false);
        
    }

    public void GoToStory()
    {
        setOne.SetActive(false);
        setTwo.SetActive(true);
    }

    public void GoToInstructions()
    {
        setOne.SetActive(false);
        setThree.SetActive(true);
    }

    public void GoToCredits()
    {
        setOne.SetActive(false);
        setFour.SetActive(true);
    }

    public void BackToSetOne()
    {
        setOne.SetActive(true);
        setTwo.SetActive(false);
        setThree.SetActive(false);
        setFour.SetActive(false);
    }

}
