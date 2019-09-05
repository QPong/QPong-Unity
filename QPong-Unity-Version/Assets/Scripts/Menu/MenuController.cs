using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public int timeToExit = 10;

    bool startWasPressed = false;
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
        if (Input.GetButtonDown("Start")) {
            GameController.Instance.StartGame();
        }
    }

     IEnumerator CountdownToRanking() {
        yield return new WaitForSeconds(timeToExit);
        if (!startWasPressed) {
            GameController.Instance.ShowRanking();
        }
    }
}
