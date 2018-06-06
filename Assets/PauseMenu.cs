using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Resume()
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
        // FIXME OMG I fixed the invert mouse!
        FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity =
            FPC.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.YSensitivity * -1;

        //        Cursor.visible = true;
    }
}
