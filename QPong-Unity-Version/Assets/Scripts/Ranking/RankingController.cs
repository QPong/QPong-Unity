using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingController : MonoBehaviour
{
   public int timeToExit = 10;

    bool startWasPressed = false;

    private void Awake() {
        startWasPressed = false;
        StartCoroutine(CountdownToExit());
    }

    void Update()
    {
        //Player can hit any button to back to main menu
        if (Input.anyKeyDown) {
            startWasPressed = true;
            GameController.Instance.LoadMainMenu();
        }
    }

    IEnumerator CountdownToExit() {
        yield return new WaitForSeconds(timeToExit);
        if (!startWasPressed) {
            GameController.Instance.LoadMainMenu();
        }
    }
}
