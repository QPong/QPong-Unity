using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public int timeToExit = 10;

    bool startWasPressed = false;

    private void Awake() {
        startWasPressed = false;
        StartCoroutine(CountdownToRanking());
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
