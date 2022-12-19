//------------------------------------------------------------------------------
//
// File Name:	MenuButton.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ButtonFunctions
{
    NewGame,
    Exit,
    Continue,
    MainMenu,
    Resume
}

public class MenuButton : MonoBehaviour
{
    // Options for what to do when button is clicked
    public ButtonFunctions buttonFunction;

    // Function to call when button is clicked
    private delegate void ButtonAction();
    private ButtonAction buttonAction;

    // Start is called before the first frame update
    private void Start()
    {
        buttonAction = null;

        switch (buttonFunction)
        {
            case ButtonFunctions.NewGame:
                buttonAction = Play;
                break;
            case ButtonFunctions.Exit:
                buttonAction = Exit;
                break;
            case ButtonFunctions.MainMenu:
                buttonAction = MainMenu;
                break;
            case ButtonFunctions.Resume:
                buttonAction = Resume;
                break;
        }
    }

    public void OnButtonClicked()
    {
        buttonAction();
    }

    private void Play()
    {
        GameManager.loadFromSave = false;
        SceneManager.LoadScene(1); //load the main play scene
    }

    private void Exit()
    {
        Application.Quit();
    }
    private void MainMenu()
    {
        SceneManager.LoadScene(0); //load the main menu
    }
    public void PlaySound(AudioClip ac)
    {
        GetComponent<AudioSource>().PlayOneShot(ac);
    }
    private void Resume()
    {
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("PauseMenu").SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}