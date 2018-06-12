using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject FPC; // First Person 
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }	
	}

    public void Resume()
    {
        FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.lockCursor = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
 //        Cursor.visible = false;
    }

    void Pause()
    {
        FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.lockCursor = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;
       

        //        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
