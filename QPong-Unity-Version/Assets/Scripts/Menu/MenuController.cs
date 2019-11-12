using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuController : MonoBehaviour
{
    public int timeToExit = 10;

    bool startWasPressed = false;
    float startButtonPressCount = 0f;
    private ArcadeAPIController arcadeAPIController;

    private void Awake() {
        startWasPressed = false;
        StartCoroutine(CountdownToRanking());
        arcadeAPIController = GetComponent<ArcadeAPIController>();
        print("arcade " + arcadeAPIController);
        arcadeAPIController.InitGame("QPong");
    }

    void Update()
    {
        //Player needs to hit the Start button to start the game instead of any button
        if (Input.GetButtonDown("Start"))
        {
            //the game or maybe even close it out and go back to the app selection screen
            startButtonPressCount = Time.time;
            print("Start button press " + startButtonPressCount);

        }
        if (Input.GetButtonDown("Menu"))
        {
            print("QUIT APP!!");
            System.Diagnostics.Process.Start("osascript", "-e 'tell application \"Quantum Arcade\" to activate'");
            Application.Quit();
        }

        if (Input.GetButtonUp("Start"))
        {

            startButtonPressCount = Math.Abs(Time.time - startButtonPressCount);

            print("time down " + startButtonPressCount);

            if (startButtonPressCount >= 3.0f)
            {
                print("QUIT APP!!");
                startButtonPressCount = 0f;

                System.Diagnostics.Process.Start("osascript", "-e 'tell application \"AppMenu\" to activate'");
                Application.Quit();
            }else
            {
                GameController.Instance.StartGame();
            }
            startButtonPressCount = 0f;
        }
    }

     IEnumerator CountdownToRanking() {
        yield return new WaitForSeconds(timeToExit);
        if (!startWasPressed) {
            GameController.Instance.ShowRanking();
        }
    }
}
