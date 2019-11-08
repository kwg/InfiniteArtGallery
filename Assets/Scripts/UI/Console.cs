using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{

    public bool consoleIsActive;
    public string consoleInput;
    public InputField consoleInputField;
    public string[] consoleInputArray;
    public HUD hud;
 
    
    // Start is called before the first frame update
    void Start()
    {
            consoleIsActive = false;
    }

    public void ToggleConsole()
    {
        consoleIsActive = !consoleIsActive;
        ActivateConsole(consoleIsActive);
    }

    void ActivateConsole(bool on)
    {
        gameObject.SetActive(consoleIsActive);
        if (on)
        {
            Cursor.lockState = CursorLockMode.Confined;
            consoleInputField.ActivateInputField();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(consoleInputField.gameObject, null);
            consoleInputField.OnPointerClick(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
            //Cursor.visible = true;
        }
        else
        {
            consoleInputField.DeactivateInputField();
            Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = false;
        }
        consoleIsActive = on;
    }

    public Text text;

    public void ProcessTerminalInput()
    {
        string input = consoleInputField.text;
        consoleInputArray = consoleInputField.text.Split(" "[0]);
        string command = consoleInputArray[0];
        string value = "";
        if (consoleInputArray.Length > 1)
        {
            value = consoleInputArray[1];
        }
        consoleInputField.text = "";

        switch (command)
        {
            case "test":
                Debug.Log("Test command entered");
                ActivateConsole(false);
                break;

            default:
                ActivateConsole(false);
                break;
        }


    }

    // Update is called once per frame
        void Update()
    {

    }
}
