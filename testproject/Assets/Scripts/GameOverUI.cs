using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    private void Awake()
    {
        //gameObject.SetActive(false); // Sets the UI to hidden initially
    }

    public void OpenGameOverScreen()
    {
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("World Scene");
    }
}
